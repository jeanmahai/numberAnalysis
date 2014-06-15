using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SohoWeb.WebMgt.Controllers
{
    public class MasterController : SSLController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
