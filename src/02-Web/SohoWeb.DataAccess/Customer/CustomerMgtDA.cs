using System;
using System.Collections.Generic;

using Soho.Utility.DataAccess;
using SohoWeb.Entity.Customer;

namespace SohoWeb.DataAccess.Customer
{
    public class CustomerMgtDA
    {
        /// <summary>
        /// 添加用户基础信息
        /// </summary>
        /// <param name="entity">用户信息</param>
        /// <returns></returns>
        public static int InsertCustomerBase(CustomerInfo entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertCustomerBase");
            cmd.SetParameterValue<CustomerInfo>(entity);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.GetParameterValue("@SysNo"));
        }

        /// <summary>
        /// 根据用户编号更新用户基础信息
        /// </summary>
        /// <param name="entity">用户信息</param>
        public static void UpdateCustomerBaseBySysNo(CustomerInfo entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateCustomerBaseBySysNo");
            cmd.SetParameterValue<CustomerInfo>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="entity">用户信息</param>
        public static void UpdateCustomerStatusBySysNo(CustomerInfo entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateCustomerStatusBySysNo");
            cmd.SetParameterValue<CustomerInfo>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="entity">用户信息</param>
        public static void UpdateCustomerPasswordByCustomerID(CustomerInfo entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("UpdateCustomerPasswordByCustomerID");
            cmd.SetParameterValue<CustomerInfo>(entity);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 根据用户ID获取有效用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static CustomerInfo GetValidCustomerByCustomerID(string customerID)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidCustomerByCustomerID");
            cmd.SetParameterValue("@CustomerID", customerID);
            return cmd.ExecuteEntity<CustomerInfo>();
        }
        public static List<CustomerInfo> GetValidCustomerListByCustomerID(string customerID)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidCustomerByCustomerID");
            cmd.SetParameterValue("@CustomerID", customerID);
            return cmd.ExecuteEntityList<CustomerInfo>();
        }

        /// <summary>
        /// 根据用户编号获取有效用户
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns></returns>
        public static CustomerInfo GetValidCustomerByCustomerSysNo(int customerSysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("GetValidCustomerByCustomerSysNo");
            cmd.SetParameterValue("@SysNo", customerSysNo);
            return cmd.ExecuteEntity<CustomerInfo>();
        }

        /// <summary>
        /// 创建用户登录日志
        /// </summary>
        /// <param name="entity">登录日志</param>
        public static void InsertCustomerLoginLog(CustomerLoginLog entity)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("InsertCustomerLoginLog");
            cmd.SetParameterValue<CustomerLoginLog>(entity);
            cmd.ExecuteNonQuery();
        }
    }
}
