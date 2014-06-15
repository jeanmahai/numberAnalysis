using System.Collections.Generic;

using SohoWeb.Entity.ControlPanel;

namespace SohoWeb.WebMgt.ViewModels.ControlPanel
{
    /// <summary>
    /// 用户存在不存在的角色信息
    /// </summary>
    public class UserRolesInfoVM
    {
        /// <summary>
        /// 存在的角色
        /// </summary>
        public List<Roles> ExistsRoles { get; set; }
        /// <summary>
        /// 不存在的角色
        /// </summary>
        public List<Roles> NotExistsRoles { get; set; }
    }
}