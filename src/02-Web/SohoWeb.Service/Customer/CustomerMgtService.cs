using System.Collections.Generic;

using Soho.Utility;
using Soho.Utility.Encryption;
using SohoWeb.Entity.Customer;
using SohoWeb.DataAccess.Customer;

namespace SohoWeb.Service.Customer
{
    public class CustomerMgtService : BaseService<CustomerMgtService>
    {
        /// <summary>
        /// 添加用户基础信息
        /// </summary>
        /// <param name="entity">用户信息</param>
        /// <returns></returns>
        public int InsertCustomer(CustomerInfo entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.CustomerID))
                throw new BusinessException("必须输入用户ID！");
            if (string.IsNullOrWhiteSpace(entity.Password))
                throw new BusinessException("必须输入密码！");

            var customerList = CustomerMgtDA.GetValidCustomerListByCustomerID(entity.CustomerID);
            if (customerList != null && customerList.Count > 0)
            {
                throw new BusinessException("用户ID已经存在！");
            }

            if (string.IsNullOrWhiteSpace(entity.CustomerName))
                entity.CustomerName = entity.CustomerID;
            entity.AuthCode = GuidCode.GetGuid("D");
            entity.Password = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", entity.Password, entity.AuthCode)).ToLower();

            int customerSysNo = CustomerMgtDA.InsertCustomerBase(entity);

            return customerSysNo;
        }

        /// <summary>
        /// 根据用户编号更新用户基础信息
        /// </summary>
        /// <param name="entity">用户信息</param>
        public void UpdateCustomerBaseBySysNo(CustomerInfo entity)
        {
            //check
            if (string.IsNullOrWhiteSpace(entity.CustomerID))
                throw new BusinessException("必须输入用户ID！");
            if (string.IsNullOrWhiteSpace(entity.CustomerName))
                throw new BusinessException("必须输入用户名！");

            var customerList = CustomerMgtDA.GetValidCustomerListByCustomerID(entity.CustomerID);
            if (customerList != null && customerList.Count > 0)
            {
                if (customerList.Count == 1 && customerList[0].SysNo.Value != entity.SysNo.Value)
                    throw new BusinessException("用户ID已经存在！");
                if (customerList.Count > 1)
                    throw new BusinessException("用户ID已经存在！");
            }

            CustomerMgtDA.UpdateCustomerBaseBySysNo(entity);
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="entity">用户信息</param>
        public void UpdateCustomerStatusBySysNo(CustomerInfo entity)
        {
            CustomerMgtDA.UpdateCustomerStatusBySysNo(entity);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="entity">用户信息</param>
        /// <param name="oldPassword">旧密码</param>
        public void UpdateCustomerPasswordByUserID(CustomerInfo entity, string oldPassword)
        {
            //check
            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new BusinessException("必须输入旧密码！");
            if (string.IsNullOrWhiteSpace(entity.Password))
                throw new BusinessException("必须输入新密码！");

            var customer = CustomerMgtDA.GetValidCustomerByCustomerID(entity.CustomerID);
            string currOldPassword = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", oldPassword, customer.AuthCode)).ToLower();
            if (!currOldPassword.Equals(customer.Password))
                throw new BusinessException("旧密码错误！");

            entity.Password = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", entity.Password, customer.AuthCode)).ToLower();

            CustomerMgtDA.UpdateCustomerPasswordByCustomerID(entity);
        }

        /// <summary>
        /// 根据用户ID获取有效用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public CustomerInfo GetValidCustomerByCustomerID(string customerID)
        {
            return CustomerMgtDA.GetValidCustomerByCustomerID(customerID);
        }
        public List<CustomerInfo> GetValidCustomerListByCustomerID(string customerID)
        {
            return CustomerMgtDA.GetValidCustomerListByCustomerID(customerID);
        }

        /// <summary>
        /// 根据用户编号获取有效用户
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns></returns>
        public CustomerInfo GetValidCustomerByCustomerSysNo(int customerSysNo)
        {
            return CustomerMgtDA.GetValidCustomerByCustomerSysNo(customerSysNo);
        }
    }
}
