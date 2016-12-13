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
    public class DepartmentController : Controller
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dialog(int Id)
        {

            if (Id < 1)
            {
                SYS_Department dept = new SYS_Department() {  DepartmentNum= "自动生成" };
                return View(dept);
            }
            string errMsg = string.Empty;
            SYS_Department department = departmentBLL.GetDepartmentByID(Id, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return View(new SYS_Department());
            }
            return View(department);
        }

        /// <summary>
        /// 获取Department数据Tree
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAppDeparmtTree()
        {

            string errMsg = string.Empty;
            int count = 0;
            //构造combobox tree 需要的数据
            var departmentTree = departmentBLL.GetDepartmentTree(ref count, ref errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                departmentTree = new List<Hashtable>();
            }
            return Json(departmentTree, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取Department TreeGrid 数据格式
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAppDepartmentTreeGrid()
        {

            int count = 0;
            string errMsg = string.Empty;


            var departmentTree = departmentBLL.GetDepartmentTreeGridList(ref count, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                departmentTree = new List<Hashtable>();
            }

            return Json(departmentTree, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDepartment(SYS_Department depmentItem)
        {
            if (depmentItem == null)
            {
                return Json(new { result = "error", message = "部门数据不正确" });
            }
            string errMsg = "";
            if (depmentItem.ID == 0)
            {
                depmentItem.CreateTime = DateTime.Now;
                //add
                departmentBLL.AddDepartment(depmentItem, ref errMsg);
                //Common.LogHelper.InsertLog(String.Format("新增部门,ID-{0}", depmentItem.ToString()), 44, "部门列表");
            }
            else
            {
                //edit
                departmentBLL.EditDepartment(depmentItem, ref errMsg);
                //Common.LogHelper.InsertLog(String.Format("编辑部门,ID-{0}", depmentItem.ToString()), 44, "部门列表");
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
        public JsonResult DeleteDepartment(int departmentID)
        {
            if (departmentID == 0)
            {
                return Json(new { result = "error", mesage = "用户编号为空" });
            }

            string errMsg = string.Empty;
            departmentBLL.DeleteDepartment(departmentID, ref errMsg);

            var result = new { result = "ok", message = "操作成功" };
            if (!string.IsNullOrEmpty(errMsg))
            {
                result = new { result = "error", message = errMsg };
            }

            //Common.LogHelper.InsertLog(String.Format("删除部门,ID-{0}", departmentID.ToString()), 44, "部门列表");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}