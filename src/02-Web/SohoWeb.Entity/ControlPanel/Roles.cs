using System;
using System.Runtime.Serialization;

using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class Roles : EntityBase
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public CommonStatus Status { get; set; }
        [DataMember]
        public string StatusText { get { return this.Status.GetEnumDescription(); } }
    }
}
