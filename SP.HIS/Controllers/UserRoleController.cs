using SP.Business.HIS;
using SP.Models;
using SP.Models.HIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP.HIS.Controllers
{
    public class UserRoleController : Controller
    {
        private UserRoleBLL userRoleBll = new UserRoleBLL();
        // GET: UserRole
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户的指定菜单下按钮权限列表
        /// </summary>
        /// <param name="actionNum"></param>
        /// <returns></returns>
        public JsonResult GetUserButtonAction(string actionNum)
        {
            //基本验证
            if (string.IsNullOrEmpty(actionNum))
            {
                return Json(new
                {
                    result = "error",
                    message = "传入的权限编码为空"
                });
            }

            List<SYS_Action> actionList = AdminSystemInfo.GetUserActionList(actionNum);
            return Json(new
            {
                result = "ok",
                data = actionList,
                message = "查询成功"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询用户角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public JsonResult GetAppUserRoleList()
        {
            int pageIndex = int.Parse(Request["page"]);  //当前页  
            int pageSize = int.Parse(Request["rows"]);  //页面行数
            string name = Request["name"];
            string userID = Request["userId"];

            if (string.IsNullOrEmpty(userID))
            {
                userID = "0";
            }
            int count = 0;

            List<SYS_ROLE> appRoleList = userRoleBll.GetAppUserRoleList(pageIndex, pageSize, name, Convert.ToInt32(userID), ref count);
            if (appRoleList == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var result = from appRole in appRoleList
                         select new
                         {
                             ID = appRole.ID,
                             appRole.ROLENUM,
                             appRole.ROLENAME,
                             //appRole.Disc,
                             CreateTime = Convert.ToDateTime(appRole.CREATETIME).ToString("yyyy-MM-dd HH:mm:ss"),

                         };

            var data = new
            {
                total = count,
                rows = result
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteUserRole(int userid, int roleid)
        {
            if (userid < 1 || roleid < 1)
            {
                return Json(new
                {
                    result = "error",
                    message = "用户和角色错误"
                });
            }

            string errMsg = string.Empty;
            userRoleBll.DeleteUserRole(userid, roleid, ref errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return Json(new
                {
                    result = "error",
                    message = errMsg
                });
            }

            return Json(new
            {
                result = "success",
                message = "设置角色成功"
            });
        }

    }
}