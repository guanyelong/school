using SP.Models;
using System;
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

    }
}
