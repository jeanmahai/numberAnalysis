using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Analysis28.DataService.Utility
{
    /// <summary>
    /// HTTP工具类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 采集数据
        /// </summary>
        /// <param name="url">http请求地址</param>
        /// <returns></returns>
        public static string GetHttpData(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(936));
                string result = sr.ReadToEnd();
                sr.Close();
                res.Close();
                return result;
            }
            catch (Exception ex)
            {
                string message = string.Format("URL:{0}\r\n异常信息：{1}", url, ex.ToString());
                WriteLog.Write(message);
                return "";
            }
        }
        /// <summary>
        /// 采集数据
        /// </summary>
        /// <param name="url">http请求地址</param>
        /// <param name="contentType">类型</param>
        /// <returns></returns>
        public static string GetHttpData(string url, string contentType)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(contentType))
                    req.ContentType = contentType;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(936));
                string result = sr.ReadToEnd();
                sr.Close();
                res.Close();
                return result;
            }
            catch (Exception ex)
            {
                string message = string.Format("URL:{0}\r\n异常信息：{1}", url, ex.ToString());
                WriteLog.Write(message);
                return "";
            }
        }
        /// <summary>
        /// 采集数据
        /// </summary>
        /// <param name="url">http请求地址</param>
        /// <returns></returns>
        public static string GetHttpDataUTF8(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                res.Close();
                return result;
            }
            catch (Exception ex)
            {
                string message = string.Format("URL:{0}\r\n异常信息：{1}", url, ex.ToString());
                WriteLog.Write(message);
                return "";
            }
        }
    }
}
