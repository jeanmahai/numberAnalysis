using System;
using System.Linq;
using System.Configuration;

using Soho.Utility.Web;
using Soho.Utility.Web.Framework;
using SohoWeb.WebSite.ViewModels;

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
                int mobileLoginTimeout = 30;
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MobileLoginTimeout"]))
                {
                    int.TryParse(ConfigurationManager.AppSettings["MobileLoginTimeout"], out mobileLoginTimeout);
                    mobileLoginTimeout = mobileLoginTimeout <= 0 ? 30 : mobileLoginTimeout;
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
        /// <param name="userID">用户ID</param>
        /// <param name="userPwd">用户密码，明文</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>true-登录成功；false-登录失败</returns>
        public bool Login(string userID, string userPwd, string validateCode)
        {
            bool result = true;// UserAuthService.Instance.Login(userID, userPwd, validateCode);
            if (result)
            {
                //var user = UserAuthService.Instance.GetUserByUserID(userID);
                LoginAuthVM authUser = new LoginAuthVM()
                {
                    //UserSysNo = user.SysNo.Value,
                    //UserID = user.UserID,
                    //UserName = user.UserName,
                    //LoginDate = DateTime.Now,
                    //Timeout = DateTime.Now.AddMinutes(30),
                    //RememberLogin = true
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