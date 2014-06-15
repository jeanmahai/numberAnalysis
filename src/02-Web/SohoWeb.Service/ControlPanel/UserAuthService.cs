using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.DataAccess.ControlPanel;
using Soho.Utility;
using Soho.Utility.Encryption;

namespace SohoWeb.Service.ControlPanel
{
    public class UserAuthService : BaseService<UserAuthService>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userPwd">用户密码，明文</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>true-登录成功；false-登录失败</returns>
        public bool Login(string userID, string userPwd, string validateCode)
        {
            var user = UserAuthDA.GetUserByUserID(userID);
            
            if (user == null)
                throw new BusinessException("用户名不存在！");
            var currPassword = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", userPwd, user.UserAuthCode)).ToLower();
            if (!currPassword.Equals(user.Password.ToLower()))
                throw new BusinessException("密码错误！");

            #region 写登录Log
            Logs log = new Logs()
            {
                Classes = 1001,
                Section = 1002,
                Family = 1005,
                RefenceSysNo = user.SysNo.Value,
                Contents = "登录成功。",
                InUserSysNo = 0,
                InUserName = "System"
            };
            LogsMgtService.Instance.InsertLogs(log);
            #endregion

            return true;
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public Users GetUserByUserID(string userID)
        {
            return UserAuthDA.GetUserByUserID(userID);
        }

        /// <summary>
        /// 根据用户编号获取用户权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public string[] GetFunctionsByUserSysNo(int userSysNo)
        {
            return UserAuthDA.GetFunctionsByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 获取所有有效权限
        /// </summary>
        /// <returns></returns>
        public string[] GetAllValidFunctions()
        {
            return UserAuthDA.GetAllValidFunctions();
        }
    }
}
