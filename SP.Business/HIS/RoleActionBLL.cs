using SP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Business.HIS
{
    public class RoleActionBLL
    {
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="name">查询字段</param>
        /// <param name="roleid">角色名称</param>
        /// <param name="errMsg">发生错误返回结果</param>
        /// <param name="count">查询数据总记录数</param>
        /// <returns></returns>
        public List<Hashtable> GetAppRoleActionTreeGrid(int roleid, ref string errMsg, ref int count)
        {
            try
            {
                List<SYS_Action> queryList = null;
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    //查询跟该角色相关的所有权限数据
                    var query = (from a in appEntities.SYS_ROLEACTIONMAPPING
                                 join b in appEntities.SYS_Action on a.ACTIONID equals b.ID
                                 where a.ROLEID == roleid && b.Flag == 1
                                 select new { b });

                    queryList = new List<SYS_Action>();
                    foreach (var item in query)
                    {
                        SYS_Action action = item.b;
                        queryList.Add(action);
                    }
                }

                List<Hashtable> list = new List<Hashtable>();
                if (queryList.Count < 1)
                {
                    return list;
                }

                //生成所有数据的集合
                var actionList = queryList.Where(o => o.ParentID == 0);
                foreach (SYS_Action actionItem in actionList)
                {
                    Hashtable ht = CreateAppRoleActionTreeGrid(queryList, actionItem);
                    list.Add(ht);
                }

                count = queryList.Count;
                return list;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return new List<Hashtable>();
            }
        }
        /// <summary>
        /// 生成查询到的数据权限
        /// </summary>
        /// <param name="queryList"></param>
        /// <param name="list"></param>
        private Hashtable CreateAppRoleActionTreeGrid(List<SYS_Action> queryList, SYS_Action actionItem)
        {
            Hashtable ht = CreateHashtable(actionItem);
            ht.Remove("children");

            var childrenList = queryList.Where(o => o.ParentID == actionItem.ID);
            if (childrenList.Count() > 0)
            {
                List<Hashtable> htList = new List<Hashtable>();
                foreach (SYS_Action item in childrenList)
                {
                    Hashtable childrenHt = CreateAppRoleActionTreeGrid(queryList, item);
                    htList.Add(childrenHt);
                }
                ht.Add("children", htList);
            }
            return ht;
        }
        public Hashtable CreateHashtable(SYS_Action actionItem)
        {
            Hashtable htItem = new Hashtable();
            htItem.Add("ID", actionItem.ID);
            htItem.Add("ParentID", actionItem.ParentID);
            htItem.Add("ActionName", actionItem.ActionName);
            htItem.Add("ActionMenu", actionItem.ActionMenu);
            htItem.Add("ActionNum", actionItem.ActionNum);
            htItem.Add("Type", actionItem.Type);
            htItem.Add("TypeName", actionItem.Type == 1 ? "菜单权限" : "按钮权限");
            //htItem.Add("Disc", actionItem.Disc);
            htItem.Add("CreateTime", actionItem.CREATETIME);
            htItem.Add("children", new List<Hashtable>());
            return htItem;
        }

        /// <summary>
        /// 获取权限的Tree数据源，如果Role不为空，则根据roleID自动选中
        /// </summary>
        /// <param name="p"></param>
        /// <param name="errMsg"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Hashtable> GetRoleActionRoleTreeList(int roleId, ref string errMsg)
        {
            try
            {
                List<SYS_Action> queryList = null;
                List<SYS_ROLEACTIONMAPPING> roleMapping = null;

                //获取权限列表和当前的权限列表
                using (HISDataEntities appEntities = new HISDataEntities())
                {
                    queryList = (from a in appEntities.SYS_Action where a.Flag == 1 select a).ToList();
                    roleMapping = (from a in appEntities.SYS_ROLEACTIONMAPPING where a.ROLEID == roleId select a).ToList();
                    if (queryList.Count() < 1)
                    {
                        return new List<Hashtable>();
                    }
                }

                List<Hashtable> htList = new List<Hashtable>();
                var findList = queryList.Where(o => o.ParentID == 0 && o.Flag == 1);

                foreach (SYS_Action parentItem in findList)
                {
                    Hashtable ht = InitActionTreeChildren(queryList, parentItem, roleMapping);
                    htList.Add(ht);
                }

                return htList;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return new List<Hashtable>();
            }
        }
        private Hashtable InitActionTreeChildren(List<SYS_Action> queryList, SYS_Action parentItem, List<SYS_ROLEACTIONMAPPING> roleMapping)
        {

            Hashtable ht = CreateTreeHashTable(parentItem);
            var childrenList = queryList.Where(o => o.ParentID == parentItem.ID);
            if (childrenList.Count() > 0)
            {
                List<Hashtable> htList = new List<Hashtable>();
                foreach (SYS_Action item in childrenList)
                {
                    Hashtable childrenHt = InitActionTreeChildren(queryList, item, roleMapping);
                    htList.Add(childrenHt);
                }
                ht.Add("children", htList);
            }
            else
            {
                //如果没有子权限证明是底层权限，底层权限判定是否选中
                if (roleMapping.Where(o => o.ACTIONID == parentItem.ID).FirstOrDefault() != null)
                {
                    ht.Add("checked", true);
                }
                else
                {
                    ht.Add("checked", false);
                }
            }

            return ht;
        }
        private Hashtable CreateTreeHashTable(SYS_Action actionItem)
        {
            Hashtable htItem = new Hashtable();
            htItem.Add("id", actionItem.ID);
            htItem.Add("text", actionItem.ActionName);
            return htItem;
        }


    }
}
