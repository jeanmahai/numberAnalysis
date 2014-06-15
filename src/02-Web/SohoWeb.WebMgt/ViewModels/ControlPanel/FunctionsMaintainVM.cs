using System.Collections.Generic;

using SohoWeb.Entity.ControlPanel;

namespace SohoWeb.WebMgt.ViewModels.ControlPanel
{
    /// <summary>
    /// 权限存在不存在信息
    /// </summary>
    public class FunctionsMaintainVM
    {
        /// <summary>
        /// 存在的权限
        /// </summary>
        public List<Functions> ExistsFunctions { get; set; }
        /// <summary>
        /// 不存在的权限
        /// </summary>
        public List<Functions> NotExistsFunctions { get; set; }
    }
}