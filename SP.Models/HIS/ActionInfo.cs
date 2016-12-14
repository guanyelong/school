using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Models.HIS
{
    public partial class ActionInfo
    {
        public int ID { get; set; }
        public string ActionName { get; set; }
        public string ActionNum { get; set; }
        public int ParentID { get; set; }
        public int ActionMenu { get; set; }
        public int Flag { get; set; }
        public int Type { get; set; }

    }
    public enum AppActionType
    {
        /// <summary>
        /// 按钮权限
        /// </summary>
        ButtonAction = 0,
        /// <summary>
        /// 菜单权限
        /// </summary>
        MenuAction = 1,
        /// <summary>
        /// 所有权限
        /// </summary>
        AllAction = 2
    }


}
