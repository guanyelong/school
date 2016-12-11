﻿using SP.Business.HIS;
using SP.Models.HIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SP.HIS.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(SP.Models.HIS.LoginInfo info)
        {
            if (ModelState.IsValid && info != null)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
            }
            return View("Main");
        }

        public ActionResult Main()
        {
            RoleInfoBLL userRoleBll = new RoleInfoBLL();

            if (AdminSystemInfo.CurrentUser != null)
            {
                var queryList = userRoleBll.GetAppUserMenuList(AdminSystemInfo.CurrentUser.ID);
                if (queryList == null)
                {
                    return View();
                }
                if (queryList != null)
                {
                    ViewBag.MainMenu = queryList.Where(o => o.ParentID == 0).OrderBy(o => o.ORDERINDEX).ToList();
                }

                return View(AdminSystemInfo.CurrentUser);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Left()
        {
            return View();
        }
        public ActionResult Center()
        {
            return View();
        }

        public ActionResult Portal() {
            return View();
        }

        public ActionResult Registration() {
            return View();
        }

        public ActionResult Price() {
            return View();
        }
        public ActionResult Patient() {
            return View();
        }
    }
}