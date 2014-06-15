using System;
using System.Runtime.Serialization;

using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class Functions : EntityBase
    {
        [DataMember]
        public int? SysNo { get; set; }
        [DataMember]
        public string FunctionName { get; set; }
        [DataMember]
        public string FunctionKey { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public CommonStatus Status { get; set; }
        [DataMember]
        public string StatusText { get { return this.Status.GetEnumDescription(); } }
    }
}
