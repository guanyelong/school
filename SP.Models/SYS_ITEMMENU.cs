//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SYS_ITEMMENU
    {
        public int ID { get; set; }
        public string MenuNum { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string TEXT { get; set; }
        public string URL { get; set; }
        public string ICON { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string STATE { get; set; }
        public Nullable<bool> CHECKED { get; set; }
        public Nullable<int> SEQ { get; set; }
        public string ActionNum { get; set; }
        public Nullable<int> ORDERINDEX { get; set; }
    }
}
