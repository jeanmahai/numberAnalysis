using System;
using System.Data;
using System.Collections.Generic;

using SohoWeb.Entity;
using SohoWeb.Entity.ControlPanel;
using Soho.Utility.DataAccess;
using SohoWeb.Entity.Enums;

namespace SohoWeb.DataAccess.ControlPanel
{
    public class FunctionsMgtDA
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="entity">权限信息</param>
        /// <returns></returns>
        public static int InsertFunctions(Functions entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertFunctions");
            cmd.SetParameterValue<Functions>(entity);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.GetParameterValue("@SysNo"));
        }

        /// <summary>
        /// 根据权限编号更新权限信息
        /// </summary>
        /// <param name="entity">权限信息</param>
        public static void UpdateFunctionsBySysNo(Functions entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateFunctionsBySysNo");
            cmd.SetParameterValue<Functions>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新权限状态
        /// </summary>
        /// <param name="entity">权限信息</param>
        public static void UpdateFunctionsStatusBySysNo(Functions entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateFunctionsStatusBySysNo");
            cmd.SetParameterValue<Functions>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static QueryResult<Functions> QueryFunctions(FunctionsQueryFilter filter)
        {
            QueryResult<Functions> result = new QueryResult<Functions>();
            result.ServicePageIndex = filter.ServicePageIndex;
            result.PageSize = filter.PageSize;

            PagingInfoEntity page = DataAccessUtil.ToPagingInfo(filter);
            CustomDataCommand cmd = DataCommandManager.CreateCustomDataCommandFromConfig("QueryFunctions");
            using (var sqlBuilder = new DynamicQuerySqlBuilder(cmd.CommandText, cmd, page, "SysNo DESC"))
            {
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Status", DbType.Int32,
                    "@Status1", QueryConditionOperatorType.NotEqual, CommonStatus.Deleted);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "SysNo", DbType.Int32,
                    "@SysNo", QueryConditionOperatorType.Equal, filter.SysNo);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "FunctionName", DbType.String,
                    "@FunctionName", QueryConditionOperatorType.Like, filter.FunctionName);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "FunctionKey", DbType.String,
                    "@FunctionKey", QueryConditionOperatorType.Like, filter.FunctionKey);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Description", DbType.String,
                    "@Description", QueryConditionOperatorType.Like, filter.Description);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Status", DbType.Int32,
                    "@Status", QueryConditionOperatorType.Equal, filter.Status);

                cmd.CommandText = sqlBuilder.BuildQuerySql();
                result.ResultList = cmd.ExecuteEntityList<Functions>();
                result.TotalCount = Convert.ToInt32(cmd.GetParameterValue("@TotalCount"));

                return result;
            }
        }

        /// <summary>
        /// 根据权限编号获取有效权限
        /// </summary>
        /// <param name="functionKey">权限编号</param>
        /// <returns></returns>
        public static Functions GetValidFunctionsBySysNo(int sysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidFunctionsBySysNo");
            cmd.SetParameterValue("@SysNo", sysNo);
            return cmd.ExecuteEntity<Functions>();
        }

        /// <summary>
        /// 根据权限Key获取有效权限
        /// </summary>
        /// <param name="functionKey">权限Key</param>
        /// <returns></returns>
        public static List<Functions> GetValidFunctionsByKey(string functionKey)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidFunctionsByKey");
            cmd.SetParameterValue("@FunctionKey", functionKey);
            return cmd.ExecuteEntityList<Functions>();
        }
    }
}
