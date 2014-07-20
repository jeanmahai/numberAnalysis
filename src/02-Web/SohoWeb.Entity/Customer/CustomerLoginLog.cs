using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SohoWeb.Entity.Customer
{
    /// <summary>
    /// 用户登录日志
    /// </summary>
    [Serializable]
    [DataContract]
    public class CustomerLoginLog
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public int CustomerSysNo { get; set; }
        [DataMember]
        public string LoginDate { get; set; }
        [DataMember]
        public string LoginIP { get; set; }
    }
}
