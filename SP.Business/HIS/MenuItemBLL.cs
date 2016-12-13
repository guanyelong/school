using SP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class MenuItemBLL
    {
        //根据ID获取菜单编号
        public SYS_ITEMMENU GetAppMenuByID(int Id, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var menuItem = appEntities.SYS_ITEMMENU.Where(o => o.ID == Id).FirstOrDefault();
                    if (menuItem == null)
                    {
                        errMsg = "查无数据";
                        return null;
                    }
                    return menuItem;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="errMsg"></param>
        public void AddMenu(SYS_ITEMMENU menuItem, ref string errMsg)
        {
            try
            {
                using (HISDataEntities hisEntities = new HISDataEntities())
                {
                    menuItem.MenuNum = CreateMenuNum(menuItem);
                    menuItem.CREATETIME = DateTime.Now;
                    menuItem.STATE = "open";

                    //更新上级菜单state 为 closeed
                    var findItem = hisEntities.SYS_ITEMMENU.Where(o => o.ID == menuItem.ParentID).FirstOrDefault();
                    if (findItem != null)
                    {
                        findItem.STATE = "closed";
                    }

                    menuItem.CHECKED = true;
                    menuItem.SEQ = 1;//排序

                    hisEntities.SYS_ITEMMENU.Add(menuItem);
                    hisEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }


        private string CreateMenuNum(SYS_ITEMMENU menu) {
            string actionCount = "01";
            string menuNum = "MN";
            SYS_ITEMMENU parentMenu = null;
            SYS_ITEMMENU lastMenu = null;
            using (HISDataEntities hisEntities = new HISDataEntities())
            {
                //查找上级菜单
                parentMenu = hisEntities.SYS_ITEMMENU.Where(o => o.ID == menu.ParentID).FirstOrDefault();
                //查询菜单下最大编号不包括自己和已经删除的
                lastMenu = hisEntities.SYS_ITEMMENU.Where(o => o.ParentID == menu.ParentID).OrderByDescending(o => o.MenuNum).FirstOrDefault();
            }
            if (lastMenu != null)
            {
                var str = lastMenu.MenuNum.Substring(lastMenu.MenuNum.Length - 2, 2);
                int number = Convert.ToInt32(str) + 1;
                actionCount = number.ToString().PadLeft(2, '0');
            }

            if (parentMenu != null)
            {
                menuNum = parentMenu.MenuNum + actionCount;
                return menuNum;
            }
            return menuNum + actionCount;
        }
        /// <summary>
        /// 获取ComboTree Grid数据
        /// </summary>
        /// <param name="count"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public object GetMenuComboTree(ref int count, ref string errMsg)
        {
            try
            {
                List<SYS_ITEMMENU> menuList = new List<SYS_ITEMMENU>();
                List<SYS_ITEMMENU> parentList = new List<SYS_ITEMMENU>();
                using (HISDataEntities appEntitys = new HISDataEntities())
                {
                    menuList = appEntitys.SYS_ITEMMENU.ToList();

                }
                parentList = menuList.Where(o => o.ParentID == 0).ToList();
                count = parentList.Count();

                List<Hashtable> tableList = new List<Hashtable>();
                Hashtable noneTable = new Hashtable();
                noneTable.Add("id", 0);
                noneTable.Add("text", "无菜单");
                tableList.Add(noneTable);

                if (count < 1)
                {
                    return tableList;
                }

                foreach (var item in parentList)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("id", item.ID);
                    ht.Add("text", item.TEXT);
                    InitMenuComboChildren(menuList, ht, item.ID);
                    tableList.Add(ht);
                }
                return tableList;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return new List<Hashtable>();
            }
        }
        private void InitMenuComboChildren(List<SYS_ITEMMENU> appMenus, Hashtable ht, int parentId)
        {
            List<SYS_ITEMMENU> menuList = appMenus.Where(o => o.ParentID == parentId).ToList();
            int childrenCount = menuList.Count();
            if (childrenCount > 0)
            {
                List<Hashtable> cList = new List<Hashtable>();
                foreach (var item in menuList)
                {
                    Hashtable cht = new Hashtable();
                    cht.Add("id", item.ID);
                    cht.Add("text", item.TEXT);
                    InitMenuComboChildren(appMenus, cht, item.ID);
                    cList.Add(cht);
                }
                ht.Add("children", cList);
            }
        }
        /// <summary>
        /// 获取GridTree
        /// </summary>
        /// <param name="count"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public object GetAppMenuGridTree(ref int count, ref string errMsg)
        {
            try
            {
                List<SYS_ITEMMENU> queryList = null;
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    queryList = appEntities.SYS_ITEMMENU.OrderBy(o => o.ORDERINDEX).ToList();
                    count = queryList.Count();
                    if (count < 1)
                    {
                        return null;
                    }
                }

                List<Hashtable> htList = new List<Hashtable>();
                var parentList = queryList.Where(o => o.ParentID == 0).OrderBy(o => o.ORDERINDEX);
                foreach (SYS_ITEMMENU actionItem in parentList)
                {
                    Hashtable ht = InitMenuGridTreeChildren(queryList, actionItem);
                    htList.Add(ht);
                }
                return htList;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                count = 0;
                return null;
            }
        }
        private Hashtable InitMenuGridTreeChildren(List<SYS_ITEMMENU> menuList, SYS_ITEMMENU item)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", item.ID);
            ht.Add("Text", item.TEXT);
            ht.Add("MenuNum", item.MenuNum);
            ht.Add("Url", item.URL);
            ht.Add("IconCls", item.ICON);
            //ht.Add("Disc", item.Disc);
            ht.Add("MenuState", item.STATE);
            ht.Add("CreateTime", Convert.ToDateTime(item.CREATETIME).ToString("yyyy-MM-dd HH:mm:ss"));
            ht.Add("ParentID", item.ParentID);
            ht.Add("OrderIndex", item.ORDERINDEX);

            var childrenList = menuList.Where(o => o.ParentID == item.ID).OrderBy(o => o.ORDERINDEX);
            if (childrenList.Count() > 0)
            {
                List<Hashtable> htList = new List<Hashtable>();
                foreach (SYS_ITEMMENU childrenItem in childrenList)
                {
                    Hashtable childrenHt = InitMenuGridTreeChildren(menuList, childrenItem);
                    htList.Add(childrenHt);
                }
                ht.Add("children", htList);
            }
            return ht;

        }
        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="errMsg"></param>
        public void EditMenu(SYS_ITEMMENU menuItem, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var findItem = appEntities.SYS_ITEMMENU.Where(o => o.ID == menuItem.ID).FirstOrDefault();

                    findItem.TEXT = menuItem.TEXT;
                    findItem.ParentID = menuItem.ParentID;
                    findItem.MenuNum = menuItem.MenuNum;
                    //findItem.Disc = menuItem.Disc;
                    findItem.ICON = menuItem.ICON;
                    findItem.URL = menuItem.URL;
                    findItem.ORDERINDEX = menuItem.ORDERINDEX;

                    appEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="errMsg"></param>
        public void DeleteMenu(int menuId, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var menuItem = appEntities.SYS_ITEMMENU.Where(o => o.ID == menuId).FirstOrDefault();
                    if (menuId == null)
                    {
                        errMsg = "查无菜单";
                        return;
                    }

                    appEntities.SYS_ITEMMENU.Remove(menuItem);
                    appEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }
    }
}
