using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using Analysis28.DataService.Entity;
using Analysis28.DataService.Utility;

namespace Analysis28.DataService.Logic
{
    /// <summary>
    /// 采集加拿大数据实现类
    /// </summary>
    public class CollectCanadanData
    {
        #region 单例模式

        private CollectCanadanData()
        { }
        private static CollectCanadanData _Instance;
        public static CollectCanadanData Instance()
        {
            if (_Instance == null)
                _Instance = new CollectCanadanData();
            return _Instance;
        }

        #endregion
        
        /// <summary>
        /// 采集加拿大数据
        /// </summary>
        /// <param name="periodNum">期号</param>
        /// <param name="dtTime">开奖时间(北京时间)</param>
        /// <returns></returns>
        public List<CollectResultEntity> Collect(long periodNum, DateTime dtTime)
        {
            int tryTimes = 21;
            List<CollectResultEntity> result = null;
            while (tryTimes > 0)
            {
                result = CollectCanadanData.Instance().CollectData(periodNum, dtTime);
                if (result == null || result.Count == 0)
                {
                    tryTimes--;
                    //如果采集失败，则间隔3秒重试，重试20次
                    Thread.Sleep(3000);
                }
                else
                {
                    tryTimes = 0;
                }
            }
            return result;
        }
        /// <summary>
        /// 采集加拿大数据
        /// </summary>
        /// <param name="periodNum">期号</param>
        /// <param name="dtTime">开奖时间(北京时间)</param>
        /// <returns></returns>
        public List<CollectResultEntity> CollectData(long periodNum, DateTime dtTime)
        {
            List<CollectResultEntity> result = new List<CollectResultEntity>();

            try
            {
                string url = string.Empty;
                string resultData = string.Empty;

                url = GetConfig.GetXMLValue("PeriodNumUrl");
                dtTime = (new GetTime()).ConvertBeijingToCanadan(dtTime);
                url = string.Format(url, (new GetTime()).FormatCanadanCollectDate(dtTime));
                resultData = HttpHelper.GetHttpData(url);
                if (string.IsNullOrEmpty(resultData) || !resultData.Contains(periodNum.ToString()))
                    return null;

                List<Draw> drawList = new List<Draw>();
                drawList = JsonHelper.JsonToObj<List<Draw>>(resultData);
                if (drawList != null && drawList.Count > 0)
                {
                    drawList = drawList.OrderByDescending(m => m.drawNbr).ToList();
                    long nowMaxPeriodNum = drawList[0].drawNbr;
                    while (true)
                    {
                        Draw item = drawList.Find(delegate(Draw d) { return d.drawNbr == periodNum; });
                        if (item == null || periodNum > nowMaxPeriodNum)
                            break;
                        CollectResultEntity itemResult = new CollectResultEntity();
                        itemResult.Group = item.drawNbrs;
                        if (itemResult.Group != null && itemResult.Group.Length == 20)
                        {
                            itemResult.PeriodNum = item.drawNbr;
                            DateTime retTime = DateTime.Parse(string.Format("{0} {1}", item.drawDate, item.drawTime));
                            itemResult.RetTime = (new GetTime()).ConvertCanadanToBeijing(retTime);
                            itemResult.Result = itemResult.Group[0].ToString();
                            for (int i = 1; i < itemResult.Group.Length; i++)
                            {
                                itemResult.Result += string.Format("|{0}", itemResult.Group[i]);
                            }
                            result.Add(itemResult);
                        }
                        periodNum++;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// 采集期号
        /// </summary>
        /// <returns></returns>
        public CollectResultEntity CollectPeriodNum()
        {
            CollectResultEntity result = new CollectResultEntity();

            int tryTimes = 10;
            while (tryTimes > 0)
            {
                try
                {
                    string url = string.Empty;
                    string resultData = string.Empty;

                    url = GetConfig.GetXMLValue("GetNewPeriodNumUrl");
                    DateTime dtTime = (new GetTime()).ConvertBeijingToCanadan(DateTime.Now);
                    url = string.Format(url, (new GetTime()).FormatCanadanCollectDate(dtTime));
                    resultData = HttpHelper.GetHttpData(url);
                    if (string.IsNullOrEmpty(resultData))
                    {
                        tryTimes--;
                        Thread.Sleep(10000);
                        continue;
                    }

                    List<Draw> drawList = new List<Draw>();
                    drawList = JsonHelper.JsonToObj<List<Draw>>(resultData);
                    if (drawList != null && drawList.Count > 0)
                    {
                        drawList = drawList.OrderByDescending(m => m.drawNbr).ToList();
                        Draw draw = drawList[0];
                        DateTime retTime = DateTime.Parse(string.Format("{0} {1}", draw.drawDate, draw.drawTime));
                        result.RetTime = (new GetTime()).ConvertCanadanToBeijing(retTime);
                        result.PeriodNum = draw.drawNbr;
                        tryTimes = 0;
                    }
                    else
                    {
                        tryTimes--;
                        Thread.Sleep(10000);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    result.PeriodNum = 0;
                    WriteLog.Write(ex.ToString());
                    tryTimes--;
                    Thread.Sleep(10000);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// 加拿大开奖数据实体
    /// </summary>
    public class Draw
    {
        /// <summary>
        /// 期号
        /// </summary>
        public long drawNbr { get; set; }
        /// <summary>
        /// 开奖日期
        /// </summary>
        public string drawDate { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public string drawTime { get; set; }
        /// <summary>
        /// 开奖结果
        /// </summary>
        public int[] drawNbrs { get; set; }
        /// <summary>
        /// × X倍率
        /// </summary>
        public decimal drawBonus { get; set; }
    }
}
