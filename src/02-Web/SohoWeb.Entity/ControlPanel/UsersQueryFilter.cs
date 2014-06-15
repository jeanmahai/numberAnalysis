using System;
using System.Runtime.Serialization;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class UsersQueryFilter : FilterBase
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int? Status { get; set; }
    }
}
