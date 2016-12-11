using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Models.HIS
{
    public  partial class RoleInfo
    {
        public int ID { get; set; }
        public string ROLENAME { get; set; }
        public byte STATE { get; set; }
        public string ROLENUM { get; set; }
        public DateTime CREATETIME { get; set; }
    }
}
