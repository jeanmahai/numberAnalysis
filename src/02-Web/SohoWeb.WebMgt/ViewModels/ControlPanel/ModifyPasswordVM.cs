using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SohoWeb.WebMgt.ViewModels
{
    [Serializable]
    [DataContract]
    public class ModifyPasswordVM
    {
        [DataMember]
        public string OldPassword { get; set; }
        [DataMember]
        public string NewPassword { get; set; }
    }
}