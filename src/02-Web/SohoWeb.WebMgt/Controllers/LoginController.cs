using Soho.Utility.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Soho.Utility.Web.Framework;

namespace SohoWeb.WebMgt.Controllers
{
    public class LoginController : WWWController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ReturnUrl = Request.Params["ReturnUrl"];
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            bool loginResult = (new AuthMgr()).Logout();
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = loginResult,
                Message = ""
            };
            return RedirectToRoute("Home");
        }
    }
}
