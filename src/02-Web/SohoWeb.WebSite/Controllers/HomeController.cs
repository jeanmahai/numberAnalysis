using System.Web.Mvc;
using Soho.Utility.Web.Framework;

using SohoWeb.Service.ControlPanel;

namespace SohoWeb.WebSite.Controllers
{
    /// <summary>
    /// 默认处理
    /// </summary>
    public class HomeController : WWWController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = 1001,
                Message = ""
            };
            return View(result);
        }
    }
}
