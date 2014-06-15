using System.Collections.Generic;

using Soho.Utility.DataAccess;
using SohoWeb.Entity.ControlPanel;

namespace SohoWeb.DataAccess.ControlPanel
{
    public class UserAuthDA
    {
        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static Users GetUserByUserID(string userID)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetUserByUserID");
            cmd.SetParameterValue("@UserID", userID);
            return cmd.ExecuteEntity<Users>();
        }

        /// <summary>
        /// 根据用户编号获取用户权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        public static string[] GetFunctionsByUserSysNo(int userSysNo)
        {
            string[] result = null;
            DataCommand cmd = DataCommandManager.GetDataCommand("GetFunctionsByUserSysNo");
            cmd.SetParameterValue("@UserSysNo", userSysNo);
            var dt = cmd.ExecuteDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                result = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    result[i] = dt.Rows[i][0].ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 获取所有有效权限
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllValidFunctions()
        {
            string[] result = null;
            DataCommand cmd = DataCommandManager.GetDataCommand("GetAllValidFunctions");
            var dt = cmd.ExecuteDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                result = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    result[i] = dt.Rows[i][0].ToString();
                }
            }
            return result;
        }
    }
}
