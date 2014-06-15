using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Soho.Utility;

namespace SohoWeb.WebSite
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutorunManager.Startup(ex => Logger.WriteLog(ex.ToString(), "ApplicationException"));
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Logger.WriteLog(ex.ToString(), "ApplicationException");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            AutorunManager.Shutdown(ex => Logger.WriteLog(ex.ToString(), "ApplicationException"));
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                if (exception is HttpException)
                {
                    int statusCode = (exception as HttpException).GetHttpCode();
                    switch (statusCode)
                    {
                        //需处理静态资源找不到不用跳转到404页面
                        case 404:
                            Response.RedirectToRoute("Error404", new { requestUrl = Request.Url.AbsoluteUri });
                            break;
                        default:
                            Logger.WriteLog(exception.ToString(), "HttpException");
                            Response.RedirectToRoute("Error");
                            break;
                    }
                }
                Server.ClearError();
            }
        }
    }
}