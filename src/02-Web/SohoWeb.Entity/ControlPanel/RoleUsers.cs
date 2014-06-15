using System;
using System.Runtime.Serialization;

using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class RoleUsers : EntityBase
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public int RoleSysNo { get; set; }
        [DataMember]
        public int UserSysNo { get; set; }
        [DataMember]
        public CommonStatus Status { get; set; }
        [DataMember]
        public string StatusText { get { return this.Status.GetEnumDescription(); } }
    }
}
