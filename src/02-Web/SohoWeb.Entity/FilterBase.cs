using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SohoWeb.Entity
{
    [Serializable]
    [DataContract]
    public class FilterBase
    {
        /// <summary>
        /// 当前查询第几页数据，从1开始
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }
        public int ServicePageIndex { get { return (this.PageIndex < 1 ? 0 : this.PageIndex - 1); } }
        /// <summary>
        /// 每页显示几条数据
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
    }
}
