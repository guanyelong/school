using SP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class MenuItemBLL
    {
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
            string menuNum = string.Empty;
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
    }
}
