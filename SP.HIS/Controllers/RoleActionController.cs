using SP.Business.HIS;
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
    }
}