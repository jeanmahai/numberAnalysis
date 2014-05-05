using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Analysis28.Service.Users;

namespace Analysis28.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var result = UserAuthService.GetDemoData(0);
            return View(result);
        }
    }
}
