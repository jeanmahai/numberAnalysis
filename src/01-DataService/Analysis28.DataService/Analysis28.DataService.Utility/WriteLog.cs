using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Analysis28.DataService.Utility
{
    public class WriteLog
    {
        static object obj = new object();
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Write(string log)
        {
            lock (obj)
            {
                try
                {
                    DateTime dtNow = DateTime.Now;
                    string strDate = string.Format("{0}{1}{2}{3}", dtNow.Year,
                        dtNow.Month < 10 ? "0" + dtNow.Month.ToString() : dtNow.Month.ToString(),
                        dtNow.Day < 10 ? "0" + dtNow.Day.ToString() : dtNow.Day.ToString(),
                        dtNow.Hour < 10 ? "0" + dtNow.Hour.ToString() : dtNow.Hour.ToString());
                    string path = string.Format("{0}\\log\\SvcLog_{1}.txt", AppDomain.CurrentDomain.BaseDirectory, strDate);
                    string logMessage = string.Format("{0}：{1}", dtNow.ToString(), log);
                    FileStream fs = new FileStream(path, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(logMessage + "\r\n");
                    sw.Close();
                    sw.Dispose();
                }
                catch (Exception ex)
                {
                    WriteEventLog(ex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        /// <summary>
        /// 写系统消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="messageType">消息类型</param>
        public static void WriteEventLog(string message, EventLogEntryType messageType)
        {
            EventLog.WriteEntry(GetConfig.GetXMLValue("ServiceName"), message, messageType);
        }
    }
}
