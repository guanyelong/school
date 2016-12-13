using SP.Business.HIS;
using SP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP.HIS.Controllers
{
    public class UserController : Controller
    {
        private UserInfoBLL appUserBll = new UserInfoBLL();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增编辑的对话框页面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Dialog(int Id)
        {
            if (Id > 0)
            {
                SYS_USERINFO user = appUserBll.GetAllAppUserById(Id);
                if (user == null)
                {
                    user = new SYS_USERINFO();
                }
                return View(user);
            }

            return View(new SYS_USERINFO());
        }

        public ActionResult ChangePasswordDlg(int Id)
        {
            if (Id > 0)
            {
                SYS_USERINFO user = appUserBll.GetAllAppUserById(Id);
                if (user == null)
                {
                    user = new SYS_USERINFO();
                }
                return View(user);
            }

            return View(new SYS_USERINFO());
        }

        /// <summary>
        /// 获取分页的用户数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAppUserList()
        {
            int pageIndex = int.Parse(Request["page"]);  //当前页  
            int pageSize = int.Parse(Request["rows"]);  //页面行数
            string name = Request["name"];
            int count = 0;
            List<SYS_USERINFO> appUserList = appUserBll.GetAppUserList(pageIndex, pageSize, name, ref count);
            if (appUserList != null)
            {

                var result = from appUser in appUserList
                             select new
                             {
                                 ID = appUser.ID,
                                 appUser.NAME,
                                 appUser.Tel,
                                 appUser.Email,
                                 CreateTime = Convert.ToDateTime(appUser.CreateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                                 appUser.HeadImg,
                                 appUser.LoginName,
                                 appUser.QQ,
                                 appUser.Position,
                                 appUser.DepartmentID,
                                 appUser.DepartmentName
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
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUser(SYS_USERINFO user)
        {
            if (user == null)
            {
                return Json(new { result = "error", mesage = "用户数据为空" });
            }
            string errMsg = "";
            if (user.ID == 0)
            {
                user.CreateTime = DateTime.Now;
                //add
                appUserBll.AddUser(user, ref errMsg);

                //Common.LogHelper.InsertLog(String.Format("新增用户,ID-{0}", user.ID.ToString()), 43, "后台用户");
            }
            else
            {

                //edit
                appUserBll.EditUser(user, ref errMsg);
                //Common.LogHelper.InsertLog(String.Format("编辑用户,ID-{0}", user.ID.ToString()), 43, "后台用户");
            }

            var result = new { result = "ok", message = "操作成功" };

            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult DeleteUser(int userId)
        {
            if (userId == 0)
            {
                return Json(new { result = "error", mesage = "用户编号为空" });
            }

            string errMsg = string.Empty;
            appUserBll.DeleteUser(userId, ref errMsg);

            var result = new { result = "ok", message = "操作成功" };
            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }

            //Common.LogHelper.InsertLog(String.Format("删除用户,ID-{0}", userId.ToString()), 43, "后台用户");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangePassword(int userId, string password)
        {
            if (userId == 0)
            {
                return Json(new { result = "error", mesage = "用户编号为空" });
            }

            string errMsg = string.Empty;
            appUserBll.ChangePassword(userId, password, ref errMsg);

            var result = new { result = "ok", message = "修改密码成功" };
            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }


            //Common.LogHelper.InsertLog(String.Format("修改用户密码,ID-{0}", userId.ToString()), 43, "后台用户");
            return Json(result, "text/html", JsonRequestBehavior.AllowGet);
        }
    }
}