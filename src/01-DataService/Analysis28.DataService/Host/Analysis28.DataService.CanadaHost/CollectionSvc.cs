using System;
using System.Threading;
using System.Diagnostics;
using System.ServiceProcess;
using System.ComponentModel;
using System.Configuration.Install;

using Analysis28.DataService.Utility;

namespace Analysis28.DataService.CanadaHost
{
    [RunInstallerAttribute(true)]
    public class CollectionSvcInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public CollectionSvcInstaller()
        {
            // Instantiate installers for process and services.
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // The services run under the system account.
            processInstaller.Account = ServiceAccount.LocalSystem;

            // The services are started manually.
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // ServiceName must equal those on ServiceBase derived classes.            
            serviceInstaller.ServiceName = GetConfig.GetXMLValue("CanadaServiceName");

            serviceInstaller.DisplayName = GetConfig.GetXMLValue("CanadaServiceName");
            serviceInstaller.Description = GetConfig.GetXMLValue("CanadaServiceDesciption");

            // Add installers to collection. Order is not important.
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
            this.AfterInstall += new InstallEventHandler(MyProjectInstaller_AfterInstall);
        }

        void MyProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            if (ServiceIsExist(GetConfig.GetXMLValue("CanadaServiceName")))
                StartService(new string[] { });
        }

        public static void InstallService()
        {
            string[] cmdline = { };
            string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;

            TransactedInstaller instutil = new TransactedInstaller();
            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline);

            Console.WriteLine(GetConfig.GetXMLValue("CanadaServiceName"));

            instutil.Installers.Add(assemblyInstaller);
            instutil.Install(new System.Collections.Hashtable());
            instutil.Dispose();

        }

        public static void UnInstallService()
        {
            string[] cmdline = { };
            string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;

            TransactedInstaller instutil = new TransactedInstaller();
            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline);

            Console.WriteLine("开始卸载" + GetConfig.GetXMLValue("CanadaServiceName") + "。");

            instutil.Installers.Add(assemblyInstaller);
            instutil.Uninstall(null);
            instutil.Dispose();

        }

        public static void StartService(string[] args)
        {
            if (ServiceIsExist(GetConfig.GetXMLValue("CanadaServiceName")))
            {
                ServiceController sc = new ServiceController(GetConfig.GetXMLValue("CanadaServiceName"));
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("开始服务……");

                    if (args.Length == 2 && args[1] == "runnow")

                        sc.Start(new string[] { "runnow" });
                    else
                        sc.Start();

                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 5));

                    Console.WriteLine("服务已开始。");
                }
                else
                {
                    Console.WriteLine("服务并未处在停止状态。");
                }
            }
            else
            {
                Console.WriteLine("服务尚未安装。");
            }
        }

        public static void StopService()
        {
            if (ServiceIsExist(GetConfig.GetXMLValue("CanadaServiceName")))
            {
                ServiceController sc = new ServiceController(GetConfig.GetXMLValue("CanadaServiceName"));
                if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
                {
                    Console.WriteLine("停止服务……");

                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));

                    Console.WriteLine("服务已停止。");
                }
                else
                {
                    Console.WriteLine("服务已停止或正在响应停止请求。");
                }
            }
            else
            {
                Console.WriteLine("服务尚未安装。");
            }
        }

        private static bool ServiceIsExist(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public partial class CollectionSvc : ServiceBase
    {
        private static Thread mainThread;

        public CollectionSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Program.isService = true;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.ExceptionLogger);

            EventLog.WriteEntry(GetConfig.GetXMLValue("CanadaServiceName"), "服务接收到开始指令。", EventLogEntryType.Information);

            SvcMain.running = true;
            mainThread = new Thread(new ThreadStart(SvcMain.Run));
            mainThread.Start();
        }

        protected override void OnStop()
        {
            this.RequestAdditionalTime(10000);

            SvcMain.running = false;
            //等待计算即时数据完成
            if ((mainThread != null) && (mainThread.IsAlive))
            {
                if (mainThread.Join(new TimeSpan(0, 0, 8)))
                {
                    mainThread.Abort();
                }
            }

            EventLog.WriteEntry(GetConfig.GetXMLValue("CanadaServiceName"), "服务接收到终止指令，已终止。", EventLogEntryType.Information);
        }
    }
}
