using System;
using System.Linq;
using System.Configuration;

using Soho.Utility.Web;
using Soho.Utility.Web.Framework;
using SohoWeb.WebSite.ViewModels;
using SohoWeb.Service.Customer;

namespace SohoWeb.WebSite
{
    public class AuthMgr : IAuth
    {
        #region IAuth 成员

        public bool ValidateAuth(string controller, string action)
        {
            return true;// IsAllowed(controller, action);
        }

        public bool ValidateLogin()
        {
            LoginAuthVM user = ReadUserInfo();
            if (user == null || user.UserSysNo <= 0)
                return false;

            if (user != null && DateTime.Now >= user.Timeout)
            {
                return false;
            }
            else if (user != null && user.RememberLogin == true)
            {
                user.LoginDate = DateTime.Now;
                int mobileLoginTimeout = 30000000;
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MobileLoginTimeout"]))
                {
                    int.TryParse(ConfigurationManager.AppSettings["MobileLoginTimeout"], out mobileLoginTimeout);
                    mobileLoginTimeout = mobileLoginTimeout <= 0 ? 30000000 : mobileLoginTimeout;
                }
                user.Timeout = DateTime.Now.AddMinutes(mobileLoginTimeout);
            }
            WriteUserInfo(user);

            return true;
        }

        #endregion

        #region 登录、登出
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="customerID">用户ID</param>
        /// <param name="customerPwd">用户密码，明文</param>
        /// <param name="validateCode">验证码</param>
        /// <param name="IP">IP地址</param>
        /// <returns>true-登录成功；false-登录失败</returns>
        public bool Login(string customerID, string customerPwd, string validateCode, string IP)
        {
            bool result = CustomerAuthService.Instance.Login(customerID, customerPwd, validateCode, IP);
            if (result)
            {
                var customer = CustomerAuthService.Instance.GetCustomerByCustomerID(customerID);
                LoginAuthVM authUser = new LoginAuthVM()
                {
                    UserSysNo = customer.SysNo.Value,
                    UserID = customer.CustomerID,
                    UserName = customer.CustomerName,
                    LoginDate = DateTime.Now,
                    Timeout = DateTime.Now.AddMinutes(30000000),
                    RememberLogin = true
                };
                WriteUserInfo(authUser);
            }
            return result;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            LoginAuthVM authUser = new LoginAuthVM();
            WriteUserInfo(authUser);
            return true;
        }
        #endregion

        #region 读写用户信息
        /// <summary>
        /// 写用户信息
        /// </summary>
        /// <param name="authUser">用户信息</param>
        public void WriteUserInfo(LoginAuthVM authUser)
        {
            CookieHelper.SaveCookie<LoginAuthVM>("LoginCookie", authUser);
            CookieHelper.SaveCookie<string>("CustomerNameCookie", authUser.UserName);
        }

        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <returns></returns>
        public LoginAuthVM ReadUserInfo()
        {
            return CookieHelper.GetCookie<LoginAuthVM>("LoginCookie");
        }
        #endregion
    }
}