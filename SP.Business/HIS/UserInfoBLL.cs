using SP.Models;
using SP.Models.HIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class UserInfoBLL
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SYS_USERINFO GetAllAppUserById(int id)
        {
            using (HISDataEntities AppStoreEntity = new HISDataEntities())
            {
                SYS_USERINFO model = null;
                var query = (from a in AppStoreEntity.SYS_USERINFO
                             join d in AppStoreEntity.SYS_Department on a.DepartmentID equals d.ID into temp
                             from d in temp.DefaultIfEmpty()
                             where a.ID == id
                             select new { a, d }).FirstOrDefault();
                if (query == null)
                {
                    return new SYS_USERINFO();
                }

                model = new SYS_USERINFO();
                model.DepartmentID = query.a.DepartmentID;
                model.CreateTime = query.a.CreateTime;
                model.Email = query.a.Email;
                model.ID = query.a.ID;
                model.LoginName = query.a.LoginName;
                model.PASSWORD = query.a.PASSWORD;
                model.NAME = query.a.NAME;
                model.QQ = query.a.QQ;
                model.Tel = query.a.Tel;
                model.Position = query.a.Position;
                model.HeadImg = query.a.HeadImg;
                if (query.d != null)
                {
                    model.DepartmentName = query.d.Department;
                    model.DepartmentNum = query.d.DepartmentNum;
                }
                else
                {
                    model.DepartmentName = "无部门";
                }

                return model;
            }
        }
        /// <summary>
        /// 分页获取AppUserList
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SYS_USERINFO> GetAppUserList(int pageIndex, int pageSize, string name, ref int count)
        {

            try
            {
                //查询数据
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var queryResult = (from a in appEntities.SYS_USERINFO
                                       join b in appEntities.SYS_Department on a.DepartmentID equals b.ID into temp
                                       from b in temp.DefaultIfEmpty()
                                       select new { a, b });
                    if (!String.IsNullOrEmpty(name))
                    {
                        queryResult = queryResult.Where(p => p.a.NAME.Contains(name) || p.a.LoginName.Contains(name));
                    }
                    count = queryResult.Count();

                    if (count < 1)
                    {
                        return new List<SYS_USERINFO>();
                    }
                    //分页
                    queryResult = queryResult.OrderByDescending(p => p.a.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                    //转换
                    List<SYS_USERINFO> result = new List<SYS_USERINFO>();
                    foreach (var item in queryResult)
                    {

                        SYS_USERINFO user = new SYS_USERINFO();
                        user.ID = item.a.ID;
                        user.NAME = item.a.NAME;
                        user.Tel = item.a.Tel;
                        user.Email = item.a.Email;
                        user.CreateTime = item.a.CreateTime;
                        user.HeadImg = item.a.HeadImg;
                        user.LoginName = item.a.LoginName;
                        user.QQ = item.a.QQ;
                        user.Position = item.a.Position;
                        if (item.b == null)
                        {
                            user.DepartmentID = 0;
                            user.DepartmentName = "无部门";
                        }
                        else
                        {
                            user.DepartmentID = item.b.ID;
                            user.DepartmentName = item.b.Department;
                        }

                        result.Add(user);
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(SYS_USERINFO user, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntity = new HISDataEntities())
                {
                    //检查重复用户名
                    IQueryable<SYS_USERINFO> appList = appEntity.SYS_USERINFO.Where(o => o.LoginName == user.LoginName);
                    if (appList.Count() > 0)
                    {

                        errMsg = "用户名已经存在";
                        return;
                    }
                    user.GUID = Guid.NewGuid();
                    user.PASSWORD = Common.MD5Helper.MD5Encrypt32bit(user.PASSWORD);
                    appEntity.SYS_USERINFO.Add(user);

                    //为用户设置默认角色
                    appEntity.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

        /// <summary>
        /// 编辑User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="errMsg"></param>
        public void EditUser(SYS_USERINFO user, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntity = new HISDataEntities())
                {
                    var item = appEntity.SYS_USERINFO.Where(o => o.ID == user.ID).FirstOrDefault();

                    if (item == null)
                    {
                        errMsg = "无此数据";
                        return;
                    }
                    item.NAME = user.NAME;
                    item.LoginName = user.LoginName;
                    item.Position = user.Position;
                    item.QQ = user.QQ;
                    item.Tel = user.Tel;
                    item.Email = user.Email;
                    item.DepartmentID = user.DepartmentID;
                    item.HeadImg = user.HeadImg;

                    appEntity.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }
        /// <summary>
        /// 删除用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="errMsg"></param>
        public void DeleteUser(int userId, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntitys = new HISDataEntities())
                {
                    var appUser = appEntitys.SYS_USERINFO.Where(o => o.ID == userId).FirstOrDefault();
                    if (appUser == null)
                    {
                        errMsg = "查找不到数据数据";
                        return;
                    }
                    //删除用户
                    appEntitys.SYS_USERINFO.Remove(appUser);
                    //删除用户级联的角色
                    var roleMapping = appEntitys.SYS_USERROLEMAPPING.Where(o => o.USERID == userId);
                    if (roleMapping.Count() > 0)
                    {
                        foreach (var item in roleMapping)
                        {
                            appEntitys.SYS_USERROLEMAPPING.Remove(item);
                        }
                    }

                    appEntitys.SaveChanges();

                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }
        /// <summary>
        /// 想修改用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="errMsg"></param>
        public void ChangePassword(int userId, string password, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var findUser = appEntities.SYS_USERINFO.Where(o => o.ID == userId).FirstOrDefault();
                    if (findUser == null)
                    {
                        errMsg = "查无用户";
                        return;
                    }
                    // 密码使用md5 加密
                    findUser.PASSWORD = Common.MD5Helper.MD5Encrypt32bit(password);
                    appEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

    }
}
