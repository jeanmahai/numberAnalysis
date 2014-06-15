using System;
using System.Runtime.Serialization;

using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class Users : EntityBase
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string UserAuthCode { get; set; }
        [DataMember]
        public CommonStatus Status { get; set; }
        [DataMember]
        public string StatusText { get { return this.Status.GetEnumDescription(); } }
    }
}
