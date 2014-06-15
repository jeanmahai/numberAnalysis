using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SohoWeb.Entity
{
    [Serializable]
    [DataContract]
    public class QueryResult<T> where T : class
    {
        public int ServicePageIndex { get; set; }
        /// <summary>
        /// 当前显示第几页数据，从1开始
        /// </summary>
        [DataMember]
        public int PageIndex { get { return (this.ServicePageIndex <= 1 ? 1 : this.ServicePageIndex + 1); } }
        /// <summary>
        /// 每页显示几条数据
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember]
        public int PageCount
        {
            get
            {
                return TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
            }
        }
        /// <summary>
        /// 当前页数据列表
        /// </summary>
        [DataMember]
        public List<T> ResultList { get; set; }
    }
}
