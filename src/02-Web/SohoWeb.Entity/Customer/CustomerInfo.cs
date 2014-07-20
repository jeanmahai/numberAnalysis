using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.Customer
{
    [Serializable]
    [DataContract]
    public class CustomerInfo
    {
        #region 用户基础信息，对应表[SohoCustomer].[dbo].[CustomerBase]

        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public string CustomerID { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string AuthCode { get; set; }
        [DataMember]
        public CustomerStatus Status { get; set; }
        [DataMember]
        public string StatusText { get { return this.Status.GetEnumDescription(); } }
        [DataMember]
        public string RegDate { get; set; }
        [DataMember]
        public string RegIP { get; set; }

        #endregion

        #region 用户扩展业务信息
        #endregion
    }
}
