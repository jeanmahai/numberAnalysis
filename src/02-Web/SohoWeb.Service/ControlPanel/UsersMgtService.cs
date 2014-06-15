using Soho.Utility;
using Soho.Utility.Encryption;

using SohoWeb.Entity;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.DataAccess.ControlPanel;
using System.Collections.Generic;

namespace SohoWeb.Service.ControlPanel
{
    public class UsersMgtService : BaseService<UsersMgtService>
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity">用户信息</param>
        /// <returns></returns>
        public int InsertUser(Users entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.UserID))
                throw new BusinessException("必须输入用户ID！");
            if (string.IsNullOrWhiteSpace(entity.UserName))
                throw new BusinessException("必须输入用户名！");
            if (string.IsNullOrWhiteSpace(entity.Password))
                throw new BusinessException("必须输入密码！");

            var userList = UsersMgtDA.GetValidUserListByUserID(entity.UserID);
            if (userList != null && userList.Count > 0)
            {
                throw new BusinessException("用户ID已经存在！");
            }
            entity.UserAuthCode = GuidCode.GetGuid("D");
            entity.Password = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", entity.Password, entity.UserAuthCode)).ToLower();
            return UsersMgtDA.InsertUser(entity);
        }

        /// <summary>
        /// 根据用户编号更新用户信息
        /// </summary>
        /// <param name="entity">用户信息</param>
        public void UpdateUserBySysNo(Users entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.UserID))
                throw new BusinessException("必须输入用户ID！");
            if (string.IsNullOrWhiteSpace(entity.UserName))
                throw new BusinessException("必须输入用户名！");

            var userList = UsersMgtDA.GetValidUserListByUserID(entity.UserID);
            if (userList != null && userList.Count > 0)
            {
                if (userList.Count == 1 && userList[0].SysNo.Value != entity.SysNo.Value)
                    throw new BusinessException("用户ID已经存在！");
                if (userList.Count > 1)
                    throw new BusinessException("用户ID已经存在！");
            }
            UsersMgtDA.UpdateUserBySysNo(entity);
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="entity">用户信息</param>
        public void UpdateUserStatusBySysNo(Users entity)
        {
            UsersMgtDA.UpdateUserStatusBySysNo(entity);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="entity">用户信息</param>
        /// <param name="oldPassword">旧密码</param>
        public void UpdateUserPasswordBySysNo(Users entity, string oldPassword)
        {
            //check
            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new BusinessException("必须输入旧密码！");
            if (string.IsNullOrWhiteSpace(entity.Password))
                throw new BusinessException("必须输入新密码！");

            var user = UsersMgtDA.GetValidUserByUserID(entity.UserID);
            string currOldPassword = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", oldPassword, user.UserAuthCode)).ToLower();
            if (!currOldPassword.Equals(user.Password))
                throw new BusinessException("旧密码错误！");

            entity.Password = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", entity.Password, user.UserAuthCode)).ToLower();
            UsersMgtDA.UpdateUserPasswordByUserID(entity);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public QueryResult<Users> QueryUsers(UsersQueryFilter filter)
        {
            return UsersMgtDA.QueryUsers(filter);
        }

        /// <summary>
        /// 根据用户ID获取有效用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public Users GetValidUserByUserID(string userID)
        {
            return UsersMgtDA.GetValidUserByUserID(userID);
        }

        /// <summary>
        /// 根据用户编号获取有效用户
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public Users GetValidUserByUserSysNo(int userSysNo)
        {
            return UsersMgtDA.GetValidUserByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 获取用户存在的权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public List<Functions> GetUserExistsFunctions(int userSysNo)
        {
            return UsersMgtDA.GetUserExistsFunctions(userSysNo);
        }

        /// <summary>
        /// 获取用户不存在的权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public List<Functions> GetUserNotExistsFunctions(int userSysNo)
        {
            return UsersMgtDA.GetUserNotExistsFunctions(userSysNo);
        }

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="list"></param>
        public void SaveUserFunctions(List<UserFunctions> list)
        {
            if (list != null && list.Count > 0)
            {
                UsersMgtDA.DeleteUserFunctionsByUserSysNo(list[0].UserSysNo);
                foreach (var item in list)
                {
                    UsersMgtDA.InsertUserFunctions(item);
                }
            }
        }
    }
}
