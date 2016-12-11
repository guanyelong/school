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
}
