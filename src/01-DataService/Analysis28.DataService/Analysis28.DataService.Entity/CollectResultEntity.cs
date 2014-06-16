using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis28.DataService.Entity
{
    /// <summary>
    /// 采集数据结果实体
    /// </summary>
    public class CollectResultEntity
    {
        /// <summary>
        /// 期号
        /// </summary>
        public long PeriodNum { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime RetTime { get; set; }
        /// <summary>
        /// 采集结果，格式：21|67|38
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 采集结果数组
        /// </summary>
        public int[] Group { get; set; }
    }
}
