using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SohoWeb.Entity.ControlPanel
{
    /// <summary>
    /// 业务日志
    /// </summary>
    [Serializable]
    [DataContract]
    public class Logs : EntityBase
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }
        /// <summary>
        /// 父编号
        /// </summary>
        [DataMember]
        public int ParentSysNo { get; set; }
        /// <summary>
        /// 纲
        /// </summary>
        [DataMember]
        public int Classes { get; set; }
        [DataMember]
        public string ClassesText { get; set; }
        /// <summary>
        /// 目
        /// </summary>
        [DataMember]
        public int Section { get; set; }
        [DataMember]
        public string SectionText { get; set; }
        /// <summary>
        /// 科
        /// </summary>
        [DataMember]
        public int Family { get; set; }
        [DataMember]
        public string FamilyText { get; set; }
        /// <summary>
        /// 业务编号
        /// </summary>
        [DataMember]
        public int RefenceSysNo { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [DataMember]
        public string Contents { get; set; }
    }
}
