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
        public static UserInfo CurrentUser
        {
            get
            {
                return HttpContext.Current.Session["AdminCurrentUser"] as UserInfo;
            }
            set { HttpContext.Current.Session["AdminCurrentUser"] = value; }
        }

        /// <summary>
        /// 当前登录用户的角色列表
        /// </summary>
        public static List<RoleInfo> CurrentUserRoleList
        {
            get
            {
                List<RoleInfo> roleList = HttpContext.Current.Session["CurrentUserRoleList"] as List<RoleInfo>;
                if (roleList == null)
                {
                    roleList = new List<RoleInfo>();
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
        public static List<ActionInfo> ButtonActionList
        {
            get
            {
                List<ActionInfo> actionList = HttpContext.Current.Session["UserButtonAction"] as List<ActionInfo>;
                if (actionList == null)
                {
                    return new List<ActionInfo>();
                }
                return actionList;
            }
            set { HttpContext.Current.Session["UserButtonAction"] = value; }
        }
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        public static List<ActionInfo> MenuActionList
        {
            get
            {
                List<ActionInfo> actionList = HttpContext.Current.Session["UserMenuAction"] as List<ActionInfo>;
                if (actionList == null)
                {
                    return new List<ActionInfo>();
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
        public static List<ActionInfo> GetUserActionList(string actionNum)
        {

            var action = MenuActionList.Where(o => o.ActionNum == actionNum).FirstOrDefault();

            if (action == null)
            {
                return new List<ActionInfo>();
            }

            var actionList = ButtonActionList.Where(o => o.ParentID == action.ID);
            if (actionList != null && actionList.Count() > 0)
            {
                return actionList.ToList();
            }

            return new List<ActionInfo>();
        }

        /// <summary>
        /// 更新菜单权限
        /// </summary>
        /// <param name="actionList"></param>
        public static void UpdateActionList(List<ActionInfo> actionList)
        {

            AdminSystemInfo.ButtonActionList = actionList.Where(o => o.Type == 0).ToList();
            AdminSystemInfo.MenuActionList = actionList.Where(o => o.Type == 1).ToList();
        }

        /// <summary>
        /// 获取当前登录用户的角色数据
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUserRoleList(List<RoleInfo> roleList)
        {

            AdminSystemInfo.CurrentUserRoleList = roleList;
        }
    }
}
