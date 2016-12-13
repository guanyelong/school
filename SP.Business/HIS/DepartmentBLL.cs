using SP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class DepartmentBLL
    {
        /// <summary>
        /// 根据id 获取department
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public SYS_Department GetDepartmentByID(int id, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntitys = new HISDataEntities())
                {
                    SYS_Department department = appEntitys.SYS_Department.Where(o => o.ID == id).FirstOrDefault();
                    if (department != null)
                    {
                        return department;
                    }
                    errMsg = "查无数据";
                    return null;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
        }

        private string CreateDeptNum(SYS_Department dept)
        {
            string actionCount = "01";
            string deptNum = "DT";
            SYS_Department parentMenu = null;
            SYS_Department lastMenu = null;
            using (HISDataEntities hisEntities = new HISDataEntities())
            {
                //查找上级菜单
                parentMenu = hisEntities.SYS_Department.Where(o => o.ID == dept.ParentID).FirstOrDefault();
                //查询菜单下最大编号不包括自己和已经删除的
                lastMenu = hisEntities.SYS_Department.Where(o => o.ParentID == dept.ParentID).OrderByDescending(o => o.DepartmentNum).FirstOrDefault();
            }
            if (lastMenu != null)
            {
                var str = lastMenu.DepartmentNum.Substring(lastMenu.DepartmentNum.Length - 2, 2);
                int number = Convert.ToInt32(str) + 1;
                actionCount = number.ToString().PadLeft(2, '0');
            }

            if (parentMenu != null)
            {
                deptNum = parentMenu.DepartmentNum + actionCount;
                return deptNum;
            }
            return deptNum + actionCount;
        }
        /// <summary>
        /// 获取部门Tree
        /// </summary>
        /// <param name="count"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<Hashtable> GetDepartmentTree(ref int count, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntitys = new HISDataEntities())
                {

                    IQueryable<SYS_Department> departmentList = appEntitys.SYS_Department.Where(o => o.Flag == 1 && o.ParentID == 0);
                    count = departmentList.Count();
                    List<Hashtable> tableList = new List<Hashtable>();
                    Hashtable noneTable = new Hashtable();
                    noneTable.Add("id", 0);
                    noneTable.Add("text", "无部门");
                    tableList.Add(noneTable);
                    if (count < 1)
                    {
                        return tableList;
                    }
                    foreach (var item in departmentList)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("id", item.ID);
                        ht.Add("text", item.Department);
                        InitDepartmentChildren(appEntitys, ht, item.ID);
                        tableList.Add(ht);
                    }
                    return tableList;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return new List<Hashtable>();
            }
        }
        /// <summary>
        /// 递归构造部门关联
        /// </summary>
        /// <param name="appEntitys"></param>
        /// <param name="ht"></param>
        /// <param name="parentId"></param>
        private void InitDepartmentChildren(HISDataEntities appEntitys, Hashtable ht, int parentId)
        {
            IQueryable<SYS_Department> departmentList = appEntitys.SYS_Department.Where(o => o.Flag == 1 && o.ParentID == parentId);
            int childrenCount = departmentList.Count();
            if (childrenCount > 0)
            {
                List<Hashtable> cList = new List<Hashtable>();
                foreach (var item in departmentList)
                {
                    Hashtable cht = new Hashtable();
                    cht.Add("id", item.ID);
                    cht.Add("text", item.Department);
                    InitDepartmentChildren(appEntitys, cht, item.ID);
                    cList.Add(cht);
                }
                ht.Add("children", cList);
            }
        }
        /// <summary>
        /// 获取TreeGrid 格式数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<Hashtable> GetDepartmentTreeGridList(ref int count, ref string errMsg)
        {
            try
            {
                List<SYS_Department> queryList = null;
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    queryList = appEntities.SYS_Department.Where(o => o.Flag == 1).ToList();
                    count = queryList.Count();
                    if (count < 1)
                    {
                        return null;
                    }
                }

                List<Hashtable> htList = new List<Hashtable>();
                var parentList = queryList.Where(o => o.ParentID == 0);
                foreach (SYS_Department actionItem in parentList)
                {
                    Hashtable ht = InitDepartmentGridChildren(queryList, actionItem);
                    htList.Add(ht);
                }
                return htList;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                count = 0;
                return null;
            }
        }
        private Hashtable InitDepartmentGridChildren(List<SYS_Department> departmentList, SYS_Department item)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", item.ID);
            ht.Add("Department", item.Department);
            ht.Add("DepartmentNum", item.DepartmentNum);
            //ht.Add("Disc", item.Disc);
            ht.Add("Flag", item.Flag);
            ht.Add("CreateTime", Convert.ToDateTime(item.CreateTime).ToString("yyyy-MM-dd HH:mm:ss"));
            ht.Add("ParentID", item.ParentID);

            var childrenList = departmentList.Where(o => o.ParentID == item.ID);
            if (childrenList.Count() > 0)
            {
                List<Hashtable> htList = new List<Hashtable>();
                foreach (SYS_Department childrenItem in childrenList)
                {
                    Hashtable childrenHt = InitDepartmentGridChildren(departmentList, childrenItem);
                    htList.Add(childrenHt);
                }
                ht.Add("children", htList);
            }
            return ht;

        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="department"></param>
        /// <param name="errMsg"></param>
        public void AddDepartment(SYS_Department department, ref string errMsg)
        {
            try
            {
                department.Flag = 1;
                using (HISDataEntities appEntitys = new HISDataEntities())
                {
                    if (department.ParentID == null) department.ParentID = 0;
                     //检查部门编号和部门名称不能重复
                     var queryList = appEntitys.SYS_Department.Where(o => o.Department == department.Department || o.DepartmentNum == department.DepartmentNum);
                    if (queryList.Count() > 0)
                    {
                        errMsg = "部门名称或部门编号重复";
                        return;
                    }
                    department.DepartmentNum = CreateDeptNum(department);
                    appEntitys.SYS_Department.Add(department);
                    appEntitys.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

        }

        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <param name="department"></param>
        /// <param name="errMsg"></param>
        public void EditDepartment(SYS_Department department, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntitys = new HISDataEntities())
                {
                    SYS_Department editItem = appEntitys.SYS_Department.Where(o => o.ID == department.ID).FirstOrDefault();
                    if (editItem == null)
                    {
                        errMsg = "查无数据";
                        return;
                    }

                    editItem.DepartmentNum = department.DepartmentNum;
                    editItem.Department = department.Department;
                    //editItem.Disc = department.Disc;
                    editItem.ParentID = department.ParentID;
                    editItem.Flag = department.Flag;

                    appEntitys.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="errMsg"></param>
        public void DeleteDepartment(int departmentID, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntitys = new HISDataEntities())
                {
                    SYS_Department editItem = appEntitys.SYS_Department.Where(o => o.ID == departmentID).FirstOrDefault();
                    if (editItem == null)
                    {
                        errMsg = "查无数据";
                        return;
                    }
                    //标记删除
                    editItem.Flag = 0;
                    appEntitys.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }



    }
}
