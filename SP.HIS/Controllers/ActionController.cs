using SP.Business.HIS;
using SP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP.HIS.Controllers
{
    public class ActionController : Controller
    {
        private ActionBLL actionBLL = new ActionBLL();
        // GET: Action
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RoleAction()
        {

            return View();
        }
        /// <summary>
        /// 新增编辑弹窗
        /// </summary>
        /// <returns></returns>
        public ActionResult Dialog()
        {
            string actionId = Request["Id"];
            if (string.IsNullOrEmpty(actionId) || actionId=="0")
            {
                SYS_Action ac = new SYS_Action() { ActionNum = "自动生成"};
                return View(ac);
            }
            string errMsg = string.Empty;
            SYS_Action actionModel = actionBLL.GetActionByID(Convert.ToInt32(actionId), ref errMsg);
            if (string.IsNullOrEmpty(errMsg))
            {
                //记录操作日志
                //Common.LogHelper.InsertLog(String.Format("新增权限,ID-{0}", actionModel.ID.ToString()), 51, "权限列表");
                return View(actionModel);
            }
            return View(new SYS_Action());
        }
        /// <summary>
        /// 角色权限页面，设置权限弹窗
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRoleAction()
        {
            string roleID = Request["roleid"];

            if (string.IsNullOrEmpty(roleID))
            {
                return View(new SYS_ROLE());
            }
            string errMsg = string.Empty;
            SYS_ROLE role = new RoleInfoBLL().GetAppRoleByID(Convert.ToInt32(roleID), ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return View(new SYS_ROLE());
            }

            return View(role);
        }
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActionGridTreeList()
        {
            int count = 0;
            List<Hashtable> actionList = actionBLL.GetAppActionTreeGridList(ref count);

            if (actionList == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var data = new
            {
                total = count,
                rows = actionList
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="actionItem"></param>
        /// <returns></returns>
        public JsonResult SaveAction(SYS_Action actionItem)
        {
            if (actionItem == null)
            {
                return Json(new { result = "error", mesage = "角色数据为空" });
            }
            string errMsg = "";
            if (actionItem.ID == 0)
            {
                actionItem.CREATETIME = DateTime.Now;
                //add
                actionBLL.AddAction(actionItem, ref errMsg);
            }
            else
            {
                //edit
                actionBLL.EditAction(actionItem, ref errMsg);
            }

            var result = new { result = "ok", message = "操作成功" };

            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }
            //Common.LogHelper.InsertLog(String.Format("修改权限,ID-{0}", actionItem.ID.ToString()), 51, "权限列表");
            //兼容ie
            return Json(result, "text/html", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActionComboTree()
        {
            int count = 0;
            var actionList = actionBLL.GetAppActionComboTreeList(ref count);

            if (actionList == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(actionList, JsonRequestBehavior.AllowGet);
        }


        
    }
}