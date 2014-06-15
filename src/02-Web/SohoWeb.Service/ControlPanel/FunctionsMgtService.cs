using Soho.Utility;
using Soho.Utility.Encryption;

using SohoWeb.Entity;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.DataAccess.ControlPanel;

namespace SohoWeb.Service.ControlPanel
{
    public class FunctionsMgtService : BaseService<FunctionsMgtService>
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="entity">权限信息</param>
        /// <returns></returns>
        public int InsertFunctions(Functions entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.FunctionKey))
                throw new BusinessException("必须输入权限Key！");
            if (string.IsNullOrWhiteSpace(entity.FunctionName))
                throw new BusinessException("必须输入权限名称！");

            var existsList = FunctionsMgtDA.GetValidFunctionsByKey(entity.FunctionKey);
            if (existsList != null && existsList.Count > 0)
            {
                throw new BusinessException("该权限点已存在！");
            }
            return FunctionsMgtDA.InsertFunctions(entity);
        }

        /// <summary>
        /// 根据权限编号更新权限信息
        /// </summary>
        /// <param name="entity">权限信息</param>
        public void UpdateFunctionsBySysNo(Functions entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.FunctionKey))
                throw new BusinessException("必须输入权限Key！");
            if (string.IsNullOrWhiteSpace(entity.FunctionName))
                throw new BusinessException("必须输入权限名称！");

            var existsList = FunctionsMgtDA.GetValidFunctionsByKey(entity.FunctionKey);
            if (existsList != null && existsList.Count > 0 && existsList[0].SysNo != entity.SysNo)
            {
                throw new BusinessException("该权限点已存在！");
            }
            FunctionsMgtDA.UpdateFunctionsBySysNo(entity);
        }

        /// <summary>
        /// 更新权限状态
        /// </summary>
        /// <param name="entity">权限信息</param>
        public void UpdateFunctionsStatusBySysNo(Functions entity)
        {
            FunctionsMgtDA.UpdateFunctionsStatusBySysNo(entity);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public QueryResult<Functions> QueryFunctions(FunctionsQueryFilter filter)
        {
            return FunctionsMgtDA.QueryFunctions(filter);
        }

        /// <summary>
        /// 根据权限编号获取有效权限
        /// </summary>
        /// <param name="sysNo">权限编号</param>
        /// <returns></returns>
        public Functions GetValidFunctionsBySysNo(int sysNo)
        {
            return FunctionsMgtDA.GetValidFunctionsBySysNo(sysNo);
        }
    }
}
