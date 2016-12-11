using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Models.HIS
{
    public  partial class MenuItem
    {
        public int ID { get; set; }
        public string MenuNum { get; set; }
        public int ParentID { get; set; }
        public string TEXT { get; set; }
        public string URL { get; set; }
    }
}
