using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Analysis28.DataService.Utility;
using Analysis28.DataService.Logic;
using Analysis28.DataService.Entity;

namespace Analysis28.DataService.CanadaHost
{
    public static class SvcMain
    {
        public static volatile bool running;
        static Thread threadNormalSvc;

        /// <summary>
        /// 启动线程
        /// </summary>
        public static void Run()
        {
            int num = 3;
            while (num > 0)
            {
                try
                {
                    // 加拿大28数据服务计算
                    threadNormalSvc = new Thread(new ThreadStart(NormalComputeSvc));
                    threadNormalSvc.Start();
                    threadNormalSvc.IsBackground = true;

                    num = 0;
                    WriteLog.WriteEventLog("数据服务已启动。", EventLogEntryType.Information);
                }
                catch (Exception ex)
                {
                    num--;
                    WriteLog.WriteEventLog(string.Format("数据服务启动失败。", ex.Message), EventLogEntryType.Error);
                }
            }
        }

        #region 线程计算

        /// <summary>
        /// 计算休眠时间（线程阻塞）
        /// </summary>
        /// <returns></returns>
        private static int GetSleepTimes()
        {
            int proofreadTime = int.Parse(GetConfig.GetXMLValue("ProofreadTime"));
            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Canadan).AddSeconds(proofreadTime);
            int minutes = 0, seconds = 0;

            if (dtNow.Hour == 19)
            {
                return (int)(DateTime.Parse(dtNow.ToShortDateString() + " 20:00:10") - (new GetTime()).NowTime(ConfigSource.Canadan).AddSeconds(proofreadTime)).TotalSeconds * 1000;
            }
            else
            {
                minutes = 4 - dtNow.Minute % 4;
                seconds = 10 - dtNow.Second;
                dtNow = dtNow.AddMinutes(minutes).AddSeconds(seconds);
                return (int)(dtNow - (new GetTime()).NowTime(ConfigSource.Canadan).AddSeconds(proofreadTime)).TotalSeconds * 1000;
            }
        }

        /// <summary>
        /// 加拿大28数据服务计算
        /// </summary>
        private static void NormalComputeSvc()
        {
            //等待Server配置启动
            Thread.Sleep(10000);

            while (running)
            {
                AsyncWays_NormalSvc();
                int sleepSeconds = GetSleepTimes();
                if (sleepSeconds > 0)
                    Thread.Sleep(sleepSeconds);
            }
        }

        #endregion

        #region 加拿大28 异步操作

        private static void AsyncWays_NormalSvc()
        {
            //实例化委托并初赋值 
            DelegateName dn = new DelegateName(NormalCompute);
            //实例化回调方法 
            AsyncCallback acb = new AsyncCallback(CallBackMethod);
            //异步开始 
            //如果参数acb 换成 null 则表示没有回调方法 
            //最后一个参数 dn 的地方，可以换成任意对象，该对象可以被回调方法从参数中获取出来，写成 null 也可以。参数 dn 相当于该线程的 ID，如果有多个异步线程，可以都是 null，但是绝对不能一样，不能是同一个 object，否则异常 
            IAsyncResult iar = dn.BeginInvoke(acb, dn);
        }
        private static void NormalCompute()
        {
            CollectResultEntity nowPeriod = Arithmetic.Instance().GetNextPeriodNum(Source.Canadan);
            if (nowPeriod.PeriodNum > 0)
            {
                //采集、计算并写入DB持久化
                Arithmetic.Instance().CollectAndCalculateCanadan(nowPeriod.PeriodNum, nowPeriod.RetTime);
            }
            else
            {
                WriteLog.Write("读取当前需要开奖的期号失败。");
            }
        }

        #endregion

        #region 异步操作

        //定义与方法同签名的委托 
        private delegate void DelegateName();
        private static void CallBackMethod(IAsyncResult ar)
        {
            //从异步状态 ar.AsyncState 中，获取委托对象 
            DelegateName dn = (DelegateName)ar.AsyncState;
            dn.EndInvoke(ar);
        }

        #endregion
    }
}
