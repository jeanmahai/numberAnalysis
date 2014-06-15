using System.Web;
using System.Web.Mvc;

using Soho.Utility;
using Soho.Utility.Web;
using Soho.Utility.Web.Framework;
using SohoWeb.WebMgt.ViewModels;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.Service.ControlPanel;

namespace SohoWeb.WebMgt.Controllers
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SystemUserController : WWWController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            bool loginResult = (new AuthMgr()).Login(Request.Params["LoginID"], Request.Params["LoginPassword"], Request.Params["ValidateCode"]);
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = loginResult,
                Message = ""
            };

            if (!string.IsNullOrWhiteSpace(Request.Params["ReturnUrl"]))
            {
                return Redirect(Request.Params["ReturnUrl"]);
            }
            else
            {
                return Redirect("/Master");
            }
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
            return View(result);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            var requestVM = GetParams<Users>();
            this.SetEntityBase(requestVM, true);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = UsersMgtService.Instance.InsertUser(requestVM),
                Message = ""
            };
            return View(result);
        }
    }
}
