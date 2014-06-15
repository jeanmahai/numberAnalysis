using System.Collections.Generic;

using Soho.Utility;
using Soho.Utility.Encryption;

using SohoWeb.Entity;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.DataAccess.ControlPanel;

namespace SohoWeb.Service.ControlPanel
{
    public class RolesMgtService : BaseService<RolesMgtService>
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity">角色信息</param>
        /// <returns></returns>
        public int InsertRoles(Roles entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.RoleName))
                throw new BusinessException("必须输入角色名！");

            var existsList = RolesMgtDA.GetValidRolesByRoleName(entity.RoleName);
            if (existsList != null && existsList.Count > 0)
            {
                throw new BusinessException("该角色名已存在！");
            }
            return RolesMgtDA.InsertRoles(entity);
        }

        /// <summary>
        /// 根据角色编号更新角色信息
        /// </summary>
        /// <param name="entity">角色信息</param>
        public void UpdateRolesBySysNo(Roles entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.RoleName))
                throw new BusinessException("必须输入角色名！");

            var existsList = RolesMgtDA.GetValidRolesByRoleName(entity.RoleName);
            if (existsList != null && existsList.Count > 0 && existsList[0].SysNo != entity.SysNo)
            {
                throw new BusinessException("该角色名已存在！");
            }
            RolesMgtDA.UpdateRolesBySysNo(entity);
        }

        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="entity">角色信息</param>
        public void UpdateRolesStatusBySysNo(Roles entity)
        {
            RolesMgtDA.UpdateRolesStatusBySysNo(entity);
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public QueryResult<Roles> QueryRoles(RolesQueryFilter filter)
        {
            return RolesMgtDA.QueryRoles(filter);
        }

        /// <summary>
        /// 根据角色编号获取有效角色
        /// </summary>
        /// <param name="functionKey">角色编号</param>
        /// <returns></returns>
        public Roles GetValidRolesBySysNo(int sysNo)
        {
            return RolesMgtDA.GetValidRolesBySysNo(sysNo);
        }

        /// <summary>
        /// 根据角色编号获取角色用户
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public List<RoleUsers> GetRoleUsersByRoleSysNo(int roleSysNo)
        {
            return RolesMgtDA.GetRoleUsersByRoleSysNo(roleSysNo);
        }

        /// <summary>
        /// 根据用户编号获取角色用户
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public List<RoleUsers> GetRoleUsersByUserSysNo(int userSysNo)
        {
            return RolesMgtDA.GetRoleUsersByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 根据用户编号获取用户存在的角色
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public List<Roles> GetExistsRoleByUserSysNo(int userSysNo)
        {
            return RolesMgtDA.GetExistsRoleByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 根据用户编号获取用户不存在的角色
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public List<Roles> GetNotExistsRoleByUserSysNo(int userSysNo)
        {
            return RolesMgtDA.GetNotExistsRoleByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 保存用户角色信息
        /// </summary>
        /// <param name="list">角色信息</param>
        public void SaveUserRoles(List<RoleUsers> list)
        {
            if (list != null && list.Count > 0)
            {
                RolesMgtDA.DeleteRoleUsersByUserSysNo(list[0].UserSysNo);
                foreach (var item in list)
                {
                    RolesMgtDA.InsertRoleUsers(item);
                }
            }
        }

        /// <summary>
        /// 获取角色存在的权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public List<Functions> GetRoleExistsFunctions(int roleSysNo)
        {
            return RolesMgtDA.GetRoleExistsFunctions(roleSysNo);
        }

        /// <summary>
        /// 获取角色不存在的权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public List<Functions> GetRoleNotExistsFunctions(int roleSysNo)
        {
            return RolesMgtDA.GetRoleNotExistsFunctions(roleSysNo);
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="list"></param>
        public void SaveRoleFunctions(List<RoleFunctions> list)
        {
            if (list != null && list.Count > 0)
            {
                RolesMgtDA.DeleteRoleFunctionsByRoleSysNo(list[0].RoleSysNo);
                foreach (var item in list)
                {
                    RolesMgtDA.InsertRoleFunctions(item);
                }
            }
        }
    }
}
