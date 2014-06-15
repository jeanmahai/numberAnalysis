using System;
using System.Runtime.Serialization;

namespace SohoWeb.WebSite.ViewModels
{
    [Serializable]
    [DataContract]
    public class LoginAuthVM
    {
        [DataMember]
        public int UserSysNo { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool RememberLogin { get; set; }
        [DataMember]
        public DateTime LoginDate { get; set; }
        [DataMember]
        public DateTime Timeout { get; set; }
    }
}