using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.DataAccess.ControlPanel;
using SohoWeb.Entity;

namespace SohoWeb.Service.ControlPanel
{
    public class LogsMgtService : BaseService<LogsMgtService>
    {
        /// <summary>
        /// 获取日志的纲
        /// </summary>
        /// <returns></returns>
        public List<Logs> GetLogClasses()
        {
            var dataList = LogsMgtDA.GetLogCategorys();
            if (dataList != null && dataList.Count > 0)
            {
                return dataList.FindAll(m => m.ParentSysNo == 0);
            }
            return null;
        }

        /// <summary>
        /// 根据纲获取日志目
        /// </summary>
        /// <param name="classes">纲</param>
        /// <returns></returns>
        public List<Logs> GetLogSectionByClasses(int classes)
        {
            var dataList = LogsMgtDA.GetLogCategorys();
            if (dataList != null && dataList.Count > 0)
            {
                return dataList.FindAll(m => m.ParentSysNo == classes);
            }
            return null;
        }

        /// <summary>
        /// 根据目获取日志科
        /// </summary>
        /// <param name="section">目</param>
        /// <returns></returns>
        public List<Logs> GetLogFamliyBySection(int section)
        {
            var dataList = LogsMgtDA.GetLogCategorys();
            if (dataList != null && dataList.Count > 0)
            {
                return dataList.FindAll(m => m.ParentSysNo == section);
            }
            return null;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="entity">日志信息</param>
        /// <returns></returns>
        public int InsertLogs(Logs entity)
        {
            return LogsMgtDA.InsertLogs(entity);
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public QueryResult<Logs> QueryLogs(LogsQueryFilter filter)
        {
            return LogsMgtDA.QueryLogs(filter);
        }
    }
}
