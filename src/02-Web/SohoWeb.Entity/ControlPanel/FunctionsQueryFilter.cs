using System;
using System.Runtime.Serialization;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class FunctionsQueryFilter : FilterBase
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
        public int? Status { get; set; }
    }
}
