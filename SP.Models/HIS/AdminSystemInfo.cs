using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SP.Models.HIS
{
    public  partial class AdminSystemInfo
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public static SYS_USERINFO CurrentUser
        {
            get
            {
                return HttpContext.Current.Session["AdminCurrentUser"] as SYS_USERINFO;
            }
            set { HttpContext.Current.Session["AdminCurrentUser"] = value; }
        }

        /// <summary>
        /// 当前登录用户的角色列表
        /// </summary>
        public static List<SYS_ROLE> CurrentUserRoleList
        {
            get
            {
                List<SYS_ROLE> roleList = HttpContext.Current.Session["CurrentUserRoleList"] as List<SYS_ROLE>;
                if (roleList == null)
                {
                    roleList = new List<SYS_ROLE>();
                }
                return roleList;
            }
            set
            {
                HttpContext.Current.Session["CurrentUserRoleList"] = value;
            }
        }
        /// <summary>
        /// 获取按钮权限列表
        /// </summary>
        public static List<SYS_Action> ButtonActionList
        {
            get
            {
                List<SYS_Action> actionList = HttpContext.Current.Session["UserButtonAction"] as List<SYS_Action>;
                if (actionList == null)
                {
                    return new List<SYS_Action>();
                }
                return actionList;
            }
            set { HttpContext.Current.Session["UserButtonAction"] = value; }
        }
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        public static List<SYS_Action> MenuActionList
        {
            get
            {
                List<SYS_Action> actionList = HttpContext.Current.Session["UserMenuAction"] as List<SYS_Action>;
                if (actionList == null)
                {
                    return new List<SYS_Action>();
                }
                return actionList;
            }
            set { HttpContext.Current.Session["UserMenuAction"] = value; }
        }

        /// <summary>
        /// 获取当前登录用户指定权限下的ActionList
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="actionNum"></param>
        /// <returns></returns>
        public static List<SYS_Action> GetUserActionList(string actionNum)
        {

            var action = MenuActionList.Where(o => o.ActionNum == actionNum).FirstOrDefault();

            if (action == null)
            {
                return new List<SYS_Action>();
            }

            var actionList = ButtonActionList.Where(o => o.ParentID == action.ID);
            if (actionList != null && actionList.Count() > 0)
            {
                return actionList.ToList();
            }

            return new List<SYS_Action>();
        }

        /// <summary>
        /// 更新菜单权限
        /// </summary>
        /// <param name="actionList"></param>
        public static void UpdateActionList(List<SYS_Action> actionList)
        {

            AdminSystemInfo.ButtonActionList = actionList.Where(o => o.Type == 0).ToList();
            AdminSystemInfo.MenuActionList = actionList.Where(o => o.Type == 1).ToList();
        }

        /// <summary>
        /// 获取当前登录用户的角色数据
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUserRoleList(List<SYS_ROLE> roleList)
        {

            AdminSystemInfo.CurrentUserRoleList = roleList;
        }
    }
}
