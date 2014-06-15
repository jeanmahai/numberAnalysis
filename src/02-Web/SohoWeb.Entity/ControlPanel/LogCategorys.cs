using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SohoWeb.Entity.Enums;

namespace SohoWeb.Entity.ControlPanel
{
    /// <summary>
    /// 日志纲目科
    /// </summary>
    [Serializable]
    [DataContract]
    public class LogCategorys
    {
        [DataMember]
        public int SysNo { get; set; }
        [DataMember]
        public int ParentSysNo { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public CommonStatus Status { get; set; }
        [DataMember]
        public string StatusText { get { return this.Status.GetEnumDescription(); } }
    }
}
