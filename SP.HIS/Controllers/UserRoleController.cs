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

            List<ActionInfo> actionList = AdminSystemInfo.GetUserActionList(actionNum);
            return Json(new
            {
                result = "ok",
                data = actionList,
                message = "查询成功"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}