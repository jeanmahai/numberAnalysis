using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis28.DataService.Utility
{
    public class GetTime
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="source">配置的源类型</param>
        /// <returns></returns>
        public DateTime NowTime(ConfigSource source)
        {
            switch (source)
            {
                case ConfigSource.Beijing:
                    return DateTime.Now;
                case ConfigSource.Canadan:
                    return DateTime.Now;
                default:
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// 北京时间转换为加拿大时间
        /// </summary>
        /// <param name="dt">需要转换的时间</param>
        /// <returns></returns>
        public DateTime ConvertBeijingToCanadan(DateTime dt)
        {
            return dt.AddHours(-15);
        }

        /// <summary>
        /// 加拿大时间转换为北京时间
        /// </summary>
        /// <param name="dt">需要转换的时间</param>
        /// <returns></returns>
        public DateTime ConvertCanadanToBeijing(DateTime dt)
        {
            return dt.AddHours(15);
        }

        /// <summary>
        /// 格式化加拿大采集日期
        /// </summary>
        /// <param name="dtTime">时间</param>
        /// <returns></returns>
        public string FormatCanadanCollectDate(DateTime dtTime)
        {
            string result = "{0}-{1}-{2}";

            int year = dtTime.Year;
            string month = dtTime.Month < 10 ? string.Format("0{0}", dtTime.Month) : dtTime.Month.ToString();
            string day = dtTime.Day < 10 ? string.Format("0{0}", dtTime.Day) : dtTime.Day.ToString();
            result = string.Format(result, year, month, day);

            return result;
        }

        /// <summary>
        /// 格式化北京采集日期
        /// </summary>
        /// <param name="dtTime">时间</param>
        /// <returns></returns>
        public string FormatBeijingCollectDate(DateTime dtTime)
        {
            string result = "{0}-{1}-{2}";

            int year = dtTime.Year;
            string month = dtTime.Month < 10 ? string.Format("0{0}", dtTime.Month) : dtTime.Month.ToString();
            string day = dtTime.Day < 10 ? string.Format("0{0}", dtTime.Day) : dtTime.Day.ToString();
            result = string.Format(result, year, month, day);

            return result;
        }
    }
}
