using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using Analysis28.DataService.Utility;
using Analysis28.DataService.Entity;
using Analysis28.DataService.Logic;

namespace Analysis28.DataService.BeijingHost
{
    public static class SvcMain
    {
        public static volatile bool running;
        static Thread threadNormalSvc;
        static Thread threadFailSvc;

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
                    // 北京28数据服务计算
                    threadNormalSvc = new Thread(new ThreadStart(NormalComputeSvc));
                    threadNormalSvc.Start();
                    threadNormalSvc.IsBackground = true;

                    // 失败计算服务
                    threadFailSvc = new Thread(new ThreadStart(FailComputeSvc));
                    threadFailSvc.Start();
                    threadFailSvc.IsBackground = true;

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
            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Beijing);
            int minutes = 0, seconds = 0;

            if (dtNow.Hour == 23 && dtNow.Minute >= 55)
            {
                return (int)(DateTime.Parse(dtNow.AddDays(1).ToShortDateString() + " 09:05:10") - (new GetTime()).NowTime(ConfigSource.Beijing)).TotalSeconds * 1000;
            }
            else if (dtNow.Hour >= 0 && dtNow.Hour < 9)
            {
                return (int)(DateTime.Parse(dtNow.ToShortDateString() + " 09:05:10") - (new GetTime()).NowTime(ConfigSource.Beijing)).TotalSeconds * 1000;
            }
            else
            {
                minutes = 5 - dtNow.Minute % 5;
                seconds = 10 - dtNow.Second;
                dtNow = dtNow.AddMinutes(minutes).AddSeconds(seconds);
                return (int)(dtNow - (new GetTime()).NowTime(ConfigSource.Beijing)).TotalSeconds * 1000;
            }
        }

        /// <summary>
        /// 北京28数据服务计算
        /// </summary>
        private static void NormalComputeSvc()
        {
            //等待Server配置启动
            Thread.Sleep(10000);

            //计算间断的期
            while (true)
            {
                CollectResultEntity nowPeriod = Arithmetic.Instance().GetNextPeriodNum(Source.Beijing);
                if (nowPeriod.PeriodNum <= 0)
                {
                    WriteLog.Write("读取当前需要开奖的期号失败。");
                    Thread.Sleep(30000);
                    continue;
                }
                //如果当前采集的期开奖时间小于等于当前时间-10秒(每次在整分10秒开奖），则开奖
                //否则跳出间间断计算，等待下一期开奖时间开奖
                if ((int)(nowPeriod.RetTime - (new GetTime()).NowTime(ConfigSource.Beijing)).TotalSeconds <= -10)
                    NormalCompute();
                else
                    break;
            }

            while (running)
            {
                Thread.Sleep(GetSleepTimes());
                AsyncWays_NormalSvc();
            }
        }

        /// <summary>
        /// 失败计算服务
        /// </summary>
        private static void FailComputeSvc()
        {
            int tmpMin = 0;
            int tmpSec = 0;
            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Beijing);
            if (dtNow.Minute % 2 != 0 || dtNow.Second != 0 || (dtNow.Minute % 2 == 0 && dtNow.Second != 0))
            {
                tmpMin = 2 - dtNow.Minute % 2;
                tmpSec = 25 - dtNow.Second;
                Thread.Sleep(tmpMin * 60000 + tmpSec * 1000);
            }
            else
            {
                //等待Server配置启动
                Thread.Sleep(10000);
            }

            while (running)
            {
                AsyncWays_FailSvc();
                DateTime dtNow1 = (new GetTime()).NowTime(ConfigSource.Beijing);
                if (dtNow1.Hour >= 9 && dtNow1.Hour <= 23)
                {
                    tmpMin = 2 - dtNow1.Minute % 2;
                    tmpSec = 25 - dtNow1.Second;
                    Thread.Sleep(tmpMin * 60000 + tmpSec * 1000);
                }
                else
                {
                    tmpMin = 20 - dtNow1.Minute % 20;
                    tmpSec = 25 - dtNow1.Second;
                    Thread.Sleep(tmpMin * 60000 + tmpSec * 1000);
                }
            }
        }

        #endregion

        #region 北京28 异步操作

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
            CollectResultEntity nowPeriod = Arithmetic.Instance().GetNextPeriodNum(Source.Beijing);
            if (nowPeriod.PeriodNum > 0)
            {
                //采集、计算并写入DB持久化
                bool result = Arithmetic.Instance().CollectAndCalculateBeijing(nowPeriod.PeriodNum, nowPeriod.RetTime, DBOperateType.Insert);
                if (!result)
                    Arithmetic.Instance().FailInsertData(Source.Beijing, nowPeriod.PeriodNum, nowPeriod.RetTime);
            }
            else
            {
                WriteLog.Write("读取当前需要开奖的期号失败。");
            }
        }

        #endregion

        #region 北京28间隔计算失败数据 异步操作

        private static void AsyncWays_FailSvc()
        {
            //实例化委托并初赋值 
            DelegateName dn = new DelegateName(FailCompute);
            //实例化回调方法 
            AsyncCallback acb = new AsyncCallback(CallBackMethod);
            //异步开始 
            //如果参数acb 换成 null 则表示没有回调方法 
            //最后一个参数 dn 的地方，可以换成任意对象，该对象可以被回调方法从参数中获取出来，写成 null 也可以。参数 dn 相当于该线程的 ID，如果有多个异步线程，可以都是 null，但是绝对不能一样，不能是同一个 object，否则异常 
            IAsyncResult iar = dn.BeginInvoke(acb, dn);
        }
        private static void FailCompute()
        {
            List<CollectResultEntity> periodList = Arithmetic.Instance().GetFailPeriodList(Source.Beijing);
            if (periodList != null && periodList.Count > 0)
            {
                foreach (CollectResultEntity period in periodList)
                {
                    bool result = Arithmetic.Instance().CollectAndCalculateBeijing(period.PeriodNum, period.RetTime, DBOperateType.Update);
                    if (!result)
                        WriteLog.Write(string.Format("间隔计算失败的期再次失败，期号：{0}。", period.PeriodNum));
                }
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
