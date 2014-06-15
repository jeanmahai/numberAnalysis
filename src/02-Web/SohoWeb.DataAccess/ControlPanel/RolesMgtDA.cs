using System;
using System.Data;
using System.Collections.Generic;

using SohoWeb.Entity;
using SohoWeb.Entity.ControlPanel;
using Soho.Utility.DataAccess;
using SohoWeb.Entity.Enums;

namespace SohoWeb.DataAccess.ControlPanel
{
    public class RolesMgtDA
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity">角色信息</param>
        /// <returns></returns>
        public static int InsertRoles(Roles entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertRoles");
            cmd.SetParameterValue<Roles>(entity);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.GetParameterValue("@SysNo"));
        }

        /// <summary>
        /// 根据角色编号更新角色信息
        /// </summary>
        /// <param name="entity">角色信息</param>
        public static void UpdateRolesBySysNo(Roles entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateRolesBySysNo");
            cmd.SetParameterValue<Roles>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="entity">角色信息</param>
        public static void UpdateRolesStatusBySysNo(Roles entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateRolesStatusBySysNo");
            cmd.SetParameterValue<Roles>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static QueryResult<Roles> QueryRoles(RolesQueryFilter filter)
        {
            QueryResult<Roles> result = new QueryResult<Roles>();
            result.ServicePageIndex = filter.ServicePageIndex;
            result.PageSize = filter.PageSize;

            PagingInfoEntity page = DataAccessUtil.ToPagingInfo(filter);
            CustomDataCommand cmd = DataCommandManager.CreateCustomDataCommandFromConfig("QueryRoles");
            using (var sqlBuilder = new DynamicQuerySqlBuilder(cmd.CommandText, cmd, page, "SysNo DESC"))
            {
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Status", DbType.Int32,
                    "@Status1", QueryConditionOperatorType.NotEqual, CommonStatus.Deleted);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "SysNo", DbType.Int32,
                    "@SysNo", QueryConditionOperatorType.Equal, filter.SysNo);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "RoleName", DbType.String,
                    "@RoleName", QueryConditionOperatorType.Like, filter.RoleName);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Status", DbType.Int32,
                    "@Status", QueryConditionOperatorType.Equal, filter.Status);

                cmd.CommandText = sqlBuilder.BuildQuerySql();
                result.ResultList = cmd.ExecuteEntityList<Roles>();
                result.TotalCount = Convert.ToInt32(cmd.GetParameterValue("@TotalCount"));

                return result;
            }
        }

        /// <summary>
        /// 根据角色编号获取有效角色
        /// </summary>
        /// <param name="functionKey">角色编号</param>
        /// <returns></returns>
        public static Roles GetValidRolesBySysNo(int sysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidRolesBySysNo");
            cmd.SetParameterValue("@SysNo", sysNo);
            return cmd.ExecuteEntity<Roles>();
        }

        /// <summary>
        /// 根据角色名称获取有效角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public static List<Roles> GetValidRolesByRoleName(string roleName)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidRolesByRoleName");
            cmd.SetParameterValue("@RoleName", roleName);
            return cmd.ExecuteEntityList<Roles>();
        }

        /// <summary>
        /// 根据角色编号获取角色用户
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public static List<RoleUsers> GetRoleUsersByRoleSysNo(int roleSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetRoleUsersByRoleSysNo");
            cmd.SetParameterValue("@RoleSysNo", roleSysNo);
            return cmd.ExecuteEntityList<RoleUsers>();
        }

        /// <summary>
        /// 根据用户编号获取角色用户
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static List<RoleUsers> GetRoleUsersByUserSysNo(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetRoleUsersByUserSysNo");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            return cmd.ExecuteEntityList<RoleUsers>();
        }

        /// <summary>
        /// 根据用户编号获取用户存在的角色
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static List<Roles> GetExistsRoleByUserSysNo(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetExistsRoleByUserSysNo");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            return cmd.ExecuteEntityList<Roles>();
        }

        /// <summary>
        /// 根据用户编号获取用户不存在的角色
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static List<Roles> GetNotExistsRoleByUserSysNo(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetNotExistsRoleByUserSysNo");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            return cmd.ExecuteEntityList<Roles>();
        }

        /// <summary>
        /// 根据用户编号删除用户角色
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        public static void DeleteRoleUsersByUserSysNo(int userSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("DeleteRoleUsersByUserSysNo");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="entity">用户角色信息</param>
        public static void InsertRoleUsers(RoleUsers entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertRoleUsers");
            cmd.SetParameterValue<RoleUsers>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取角色存在的权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public static List<Functions> GetRoleExistsFunctions(int roleSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetRoleExistsFunctions");
            cmd.SetParameterValue("@RoleSysNo", roleSysNo);
            return cmd.ExecuteEntityList<Functions>();
        }

        /// <summary>
        /// 获取角色不存在的权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public static List<Functions> GetRoleNotExistsFunctions(int roleSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetRoleNotExistsFunctions");
            cmd.SetParameterValue("@RoleSysNo", roleSysNo);
            return cmd.ExecuteEntityList<Functions>();
        }

        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="entity">角色权限信息</param>
        public static void InsertRoleFunctions(RoleFunctions entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertRoleFunctions");
            cmd.SetParameterValue<RoleFunctions>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 根据角色编号删除角色权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        public static void DeleteRoleFunctionsByRoleSysNo(int roleSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("DeleteRoleFunctionsByRoleSysNo");
            cmd.SetParameterValue("@RoleSysNo", roleSysNo);
            cmd.ExecuteNonQuery();
        }
    }
}
