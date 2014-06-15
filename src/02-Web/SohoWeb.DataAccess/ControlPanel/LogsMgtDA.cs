using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soho.Utility.DataAccess;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.Entity;
using System.Data;

namespace SohoWeb.DataAccess.ControlPanel
{
    public class LogsMgtDA
    {
        /// <summary>
        /// 获取所有日志纲目科
        /// </summary>
        /// <returns></returns>
        public static List<Logs> GetLogCategorys()
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetLogCategorys");
            return cmd.ExecuteEntityList<Logs>();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="entity">日志信息</param>
        /// <returns></returns>
        public static int InsertLogs(Logs entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertLogs");
            cmd.SetParameterValue<Logs>(entity);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.GetParameterValue("@SysNo"));
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static QueryResult<Logs> QueryLogs(LogsQueryFilter filter)
        {
            QueryResult<Logs> result = new QueryResult<Logs>();
            result.ServicePageIndex = filter.ServicePageIndex;
            result.PageSize = filter.PageSize;

            PagingInfoEntity page = DataAccessUtil.ToPagingInfo(filter);
            CustomDataCommand cmd = DataCommandManager.CreateCustomDataCommandFromConfig("QueryLogs");
            using (var sqlBuilder = new DynamicQuerySqlBuilder(cmd.CommandText, cmd, page, "SysNo DESC"))
            {
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Classes", DbType.Int32,
                    "@Classes", QueryConditionOperatorType.Like, filter.Classes);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Section", DbType.Int32,
                    "@Section", QueryConditionOperatorType.Like, filter.Section);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Family", DbType.Int32,
                    "@Family", QueryConditionOperatorType.Like, filter.Family);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "RefenceSysNo", DbType.Int32,
                    "@RefenceSysNo", QueryConditionOperatorType.Like, filter.RefenceSysNo);
                sqlBuilder.ConditionConstructor.AddCondition(QueryConditionRelationType.AND, "Contents", DbType.String,
                    "@Contents", QueryConditionOperatorType.Like, filter.Contents);

                cmd.CommandText = sqlBuilder.BuildQuerySql();
                result.ResultList = cmd.ExecuteEntityList<Logs>();
                result.TotalCount = Convert.ToInt32(cmd.GetParameterValue("@TotalCount"));

                return result;
            }
        }
    }
}
