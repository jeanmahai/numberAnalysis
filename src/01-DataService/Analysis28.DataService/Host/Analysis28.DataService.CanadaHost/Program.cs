using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;

using Analysis28.DataService.Utility;

namespace Analysis28.DataService.CanadaHost
{
    static class Program
    {
        public static bool isService;

        public static void ExceptionLogger(object sender, UnhandledExceptionEventArgs e)
        {
            object o = e.ExceptionObject;
            if (e.IsTerminating)
            {
                EventLog.WriteEntry(GetConfig.GetXMLValue("ServiceName"), "致命错误！未处理的异常：" + o.ToString(), EventLogEntryType.Error);

            }
            else
            {
                EventLog.WriteEntry(GetConfig.GetXMLValue("ServiceName"), "错误！未处理的异常：" + o.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "start":
                        CollectionSvcInstaller.StartService(args);
                        break;
                    case "stop":
                        CollectionSvcInstaller.StopService();
                        break;
                    case "run":
                        SvcMain.Run();
                        break;
                    case "install":
                        CollectionSvcInstaller.InstallService();
                        break;
                    case "uninstall":
                        CollectionSvcInstaller.UnInstallService();
                        break;
                    default:
                        Console.WriteLine(GetConfig.GetXMLValue("ServiceName") + "-后台服务程序。");
                        string tmpHelpMsg = "参数 <start/stop/run/install/uninstall/help> ";
                        tmpHelpMsg += "\n start     参数将启动28分析数据服务。";
                        tmpHelpMsg += "\n stop      参数将停止28分析数据服务。";
                        tmpHelpMsg += "\n run       参数将运行28分析采集线程。";
                        tmpHelpMsg += "\n install   参数将安装28分析数据服务。";
                        tmpHelpMsg += "\n uninstall 参数将卸载28分析数据服务。";
                        tmpHelpMsg += "\n help      参数将列出帮助。";
                        Console.WriteLine(tmpHelpMsg);
                        break;
                }

                Console.WriteLine("\n按任意键继续。");
                Console.ReadKey(true);
                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new CollectionSvc() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
