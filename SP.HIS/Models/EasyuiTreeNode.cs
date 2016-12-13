using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP.HIS.Models
{
    public class EasyuiTreeNode
    {
        public string id { get; set; }
        public string text { get; set; }
        public string iconCls { get; set; }
        public bool @checked { get; set; }
        public Dictionary<string, string> attributes { get; set; }
        public string state { get; set; }
        public List<EasyuiTreeNode> children = new List<EasyuiTreeNode>();
    }
}