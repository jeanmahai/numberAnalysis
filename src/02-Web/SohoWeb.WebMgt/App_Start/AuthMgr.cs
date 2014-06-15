using System;
using System.Linq;
using System.Configuration;

using Soho.Utility.Web;
using Soho.Utility.Web.Framework;
using SohoWeb.Service.ControlPanel;
using SohoWeb.WebMgt.ViewModels;

namespace SohoWeb.WebMgt
{
    public class AuthMgr : IAuth
    {
        #region IAuth 成员

        public bool ValidateAuth(string controller, string action)
        {
            return IsAllowed(controller, action);
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
            bool result = UserAuthService.Instance.Login(userID, userPwd, validateCode);
            if (result)
            {
                var user = UserAuthService.Instance.GetUserByUserID(userID);
                LoginAuthVM authUser = new LoginAuthVM()
                {
                    UserSysNo = user.SysNo.Value,
                    UserID = user.UserID,
                    UserName = user.UserName,
                    LoginDate = DateTime.Now,
                    Timeout = DateTime.Now.AddMinutes(30),
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
            CookieHelper.SaveCookie<string>("UserNameCookie", authUser.UserName);
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

        #region 权限
        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsAllowed(string controller, string action)
        {
            string authKey = string.Format("{0}|{1}", controller, action);
            return IsAllowed(authKey);
        }

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="authKey"></param>
        /// <returns></returns>
        public bool IsAllowed(string authKey)
        {
            string[] allAuthFunctions = GetAllAuthFunctions();

            //不存在有效的权限，则允许访问
            if (allAuthFunctions == null || allAuthFunctions.Length == 0)
                return true;

            string[] currUserAuthInfo = GetCurrUserAuthInfo();
            //存在该权限，但是当前用户无有效权限，则不允许访问
            if ((allAuthFunctions.Contains(authKey, StringComparer.CurrentCultureIgnoreCase))
                && (currUserAuthInfo == null || currUserAuthInfo.Length == 0))
                return false;

            //存在方法权限，但是当前用户不存在该权限，则不允许访问
            if (allAuthFunctions.Contains(authKey, StringComparer.CurrentCultureIgnoreCase)
                && !currUserAuthInfo.Contains(authKey, StringComparer.CurrentCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// 刷新用户权限缓存
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        public void RefreshUserFunctions(int userSysNo)
        {
            GetUserAuthInfo(userSysNo, true);
        }

        /// <summary>
        /// 获取当前用户有效权限
        /// </summary>
        /// <returns></returns>
        private string[] GetCurrUserAuthInfo()
        {
            var user = ReadUserInfo();
            return GetUserAuthInfo(user.UserSysNo, false);
        }

        /// <summary>
        /// 获取所有有效权限
        /// </summary>
        /// <returns></returns>
        private string[] GetAllAuthFunctions()
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            string cacheKey = "SOHO.WEB.ALLAUTHFUNCTIONS";
            string[] allAuthFunctions = cache[cacheKey] as string[];
            if (allAuthFunctions == null || allAuthFunctions.Length == 0)
            {
                allAuthFunctions = UserAuthService.Instance.GetAllValidFunctions();
                if (allAuthFunctions != null && allAuthFunctions.Length > 0)
                    cache.Set(cacheKey, allAuthFunctions, DateTimeOffset.Now.AddDays(1D));
            }
            return allAuthFunctions;
        }

        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        private string[] GetUserAuthInfo(int userSysNo, bool isFromDB)
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            string cacheKey = string.Format("SOHO.WEB.USER.AUTHINFO.{0}", userSysNo);
            string[] authInfo = cache[cacheKey] as string[];
            if (isFromDB || authInfo == null || authInfo.Length == 0)
            {
                authInfo = UserAuthService.Instance.GetFunctionsByUserSysNo(userSysNo);
                if (authInfo != null && authInfo.Length > 0)
                    cache.Set(cacheKey, authInfo, DateTimeOffset.Now.AddDays(1D));
            }
            return authInfo;
        }
        #endregion
    }
}