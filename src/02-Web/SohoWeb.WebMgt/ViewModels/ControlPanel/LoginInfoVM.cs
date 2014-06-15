using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SohoWeb.WebMgt.ViewModels
{
    [Serializable]
    [DataContract]
    public class LoginInfoVM
    {
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string UserPassword { get; set; }
        [DataMember]
        public string ValidateCode { get; set; }
    }
}