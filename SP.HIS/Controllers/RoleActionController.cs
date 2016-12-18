using SP.Business.HIS;
using SP.Models.HIS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP.HIS.Controllers
{
    public class RoleActionController : Controller
    {
        private RoleActionBLL roleActionBLL = new RoleActionBLL();
        // GET: RoleAction
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAppRoleActionList()
        {
            string roleid = Request["roleid"];

            if (string.IsNullOrEmpty(roleid))
            {
                return Json("");
            }
            string errMsg = string.Empty;
            int count = 0;
            List<Hashtable> actionList = roleActionBLL.GetAppRoleActionTreeGrid(Convert.ToInt32(roleid), ref errMsg, ref count);

            var data = new
            {
                total = count,
                rows = actionList
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取设置权限时的权限列表框
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleActionTreeList()
        {
            string roleId = Request["roleid"];

            if (string.IsNullOrEmpty(roleId))
            {
                roleId = "0";
            }

            string errMsg = "";
            List<Hashtable> treeList = roleActionBLL.GetRoleActionRoleTreeList(Convert.ToInt32(roleId), ref errMsg);

            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置角色的权限
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveRoleAction()
        {
            string roleid = Request["roleid"];
            string actionids = Request["actionids"];
            if (string.IsNullOrEmpty(roleid) || string.IsNullOrEmpty(actionids))
            {
                return Json(new { result = "error", message = "参数错误" });
            }

            string errMsg = string.Empty;
            roleActionBLL.SaveRoleAction(Convert.ToInt32(roleid), actionids.Split(','), ref errMsg);
            // 刷新当前用户的权限列表
            UserRoleBLL userRole = new UserRoleBLL();
            //AdminSystemInfo.UpdateActionList(userRole.GetAppUserActionList(AdminSystemInfo.CurrentUser.ID, SP.Models.HIS.AppActionType.AllAction));

            if (!string.IsNullOrEmpty(errMsg))
            {
                return Json(new { result = "error", message = "参数错误" });
            }

            //Common.LogHelper.InsertLog("设置权限", 52, "角色权限");

            return Json(new { result = "ok", message = "权限设置成功" });
        }
    }
}