using SP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public  class ActionBLL
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="actionItem"></param>
        /// <param name="errMsg"></param>
        public void AddAction(SYS_Action actionItem, ref string errMsg)
        {
            try
            {
                string actionNum = CreateActionNum(actionItem);
                //创建权限
                actionItem.ActionNum = actionNum;
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    actionItem.CREATETIME = DateTime.Now;

                    //新增权限名称不能重复
                    var findList = appEntities.SYS_Action.Where(o => o.ActionName == actionItem.ActionName && o.ParentID == actionItem.ParentID);
                    if (findList.Count() > 0)
                    {
                        errMsg = "权限名称已经存在";
                        return;
                    }

                    if (actionItem.Type == 1)
                    {
                        //给菜单编写权限编码
                        var appMenu = appEntities.SYS_ITEMMENU.Where(o => o.ID == actionItem.ActionMenu).FirstOrDefault();
                        //如果菜单已被其他权限关联，则不能继续被关联
                        if (!string.IsNullOrEmpty(appMenu.ActionNum))
                        {
                            errMsg = "菜单已被权限" + appMenu.ActionNum + "关联,请选择其他菜单";
                            return;
                        }
                        appMenu.ActionNum = actionItem.ActionNum;
                    }

                    appEntities.SYS_Action.Add(actionItem);
                    appEntities.SaveChanges();
                }


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

        /// <summary>
        /// 自动生成权限编码
        /// </summary>
        /// <param name="actionItem"></param>
        /// <returns></returns>
        private string CreateActionNum(SYS_Action actionItem)
        {
            SYS_Action parentAction = null;
            SYS_Action lastAction = null;
            string actionCount = "01";
            string actionNum = "RT";

            using (HISDataEntities appEntities = new HISDataEntities())
            {
                //查找上级菜单
                parentAction = appEntities.SYS_Action.Where(o => o.ID == actionItem.ParentID).FirstOrDefault();
                //查询菜单下最大编号不包括自己和已经删除的
                lastAction = appEntities.SYS_Action.Where(o => o.ParentID == actionItem.ParentID).OrderByDescending(o => o.ActionNum).FirstOrDefault();
            }

            if (lastAction != null)
            {
                var str = lastAction.ActionNum.Substring(lastAction.ActionNum.Length - 2, 2);
                int number = Convert.ToInt32(str) + 1;
                actionCount = number.ToString().PadLeft(2, '0');
            }

            if (parentAction != null)
            {
                actionNum = parentAction.ActionNum + actionCount;
                return actionNum;
            }
            return actionNum + actionCount;
        }


        /// <summary>
        /// 根据name获取菜单列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Hashtable> GetAppActionTreeGridList(ref int count)
        {
            try
            {
                List<Models.SYS_Action> queryList = null;
                using (HISDataEntities appEntities = new HISDataEntities())
                {

                    queryList = appEntities.SYS_Action.Where(o => o.Flag == 1).ToList();
                    count = queryList.Count();
                    if (count < 1)
                    {
                        return null;
                    }
                }
                List<Hashtable> htList = new List<Hashtable>();
                var parentList = queryList.Where(o => o.ParentID == 0);
                foreach (SYS_Action actionItem in parentList)
                {
                    Hashtable ht = CreateTreeGridList(queryList, actionItem);
                    htList.Add(ht);
                }
                return htList;
            }
            catch (Exception e)
            {
                count = 0;
                return null;
            }
        }
        /// <summary>
        /// 返回查询的数据
        /// </summary>
        /// <param name="queryList"></param>
        /// <param name="resultList"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private Hashtable CreateTreeGridList(List<SYS_Action> queryList, SYS_Action actionItem)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", actionItem.ID);
            ht.Add("ActionName", actionItem.ActionName);
            ht.Add("ActionNum", actionItem.ActionNum);
            //ht.Add("Disc", actionItem.Disc);
            ht.Add("Type", actionItem.Type);
            ht.Add("TypeName", actionItem.Type == 1 ? "菜单权限" : "按钮权限");
            ht.Add("ParentID", actionItem.ParentID);
            ht.Add("CreateTime", Convert.ToDateTime(actionItem.CREATETIME).ToString("yyyy-MM-dd HH:mm:ss"));

            var childrenList = queryList.Where(o => o.ParentID == actionItem.ID);
            if (childrenList.Count() > 0)
            {
                List<Hashtable> htList = new List<Hashtable>();
                foreach (SYS_Action item in childrenList)
                {
                    Hashtable childrenHt = CreateTreeGridList(queryList, item);
                    htList.Add(childrenHt);
                }
                ht.Add("children", htList);
            }
            return ht;
        }

        /// <summary>
        /// 根据ID获取权限Model
        /// </summary>
        /// <param name="p"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public SYS_Action GetActionByID(int actionId, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    SYS_Action actionItem = appEntities.SYS_Action.Where(o => o.ID == actionId && o.Flag == 1).FirstOrDefault();
                    if (actionItem == null)
                    {
                        errMsg = "查无数据";
                        return null;
                    }
                    //根据action num 获取菜单id
                    var appMenu = appEntities.SYS_ITEMMENU.Where(o => o.ActionNum == actionItem.ActionNum).FirstOrDefault();
                    if (appMenu != null)
                    {

                        actionItem.ActionMenu = appMenu.ID;
                    }
                    return actionItem;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
        }
        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="actionItem">最新的权限数据</param>
        /// <param name="errMsg">错误提示</param>
        public void EditAction(SYS_Action actionItem, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    //原来的数据
                    var findItem = (from a in appEntities.SYS_Action
                                    join b in appEntities.SYS_ITEMMENU on a.ActionNum equals b.ActionNum into temp
                                    from b in temp.DefaultIfEmpty()
                                    where a.ID == actionItem.ID
                                    select new { a, b }).FirstOrDefault();

                    SYS_Action updateItem = findItem.a;
                    if (findItem.b != null)
                    {

                        updateItem.ActionMenu = findItem.b.ID;
                    }

                    if (updateItem == null)
                    {
                        errMsg = "查无数据";
                        return;
                    }

                    //关联菜单发生变化
                    //1：菜单权限下关联的菜单发生改变
                    //2: 权限类型按钮权限和控制权限之间互相转换
                    if ((actionItem.ActionMenu != updateItem.ActionMenu) || (actionItem.Type != updateItem.Type))
                    {

                        //如果需要关联的菜单已经被关联了，则限制不能被关联
                        var updateMenu = appEntities.SYS_ITEMMENU.Where(o => o.ID == actionItem.ActionMenu).FirstOrDefault();
                        if (!string.IsNullOrEmpty(updateMenu.ActionNum) && updateMenu.ActionNum != actionItem.ActionNum)
                        {
                            errMsg = "菜单已被权限" + updateMenu.ActionNum + "关联，请选择其他菜单";
                            return;
                        }

                        if (updateItem.Type == 1)
                        {
                            //将原来的关联菜单取消
                            var appMenu = appEntities.SYS_ITEMMENU.Where(o => o.ActionNum == updateItem.ActionNum).FirstOrDefault();
                            if (appMenu != null)
                            {
                                appMenu.ActionNum = string.Empty;
                            }
                        }
                        if (actionItem.Type == 1)
                        {
                            //关联最新的权限Num
                            updateMenu.ActionNum = actionItem.ActionNum;
                        }
                    }

                    updateItem.ActionNum = actionItem.ActionNum;
                    updateItem.ActionName = actionItem.ActionName;
                    updateItem.ActionMenu = actionItem.ActionMenu;
                    //updateItem.Disc = actionItem.Disc;
                    updateItem.ParentID = actionItem.ParentID;
                    updateItem.Type = actionItem.Type;

                    appEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        public void DeleteAction(int actionId, ref string errMsg)
        {
            try
            {
                List<SYS_Action> deleteList = null;
                using (HISDataEntities appEntities = new HISDataEntities())
                {

                    SYS_Action actionItem = appEntities.SYS_Action.Where(o => o.ID == actionId).FirstOrDefault();
                    if (actionItem == null)
                    {
                        errMsg = "查无数据";
                        return;
                    }
                    actionItem.Flag = 0;
                    deleteList = DeleteActionChildren(appEntities, actionId);
                    deleteList.Add(actionItem);
                    //更新菜单中的ActionNum
                    SYS_ITEMMENU findMenu = appEntities.SYS_ITEMMENU.Where(o => o.ActionNum == actionItem.ActionNum).FirstOrDefault();
                    if (findMenu != null)
                    {
                        findMenu.ActionNum = string.Empty;
                    }
                    // 保存
                    appEntities.SaveChanges();
                }

            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }
        //递归获取所有的子权限
        private List<SYS_Action> DeleteActionChildren(HISDataEntities appEntities, int actionId)
        {
            List<SYS_Action> listAction = new List<SYS_Action>();
            var childrenAction = appEntities.SYS_Action.Where(o => o.ParentID == actionId);
            if (childrenAction.Count() > 0)
            {
                foreach (SYS_Action actionItem in childrenAction)
                {
                    listAction.Add(actionItem);
                    listAction.AddRange(DeleteActionChildren(appEntities, actionItem.ID));
                    actionItem.Flag = 0;
                    actionItem.ActionNum = string.Empty;
                }
            }
            return listAction;
        }

        public List<Hashtable> GetAppActionComboTreeList(ref int count)
        {
            try
            {
                List<SYS_Action> queryList = null;
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    queryList = appEntities.SYS_Action.Where(o => o.Flag == 1).ToList();
                    count = queryList.Count();
                }

                List<Hashtable> htList = new List<Hashtable>();
                Hashtable firstItem = new Hashtable();
                firstItem.Add("id", 0);
                firstItem.Add("text", "无上级");
                htList.Add(firstItem);

                if (count < 1)
                {
                    return htList;
                }

                var parentList = queryList.Where(o => o.ParentID == 0);
                foreach (SYS_Action actionItem in parentList)
                {
                    Hashtable ht = CreateComboTreeList(queryList, actionItem);
                    htList.Add(ht);
                }
                return htList;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private Hashtable CreateComboTreeList(List<SYS_Action> queryList, SYS_Action actionItem)
        {
            Hashtable ht = new Hashtable();
            ht.Add("id", actionItem.ID);
            ht.Add("text", actionItem.ActionName);

            var childrenList = queryList.Where(o => o.ParentID == actionItem.ID);
            if (childrenList.Count() > 0)
            {
                List<Hashtable> htList = new List<Hashtable>();
                foreach (SYS_Action item in childrenList)
                {
                    Hashtable childrenHt = CreateComboTreeList(queryList, item);
                    htList.Add(childrenHt);
                }
                ht.Add("children", htList);
            }
            return ht;
        }


    }
}
