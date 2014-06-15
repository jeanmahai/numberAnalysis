using System;
using System.Data;
using System.Collections.Generic;

using SohoWeb.Entity;
using SohoWeb.Entity.ControlPanel;
using Soho.Utility.DataAccess;
using SohoWeb.Entity.Enums;

namespace SohoWeb.DataAccess.ControlPanel
{
    public class UsersMgtDA
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity">用户信息</param>
        /// <returns></returns>
        public static int InsertUser(Users entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertUser");
            cmd.SetParameterValue<Users>(entity);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.GetParameterValue("@SysNo"));
        }

        /// <summary>
        /// 根据用户编号更新用户信息
        /// </summary>
        /// <param name="entity">用户信息</param>
        public static void UpdateUserBySysNo(Users entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateUserBySysNo");
            cmd.SetParameterValue<Users>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="entity">用户信息</param>
        public static void UpdateUserStatusBySysNo(Users entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateUserStatusBySysNo");
            cmd.SetParameterValue<Users>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="entity">用户信息</param>
        public static void UpdateUserPasswordByUserID(Users entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateUserPasswordByUserID");
            cmd.SetParameterValue<Users>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static QueryResult<Users> QueryUsers(UsersQueryFilter filter)
        {
            QueryResult<Users> result = new QueryResult<Users>();
            result.ServicePageIndex = filter.ServicePageIndex;
            result.PageSize = filter.PageSize;

            PagingInfoEntity page = DataAccessUtil.ToPagingInfo(filter);
            CustomDataCommand cmd = DataCommandManager.CreateCustomDataCommandFromConfig("QueryUsers");
            using (var sqlBuilder = new DynamicQuerySqlBuilder(cmd.CommandText, cmd, page, "SysNo DESC"))
            {
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Status", DbType.Int32,
                    "@Status1", QueryConditionOperatorType.NotEqual, CommonStatus.Deleted);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "SysNo", DbType.Int32,
                    "@SysNo", QueryConditionOperatorType.Equal, filter.SysNo);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "UserID", DbType.String,
                    "@UserID", QueryConditionOperatorType.Like, filter.UserID);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "UserName", DbType.String,
                    "@UserName", QueryConditionOperatorType.Like, filter.UserName);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Status", DbType.Int32,
                    "@Status", QueryConditionOperatorType.Equal, filter.Status);

                cmd.CommandText = sqlBuilder.BuildQuerySql();
                result.ResultList = cmd.ExecuteEntityList<Users>();
                result.TotalCount = Convert.ToInt32(cmd.GetParameterValue("@TotalCount"));

                return result;
            }
        }

        /// <summary>
        /// 根据用户ID获取有效用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static Users GetValidUserByUserID(string userID)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidUserByUserID");
            cmd.SetParameterValue("@UserID", userID);
            return cmd.ExecuteEntity<Users>();
        }
        public static List<Users> GetValidUserListByUserID(string userID)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidUserByUserID");
            cmd.SetParameterValue("@UserID", userID);
            return cmd.ExecuteEntityList<Users>();
        }

        /// <summary>
        /// 根据用户编号获取有效用户
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static Users GetValidUserByUserSysNo(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidUserByUserSysNo");
            cmd.SetParameterValue("@SysNo", userSysNo);
            return cmd.ExecuteEntity<Users>();
        }

        /// <summary>
        /// 获取用户存在的权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static List<Functions> GetUserExistsFunctions(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetUserExistsFunctions");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            return cmd.ExecuteEntityList<Functions>();
        }

        /// <summary>
        /// 获取用户不存在的权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static List<Functions> GetUserNotExistsFunctions(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetUserNotExistsFunctions");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            return cmd.ExecuteEntityList<Functions>();
        }

        /// <summary>
        /// 添加用户权限
        /// </summary>
        /// <param name="entity">用户权限信息</param>
        public static void InsertUserFunctions(UserFunctions entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertUserFunctions");
            cmd.SetParameterValue<UserFunctions>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 根据用户编号删除用户权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        public static void DeleteUserFunctionsByUserSysNo(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("DeleteUserFunctionsByUserSysNo");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            cmd.ExecuteNonQuery();
        }
    }
}
