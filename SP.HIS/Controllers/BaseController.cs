using SP.Models.HIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SP.HIS.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            if (AdminSystemInfo.CurrentUser == null)
            {
                //FormsAuthentication.RedirectToLoginPage();
            }
        }
    }
}