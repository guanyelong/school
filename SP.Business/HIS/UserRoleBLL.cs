using SP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class UserRoleBLL
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
        /// 根据用户Id获取所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SYS_ROLE> GetAppUserRoleList(int userId, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    var query = (from a in appEntities.SYS_ROLE
                                 join b in appEntities.SYS_USERROLEMAPPING on a.ID equals b.ROLEID
                                 where b.USERID == userId
                                 select a);

                    if (query == null)
                    {
                        return null;
                    }

                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
        }
        /// <summary>
        /// 获取分页用户角色
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="name">用于检索的记录数</param>
        /// <param name="userId">用户Id</param>
        /// <param name="count"></param>
        /// <returns>返回角色列表页</returns>
        public List<SYS_ROLE> GetAppUserRoleList(int pageIndex, int pageSize, string name, int userId, ref int count)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {


                    var query = (from a in appEntities.SYS_ROLE
                                 join b in appEntities.SYS_USERROLEMAPPING on a.ID equals b.ROLEID
                                 join c in appEntities.SYS_USERINFO on b.USERID equals c.ID
                                 where b.USERID == userId && a.FLAG == 1
                                 select new { a });

                    if (query == null)
                    {
                        count = 0;
                        return null;
                    }
                    //筛选
                    query = query.Where(o => o.a.ROLENAME.Contains(name));
                    count = query.Count();
                    //分页
                    query = query.OrderByDescending(o => o.a.CREATETIME).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                    List<SYS_ROLE> roleList = new List<SYS_ROLE>();
                    foreach (var item in query)
                    {
                        SYS_ROLE model = new SYS_ROLE();
                        model.ID = item.a.ID;
                        model.ROLENAME = item.a.ROLENAME;
                        model.ROLENUM = item.a.ROLENUM;
                        model.CREATETIME = item.a.CREATETIME;
                        model.FLAG = item.a.FLAG;
                        //model.Disc = item.a.Disc;
                        roleList.Add(model);
                    }

                    return roleList;
                }
            }
            catch (Exception e)
            {
                count = 0;
                return null;
            }
        }
        /// <summary>
        /// 删除user的role
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="intRoleIds"></param>
        /// <param name="errMsg"></param>
        public void DeleteUserRole(int userid, int roleid, ref string errMsg)
        {
            try
            {
                using (HISDataEntities appEntitys = new HISDataEntities())
                {

                    var item = appEntitys.SYS_USERROLEMAPPING.Where(o => o.USERID == userid && o.ROLEID == roleid).FirstOrDefault();
                    if (item == null)
                    {
                        errMsg = "用户不存在该角色";
                        return;
                    }
                    appEntitys.SYS_USERROLEMAPPING.Remove(item);

                    appEntitys.SaveChanges();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
        }
        /// <summary>
        /// 根据用户id，获取用户下的权限列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="actionType">权限类型</param>
        /// <returns></returns>
        public List<SYS_Action> GetAppUserActionList(int userid, Models.HIS.AppActionType actionType)
        {
            try
            {
                using (HISDataEntities appEntities = new HISDataEntities())
                {

                    //查询
                    var queryList = (from a in appEntities.SYS_Action
                                     join b in appEntities.SYS_ROLEACTIONMAPPING on a.ID equals b.ACTIONID
                                     join c in appEntities.SYS_USERROLEMAPPING on b.ROLEID equals c.ROLEID
                                     where c.USERID == userid
                                     select a).Distinct();

                    if (queryList.Count() < 1)
                    {
                        return null;
                    }

                    //过滤菜单权限还是按钮权限
                    if (actionType == Models.HIS.AppActionType.AllAction)
                    {
                        return queryList.ToList();
                    }
                    int typeInt = Convert.ToInt32(actionType);
                    queryList = queryList.Where(o => o.Type == typeInt);
                    return queryList.ToList();
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
