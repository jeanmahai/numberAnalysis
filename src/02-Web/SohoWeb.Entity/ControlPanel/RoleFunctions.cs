using System;
using System.Runtime.Serialization;

using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class RoleFunctions : EntityBase
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public int RoleSysNo { get; set; }
        [DataMember]
        public int FunctionSysNo { get; set; }
    }
}
