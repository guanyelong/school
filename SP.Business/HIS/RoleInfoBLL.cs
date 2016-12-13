using SP.Models;
using SP.Models.HIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class RoleInfoBLL
    {
        /// <summary>
        /// 根据权限获取该用户的菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SYS_ITEMMENU> GetAppUserMenuList(int userId)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {

                    //级联角色，如果角色删除，排除删除的角色
                    //将菜单存到数据库
                    var quertList = from a in appEntities.SYS_ITEMMENU
                                    join b in appEntities.SYS_Action on a.ActionNum equals b.ActionNum
                                    join c in appEntities.SYS_ROLEACTIONMAPPING on b.ID equals c.ACTIONID
                                    join d in appEntities.SYS_USERROLEMAPPING on c.ROLEID equals d.ROLEID
                                    join e in appEntities.SYS_ROLE on d.ROLEID equals e.ID
                                    where d.USERID == userId && b.Flag == 1 && e.FLAG == 1
                                    orderby a.ORDERINDEX ascending
                                    select a;

                    return quertList.Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据ID获取系统角色
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public SYS_ROLE GetAppRoleByID(int Id, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {

                    SYS_ROLE role = appEntities.SYS_ROLE.Where(o => o.ID == Id).FirstOrDefault();
                    if (role == null)
                    {
                        errMsg = "查无数据";
                        return null;
                    }

                    return role;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SYS_ROLE> GetAppRoleList(int pageIndex, int pageSize, ref int count, string name)
        {
            try
            {
                using (HISDataEntities appEntites = new HISDataEntities())
                {
                    var roleList = appEntites.SYS_ROLE.Where(o => o.FLAG == 1);
                    if (!string.IsNullOrEmpty(name))
                    {
                        roleList = roleList.Where(o => o.ROLENAME.Contains(name));
                    }

                    count = roleList.Count();
                    if (count < 1)
                    {
                        return new List<SYS_ROLE>();
                    }
                    roleList = roleList.OrderByDescending(o => o.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    return roleList.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private string CreateRoleNum(SYS_ROLE role)
        {
            string actionCount = "01";
            string menuNum = "RE";
            SYS_ROLE lastMenu = null;
            using (HISDataEntities hisEntities = new HISDataEntities())
            {
                //查询菜单下最大编号不包括自己和已经删除的
                lastMenu = hisEntities.SYS_ROLE.OrderByDescending(o => o.ROLENUM).FirstOrDefault();
            }
            if (lastMenu != null)
            {
                var str = lastMenu.ROLENUM.Substring(lastMenu.ROLENUM.Length - 2, 2);
                int number = Convert.ToInt32(str) + 1;
                actionCount = number.ToString().PadLeft(2, '0');
            }
            return menuNum + actionCount;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <param name="errMsg"></param>
        public void AddRole(SYS_ROLE roleInfo, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    roleInfo.FLAG = 1;
                    roleInfo.CREATETIME = DateTime.Now;

                    //判断角色编码不能重复
                    var existRole = appEntities.SYS_ROLE.Where(o => (o.ROLENUM == roleInfo.ROLENUM || o.ROLENAME == roleInfo.ROLENAME) && o.FLAG != -1).FirstOrDefault();
                    if (existRole != null)
                    {
                        errMsg = "角色编码/名称不能重复";
                        return;
                    }
                    roleInfo.ROLENUM = CreateRoleNum(roleInfo);
                    appEntities.SYS_ROLE.Add(roleInfo);
                    appEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <param name="errMsg"></param>
        public void EditRole(SYS_ROLE roleInfo, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    SYS_ROLE roleItem = appEntities.SYS_ROLE.Where(o => o.ID == roleInfo.ID).FirstOrDefault();
                    if (roleItem == null)
                    {
                        errMsg = "查无数据";
                        return;
                    }

                    //检查用户编码不能重复
                    if (appEntities.SYS_ROLE.Where(o => o.ROLENUM == roleInfo.ROLENUM && o.ID != roleInfo.ID).Count() > 0)
                    {
                        errMsg = "角色编码不能重复";
                        return;
                    }

                    roleItem.ROLENAME = roleInfo.ROLENAME;
                    roleItem.ROLENUM = roleInfo.ROLENUM;
                    //roleItem.Disc = roleInfo.Disc;

                    appEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="errMsg"></param>
        public void DeleteRole(int roleId, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var roleItem = appEntities.SYS_ROLE.Where(o => o.ID == roleId).FirstOrDefault();
                    if (roleItem == null)
                    {
                        errMsg = "查无数据";
                        return;
                    }
                    //标记删除角色
                    roleItem.FLAG = -1;
                    //删除角色关联的权限
                    var roleMapping = appEntities.SYS_ROLEACTIONMAPPING.Where(o => o.ROLEID == roleId);
                    if (roleMapping.Count() > 0)
                    {
                        foreach (var item in roleMapping)
                        {
                            appEntities.SYS_ROLEACTIONMAPPING.Remove(item);
                        }
                    }
                    //删除角色关联的用户

                    var userMapping = appEntities.SYS_USERROLEMAPPING.Where(o => o.ROLEID == roleId);
                    if (userMapping.Count() > 0)
                    {
                        foreach (var item in userMapping)
                        {
                            appEntities.SYS_USERROLEMAPPING.Remove(item);
                        }
                    }

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
