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
    }
}
