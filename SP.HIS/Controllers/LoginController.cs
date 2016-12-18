using SP.Business.HIS;
using SP.Models;
using SP.Models.HIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SP.HIS.Controllers
{
    public class LoginController : Controller
    {
        private UserRoleBLL userRole = new UserRoleBLL();
        private UserInfoBLL userBLL = new UserInfoBLL();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(SYS_USERINFO info)
        {
            JsonResult ret = new JsonResult();
            SYS_USERINFO findUser = userBLL.ValidateLogin(info);
            if (findUser!=null)
            {
                AdminSystemInfo.CurrentUser = findUser;
                FormsAuthentication.SetAuthCookie(findUser.LoginName, false);
                string url = FormsAuthentication.GetRedirectUrl(findUser.LoginName, false);
                ret.Data = new { ifOk = true, url = url };
                //获取用户的所有按钮权限列表存放到Session
                LoginSuccess(findUser.ID);
                //Common.LogHelper.InsertLog("登录成功！");
            }
            return ret;
        }

        private void LoginSuccess(int userId)
        {
            //设置用户的登录按钮权限 
            List<SYS_Action> actionList = userRole.GetAppUserActionList(userId, AppActionType.AllAction);
            if (actionList == null)
            {
                actionList = new List<SYS_Action>();
            }
            AdminSystemInfo.UpdateActionList(actionList);

            //设置当前用户的角色信息
            string errMsg = string.Empty;
            List<SYS_ROLE> roleList = userRole.GetAppUserRoleList(userId, ref errMsg);
            AdminSystemInfo.UpdateUserRoleList(roleList);
        }
    }
}