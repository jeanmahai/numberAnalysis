using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soho.Utility;
using Soho.Utility.Encryption;
using SohoWeb.DataAccess.Customer;
using SohoWeb.Entity.Customer;

namespace SohoWeb.Service.Customer
{
    public class CustomerAuthService : BaseService<CustomerAuthService>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="customerID">用户ID</param>
        /// <param name="userPwd">用户密码，明文</param>
        /// <param name="validateCode">验证码</param>
        /// <param name="IP">IP地址</param>
        /// <returns>true-登录成功；false-登录失败</returns>
        public bool Login(string customerID, string userPwd, string validateCode, string IP)
        {
            var customer = CustomerMgtDA.GetValidCustomerByCustomerID(customerID);

            if (customer == null)
                throw new BusinessException("用户名不存在！");
            var currPassword = MD5Encrypt.MD5Encrypt32(string.Format("{0}-{1}", userPwd, customer.AuthCode)).ToLower();
            if (!currPassword.Equals(customer.Password.ToLower()))
                throw new BusinessException("密码错误！");

            #region 写登录Log
            CustomerLoginLog log = new CustomerLoginLog()
            {
                CustomerSysNo = customer.SysNo.Value,
                LoginIP = IP
            };
            CustomerMgtDA.InsertCustomerLoginLog(log);
            #endregion

            return true;
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="customerID">用户ID</param>
        /// <returns></returns>
        public CustomerInfo GetCustomerByCustomerID(string customerID)
        {
            return CustomerMgtDA.GetValidCustomerByCustomerID(customerID);
        }
    }
}
