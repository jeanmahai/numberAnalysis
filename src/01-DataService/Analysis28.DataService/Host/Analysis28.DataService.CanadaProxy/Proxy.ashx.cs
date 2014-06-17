using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Analysis28.DataService.CanadaProxy
{
    /// <summary>
    /// Proxy 的摘要说明
    /// </summary>
    public class Proxy : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["d"] != null && context.Request["d"].Trim().Length > 0)
            {
                string date = context.Request["d"].Trim();
                string url = string.Format("http://lotto.bclc.com/services2/keno/draw/{0}", date);
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string result = webClient.DownloadString(url);
                context.Response.Write(result);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}