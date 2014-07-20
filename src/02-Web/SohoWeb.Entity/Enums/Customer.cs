using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SohoWeb.Entity.Enums
{
    /// <summary>
    /// 用户状态
    /// </summary>
    [Serializable]
    [DataContract]
    public enum CustomerStatus
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
