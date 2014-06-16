using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis28.DataService.Entity
{
    /// <summary>
    /// 采集计算结果实体，对应源数据表
    /// </summary>
    public class SourceDataEntity
    {
        public SourceDataEntity()
        {
            PeriodNum = 0;
            RetTime = DateTime.Now;
            SiteSysNo = 0;
            RetOddNum = 0;
            RetNum = 0;
            RetMidNum = "";
            CollectRet = "";
            CollectTime = DateTime.Now;
            Status = -1;
        }
        public long PeriodNum { get; set; }
        public DateTime RetTime { get; set; }
        public int SiteSysNo { get; set; }
        public int RetOddNum { get; set; }
        public int RetNum { get; set; }
        public string RetMidNum { get; set; }
        public string CollectRet { get; set; }
        public DateTime CollectTime { get; set; }
        public int Status { get; set; }
    }
}
