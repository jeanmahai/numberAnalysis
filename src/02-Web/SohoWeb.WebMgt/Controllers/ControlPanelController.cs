using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using Soho.Utility;
using Soho.Utility.Web.Framework;
using SohoWeb.Service.ControlPanel;
using SohoWeb.Entity.ControlPanel;
using SohoWeb.WebMgt.ViewModels;
using SohoWeb.Entity;
using SohoWeb.Entity.Enums;
using SohoWeb.WebMgt.ViewModels.ControlPanel;

namespace SohoWeb.WebMgt.Controllers
{
    /// <summary>
    /// 控制面板
    /// </summary>
    public class ControlPanelController : SSLController
    {
        /// <summary>
        /// 获取控制面板通用状态枚举列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCommonStatusList()
        {
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = EnumsHelper.GetKeyValuePairs<CommonStatus>(EnumAppendItemType.Select),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 获取指定key该用户是否有权限
        /// </summary>
        /// <returns></returns>
        public ActionResult GetIsAllowByKey()
        {
            var request = GetParams<List<string>>();

            bool bResult = false;
            if (request != null && request.Count > 0)
            {
                bResult = (new AuthMgr()).IsAllowed(request[0]);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = bResult,
                Message = ""
            };
            return View(result);
        }

        #region 用户

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertUser()
        {
            var requestVM = GetParams<Users>();
            this.SetEntityBase(requestVM, true);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = UsersMgtService.Instance.InsertUser(requestVM),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateUser()
        {
            var requestVM = GetParams<Users>();
            this.SetEntityBase(requestVM, false);
            UsersMgtService.Instance.UpdateUserBySysNo(requestVM);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 更新权限状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateUserStatus()
        {
            var user = GetParams<Users>();
            UsersMgtService.Instance.UpdateUserStatusBySysNo(user);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteUser()
        {
            var request = GetParams<List<string>>();

            if (request != null && request.Count > 0)
            {
                foreach (string str in request)
                {
                    Users entity = new Users()
                    {
                        SysNo = int.Parse(str),
                        Status = Entity.Enums.CommonStatus.Deleted
                    };
                    this.SetEntityBase(entity, false);
                    UsersMgtService.Instance.UpdateUserStatusBySysNo(entity);
                }
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPassword()
        {
            var requestVM = GetParams<ModifyPasswordVM>();
            Users entity = new Users()
            {
                UserID = this.CurrUser.UserID,
                Password = requestVM.NewPassword
            };
            this.SetEntityBase(entity, false);
            UsersMgtService.Instance.UpdateUserPasswordBySysNo(entity, requestVM.OldPassword);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryUsers()
        {
            var filter = GetParams<UsersQueryFilter>();
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = UsersMgtService.Instance.QueryUsers(filter),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 根据用户编号获取有效用户
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserByUserSysNo()
        {
            var user = GetParams<Users>();
            int sysNo = user.SysNo.Value;

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = UsersMgtService.Instance.GetValidUserByUserSysNo(sysNo),
                Message = ""
            };
            return View(result);
        }
        #endregion

        #region 权限点

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertFunctions()
        {
            var requestVM = GetParams<Functions>();
            this.SetEntityBase(requestVM, true);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = FunctionsMgtService.Instance.InsertFunctions(requestVM),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateFunction()
        {
            var requestVM = GetParams<Functions>();
            this.SetEntityBase(requestVM, false);
            FunctionsMgtService.Instance.UpdateFunctionsBySysNo(requestVM);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 更新权限状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateFunctionStatus()
        {
            var functions = GetParams<Functions>();
            FunctionsMgtService.Instance.UpdateFunctionsStatusBySysNo(functions);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteFunction()
        {
            var request = GetParams<List<string>>();

            if (request != null && request.Count > 0)
            {
                foreach (string str in request)
                {
                    Functions entity = new Functions()
                    {
                        SysNo = int.Parse(str),
                        Status = Entity.Enums.CommonStatus.Deleted
                    };
                    this.SetEntityBase(entity, false);
                    FunctionsMgtService.Instance.UpdateFunctionsStatusBySysNo(entity);
                }
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryFunctions()
        {
            var filter = GetParams<FunctionsQueryFilter>();
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = FunctionsMgtService.Instance.QueryFunctions(filter),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 根据权限编号获取有效权限
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFunctionsBySysNo()
        {
            var user = GetParams<Functions>();
            int sysNo = user.SysNo.Value;

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = FunctionsMgtService.Instance.GetValidFunctionsBySysNo(sysNo),
                Message = ""
            };
            return View(result);
        }
        #endregion

        #region 角色

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertRoles()
        {
            var requestVM = GetParams<Roles>();
            this.SetEntityBase(requestVM, true);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = RolesMgtService.Instance.InsertRoles(requestVM),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateRole()
        {
            var requestVM = GetParams<Roles>();
            this.SetEntityBase(requestVM, false);
            RolesMgtService.Instance.UpdateRolesBySysNo(requestVM);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateRoleStatus()
        {
            var role = GetParams<Roles>();
            RolesMgtService.Instance.UpdateRolesStatusBySysNo(role);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteRole()
        {
            var request = GetParams<List<string>>();

            if (request != null && request.Count > 0)
            {
                foreach (string str in request)
                {
                    Roles entity = new Roles()
                    {
                        SysNo = int.Parse(str),
                        Status = Entity.Enums.CommonStatus.Deleted
                    };
                    this.SetEntityBase(entity, false);
                    RolesMgtService.Instance.UpdateRolesStatusBySysNo(entity);
                }
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryRoles()
        {
            var filter = GetParams<RolesQueryFilter>();
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = RolesMgtService.Instance.QueryRoles(filter),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 根据角色编号获取有效角色
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRolesBySysNo()
        {
            var user = GetParams<Roles>();
            int sysNo = user.SysNo.Value;

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = RolesMgtService.Instance.GetValidRolesBySysNo(sysNo),
                Message = ""
            };
            return View(result);
        }

        #endregion

        #region 角色用户管理

        /// <summary>
        /// 根据角色编号获取角色用户
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleUsersByRoleSysNo()
        {
            var request = GetParams<List<string>>();

            List<RoleUsers> data = null;
            if (request != null && request.Count > 0)
            {
                int roleSysNo = int.Parse(request[0]);
                data = RolesMgtService.Instance.GetRoleUsersByRoleSysNo(roleSysNo);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = data,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 根据用户编号获取角色用户
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleUsersByUserSysNo()
        {
            var request = GetParams<List<string>>();

            List<RoleUsers> data = null;
            if (request != null && request.Count > 0)
            {
                int userSysNo = int.Parse(request[0]);
                data = RolesMgtService.Instance.GetRoleUsersByUserSysNo(userSysNo);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = data,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 获取用户存在不存在的角色信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserRolesInfo()
        {
            var request = GetParams<List<string>>();

            UserRolesInfoVM data = new UserRolesInfoVM();

            if (request != null && request.Count > 0)
            {
                int userSysNo = int.Parse(request[0]);
                data.ExistsRoles = RolesMgtService.Instance.GetExistsRoleByUserSysNo(userSysNo);
                data.NotExistsRoles = RolesMgtService.Instance.GetNotExistsRoleByUserSysNo(userSysNo);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = data,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 保存用户角色信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveUserRoles()
        {
            var requestVM = GetParams<List<RoleUsers>>();

            if (requestVM != null && requestVM.Count > 0)
            {
                requestVM.ForEach(m =>
                {
                    this.SetEntityBase(m);
                });
            }
            RolesMgtService.Instance.SaveUserRoles(requestVM);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        #endregion

        #region 角色权限管理

        /// <summary>
        /// 获取角色存在不存在的权限信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleFunctionsInfo()
        {
            var request = GetParams<List<string>>();

            FunctionsMaintainVM data = new FunctionsMaintainVM();

            if (request != null && request.Count > 0)
            {
                int roleSysNo = int.Parse(request[0]);
                data.ExistsFunctions = RolesMgtService.Instance.GetRoleExistsFunctions(roleSysNo);
                data.NotExistsFunctions = RolesMgtService.Instance.GetRoleNotExistsFunctions(roleSysNo);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = data,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveRoleFunctions()
        {
            var requestVM = GetParams<List<RoleFunctions>>();

            if (requestVM != null && requestVM.Count > 0)
            {
                requestVM.ForEach(m =>
                {
                    this.SetEntityBase(m);
                });
            }
            RolesMgtService.Instance.SaveRoleFunctions(requestVM);
            if (requestVM != null && requestVM.Count > 0)
            {
                requestVM.ForEach(m =>
                {
                    var users = RolesMgtService.Instance.GetRoleUsersByRoleSysNo(m.RoleSysNo);
                    if (users != null && users.Count > 0)
                    {
                        users.ForEach(n =>
                        {
                            (new AuthMgr()).RefreshUserFunctions(n.UserSysNo);
                        });
                    }
                });
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        #endregion

        #region 用户权限管理

        /// <summary>
        /// 获取用户存在不存在的权限信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserFunctionsInfo()
        {
            var request = GetParams<List<string>>();

            FunctionsMaintainVM data = new FunctionsMaintainVM();

            if (request != null && request.Count > 0)
            {
                int userSysNo = int.Parse(request[0]);
                data.ExistsFunctions = UsersMgtService.Instance.GetUserExistsFunctions(userSysNo);
                data.NotExistsFunctions = UsersMgtService.Instance.GetUserNotExistsFunctions(userSysNo);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = data,
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveUserFunctions()
        {
            var requestVM = GetParams<List<UserFunctions>>();

            int userSysNo = 0;
            if (requestVM != null && requestVM.Count > 0)
            {
                userSysNo = requestVM[0].UserSysNo;
                requestVM.ForEach(m =>
                {
                    this.SetEntityBase(m);
                });
            }
            UsersMgtService.Instance.SaveUserFunctions(requestVM);
            if (userSysNo > 0)
            {
                (new AuthMgr()).RefreshUserFunctions(userSysNo);
            }

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = true,
                Message = ""
            };
            return View(result);
        }

        #endregion

        #region 日志管理

        /// <summary>
        /// 获取日志的纲
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLogClasses()
        {
            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = LogsMgtService.Instance.GetLogClasses(),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 根据纲获取日志目
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLogSectionByClasses()
        {
            var request = GetParams<List<string>>();
            int classes = 0;
            if (request != null && request.Count > 0)
                classes = int.Parse(request[0]);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = LogsMgtService.Instance.GetLogSectionByClasses(classes),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 根据目获取日志科
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLogFamliyBySection()
        {
            var request = GetParams<List<string>>();
            int section = 0;
            if (request != null && request.Count > 0)
                section = int.Parse(request[0]);

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = LogsMgtService.Instance.GetLogFamliyBySection(section),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertLogs()
        {
            var requestVM = GetParams<Logs>();

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = LogsMgtService.Instance.InsertLogs(requestVM),
                Message = ""
            };
            return View(result);
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryLogs()
        {
            var filter = GetParams<LogsQueryFilter>();

            PortalResult result = new PortalResult()
            {
                Code = 0,
                Success = true,
                Data = LogsMgtService.Instance.QueryLogs(filter),
                Message = ""
            };
            return View(result);
        }
        #endregion
    }
}
