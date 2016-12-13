using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Models.HIS
{
    public partial class UserInfo
    {
        public int ID { get; set; }
        public string GUID { get; set; }
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [RegularExpression(@"^[\u4E00-\u9FA5|\W|\w]{1,32}$", ErrorMessage = "6-32位字符，支持中英文")]
        public string LoginName { get; set; }
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码必填")]
        [RegularExpression(@"[a-zA-Z0-9]{1,}", ErrorMessage = "密码由字母和数字组成")]
        public string PASSWORD { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入姓名")]
        [RegularExpression(@"^[\u4E00-\u9FA5|\W|\w]{1,32}$", ErrorMessage = "6-32位字符，支持中英文")]
        public string NAME { get; set; }
        [Display(Name = "年龄")]
        [Range(1, 120)]
        [Required(ErrorMessage = "请输入年龄")]
        public int AGE { get; set; }
        [Display(Name = "性别")]
        [Required(ErrorMessage = "请选择性别")]
        public byte SEX { get; set; }
        [Display(Name = "手机")]
        [Required(ErrorMessage = "请输入手机")]
        [RegularExpression(@"^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$", ErrorMessage = "手机格式不正确")]
        public string PHONE { get; set; }
    }

    public partial class LoginInfo{
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [RegularExpression(@"^[\u4E00-\u9FA5|\W|\w]{1,32}$", ErrorMessage = "6-32位字符，支持中英文")]
        public string LoginName { get; set; }
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }
    }

    public partial class UserItems
    {

       
    }
}
