using SP.Business.HIS;
using SP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP.HIS.Controllers
{
    public class RoleController : Controller
    {
        private RoleInfoBLL roleBll = new RoleInfoBLL();
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 角色弹窗编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Dialog(int Id)
        {
            if (Id < 1)
            {
                SYS_ROLE r = new SYS_ROLE() { ROLENUM= "自动生成" };
                return View(r);
            }

            string errMsg = string.Empty;
            SYS_ROLE role = roleBll.GetAppRoleByID(Id, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return View(new SYS_ROLE());
            }

            return View(role);
        }
        /// <summary>
        /// 角色用户界面
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleUser()
        {
            return View();
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult SetUserRole()
        {

            string userid = Request["userid"];
            if (string.IsNullOrEmpty(userid))
            {
                return View(new SYS_USERINFO());
            }
            
            SYS_USERINFO user = new UserInfoBLL().GetAllAppUserById(Convert.ToInt32(userid));

            return View(user);
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAppRoleList()
        {
            int pageIndex = int.Parse(Request["page"]);  //当前页  
            int pageSize = int.Parse(Request["rows"]);  //页面行数
            string name = Request["name"];
            int count = 0;
            List<SYS_ROLE> appRoleList = roleBll.GetAppRoleList(pageIndex, pageSize, ref count, name);
            if (appRoleList != null)
            {
                var result = from appRole in appRoleList
                             select new
                             {
                                 ID = appRole.ID,
                                 appRole.ROLENAME,
                                 CreateTime = Convert.ToDateTime(appRole.CREATETIME).ToString("yyyy-MM-dd HH:mm:ss"),
                                 //appRole.Disc,
                                 appRole.ROLENUM,
                                 appRole.FLAG
                             };

                var data = new
                {
                    total = count,
                    rows = result
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    total = 0,
                    rows = new List<SYS_ROLE>()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public JsonResult SaveRole(SYS_ROLE roleInfo)
        {
            if (roleInfo == null)
            {
                return Json(new { result = "error", mesage = "角色数据为空" });
            }
            string errMsg = "";
            if (roleInfo.ID == 0)
            {
                roleInfo.CREATETIME = DateTime.Now;
                //add
                roleBll.AddRole(roleInfo, ref errMsg);

                //Common.LogHelper.InsertLog(String.Format("新增角色,ID-{0}", roleInfo.ID.ToString()), 52, "角色列表");
            }
            else
            {
                //edit
                roleBll.EditRole(roleInfo, ref errMsg);

                //Common.LogHelper.InsertLog(String.Format("编辑角色,ID-{0}", roleInfo.ID.ToString()), 52, "角色列表");
            }

            var result = new { result = "ok", message = "操作成功" };

            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public JsonResult DeleteRole(int roleId)
        {
            if (roleId == 0)
            {
                return Json(new { result = "error", mesage = "角色编号为空" });
            }

            string errMsg = string.Empty;
            roleBll.DeleteRole(roleId, ref errMsg);

            var result = new { result = "ok", message = "操作成功" };
            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }
            //Common.LogHelper.InsertLog(String.Format("删除角色,ID-{0}", roleId.ToString()), 52, "角色列表");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}