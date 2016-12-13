using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Models
{
    public partial class SYS_USERINFO
    {
        private List<SYS_ROLE> _RoleUserList;
        /// <summary>
        /// 角色集合
        /// </summary>
        public List<SYS_ROLE> RoleUserList
        {
            get
            {
                if (_RoleUserList == null)
                {
                    return _RoleUserList = new List<SYS_ROLE>();
                }
                return _RoleUserList;
            }
            set { _RoleUserList = value; }
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 客服部门编号
        /// </summary>
        public string DepartmentNum { get; set; }
    }
}
