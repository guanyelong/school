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
    public class MenuController : Controller
    {
        MenuItemBLL menuBll = new MenuItemBLL();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dialog(int Id)
        {
            if (Id < 1)
            {
                SYS_ITEMMENU addItem = new SYS_ITEMMENU() { MenuNum = "自动生成" };
                return View(addItem);
            }

            string errMsg = string.Empty;
            SYS_ITEMMENU menuItem = menuBll.GetAppMenuByID(Id, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return View(new SYS_ITEMMENU());
            }
            return View(menuItem);

        }
        public JsonResult GetMenuComboTree()
        {

            string errMsg = string.Empty;
            int count = 0;
            //构造combobox tree 需要的数据
            var menuTree = menuBll.GetMenuComboTree(ref count, ref errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                menuTree = new List<Hashtable>();
            }
            return Json(menuTree, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取Department TreeGrid 数据格式
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAppMenuGridTree()
        {

            int count = 0;
            string errMsg = string.Empty;

            var menuTree = menuBll.GetAppMenuGridTree(ref count, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                menuTree = new List<Hashtable>();
            }

            return Json(menuTree, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveMenu(SYS_ITEMMENU menuItem)
        {
            if (menuItem == null)
            {
                return Json(new { result = "error", message = "部门数据不正确" });
            }
            string errMsg = "";
            if (menuItem.ParentID==null)
            {
                menuItem.ParentID = 0;
            }
            if (menuItem.ID == 0)
            {
                menuItem.CREATETIME = DateTime.Now;
                //add
                menuBll.AddMenu(menuItem, ref errMsg);
                //Common.LogHelper.InsertLog(String.Format("新增菜单,ID-{0}", menuItem.ToString()), 50, "菜单列表");
            }
            else
            {
                //edit
                menuBll.EditMenu(menuItem, ref errMsg);
                //Common.LogHelper.InsertLog(String.Format("编辑菜单,ID-{0}", menuItem.ToString()), 50, "菜单列表");
            }

            var result = new { result = "ok", message = "操作成功" };

            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public JsonResult DeleteMenu(int menuId)
        {
            if (menuId == 0)
            {
                return Json(new { result = "error", mesage = "用户编号为空" });
            }

            string errMsg = string.Empty;
            menuBll.DeleteMenu(menuId, ref errMsg);

            var result = new { result = "ok", message = "操作成功" };
            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }
            //Common.LogHelper.InsertLog(String.Format("删除菜单,ID-{0}", menuId.ToString()), 50, "菜单列表");
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}