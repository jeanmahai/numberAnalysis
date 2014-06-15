using System;
using System.Runtime.Serialization;

namespace SohoWeb.Entity.ControlPanel
{
    [Serializable]
    [DataContract]
    public class LogsQueryFilter : FilterBase
    {
        /// <summary>
        /// 纲
        /// </summary>
        [DataMember]
        public int? Classes { get; set; }
        /// <summary>
        /// 目
        /// </summary>
        [DataMember]
        public int? Section { get; set; }
        /// <summary>
        /// 科
        /// </summary>
        [DataMember]
        public int? Family { get; set; }
        /// <summary>
        /// 业务编号
        /// </summary>
        [DataMember]
        public int? RefenceSysNo { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [DataMember]
        public string Contents { get; set; }
    }
}
