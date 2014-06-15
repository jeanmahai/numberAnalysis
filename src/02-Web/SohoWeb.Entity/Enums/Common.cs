using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SohoWeb.Entity.Enums
{
    /// <summary>
    /// 通用状态
    /// </summary>
    [Serializable]
    [DataContract]
    public enum CommonStatus
    {
        /// <summary>
        /// 删除
        /// </summary>
        [EnumMember]
        [Description("删除")]
        Deleted = -100,
        /// <summary>
        /// 无效
        /// </summary>
        [EnumMember]
        [Description("无效")]
        InValid = 0,
        /// <summary>
        /// 有效
        /// </summary>
        [EnumMember]
        [Description("有效")]
        Valid = 100
    }
}
