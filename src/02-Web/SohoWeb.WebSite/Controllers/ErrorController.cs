using System.Web.Mvc;

namespace SohoWeb.WebSite.Controllers
{
    /// <summary>
    /// 错误处理
    /// </summary>
    public class ErrorController : WWWController
    {
        /// <summary>
        /// 一般错误
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 404错误
        /// </summary>
        /// <returns></returns>
        public ActionResult Error404()
        {
            return View();
        }

        /// <summary>
        /// 授权错误
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthError()
        {
            return View();
        }
    }
}
