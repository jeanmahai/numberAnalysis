using System;
using System.Collections.Generic;

using Analysis28.DataService.Entity;
using Analysis28.DataService.Utility;
using Analysis28.DataService.DataAccess;

namespace Analysis28.DataService.Logic
{
    /// <summary>
    /// 算法
    /// </summary>
    public class Arithmetic
    {
        #region 单例模式

        private Arithmetic()
        { }
        private static Arithmetic _Instance;
        public static Arithmetic Instance()
        {
            if (_Instance == null)
                _Instance = new Arithmetic();
            return _Instance;
        }

        #endregion

        /// <summary>
        /// 采集并计算北京数据
        /// </summary>
        /// <param name="periodNum">期号</param>
        /// <param name="retTime">开奖时间</param>
        /// <param name="dbOperateType">DB操作类型</param>
        /// <returns></returns>
        public bool CollectAndCalculateBeijing(long periodNum, DateTime retTime, DBOperateType dbOperateType)
        {
            bool result = true;

            string errorMessage = "{0}，方法：CollectAndCalculate，期号：" + periodNum.ToString();

            try
            {
                #region 采集数据
                CollectResultEntity collectResult = CollectBeijingData.Instance().Collect(periodNum, retTime);
                if (collectResult == null || collectResult.Group == null || collectResult.Group.Length != 20)
                {
                    WriteLog.Write(string.Format(errorMessage, "采集数据失败"));
                    return false;
                }
                collectResult.PeriodNum = periodNum;
                collectResult.RetTime = retTime;
                #endregion

                #region 计算数据
                //28数据
                List<SourceDataEntity> sourceDataList28 = Calculate28Data(collectResult);
                if (sourceDataList28 == null || sourceDataList28.Count == 0)
                {
                    WriteLog.Write(string.Format(errorMessage, "计算数据失败"));
                    return false;
                }
                #endregion

                #region DB持久化
                //28数据
                switch (dbOperateType)
                {
                    case DBOperateType.Insert:
                        result = Insert28Data(Source.Beijing, sourceDataList28);
                        break;
                    case DBOperateType.Update:
                        result = Update28Data(Source.Beijing, sourceDataList28);
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format(errorMessage, "异常，异常信息：" + ex.ToString()));
            }

            return result;
        }
        /// <summary>
        /// 采集并计算加拿大数据
        /// </summary>
        /// <param name="periodNum">期号</param>
        /// <param name="retTime">开奖时间(北京时间)</param>
        /// <returns></returns>
        public void CollectAndCalculateCanadan(long periodNum, DateTime retTime)
        {
            bool result = true;

            string errorMessage = "{0}，方法：CollectAndCalculateCanadan，期号：" + periodNum.ToString();

            try
            {
                #region 采集数据
                List<CollectResultEntity> collectResult = CollectCanadanData.Instance().Collect(periodNum, retTime);
                if (collectResult == null || collectResult.Count == 0)
                {
                    bool bWriteError = Arithmetic.Instance().IsWriteErrorMessage();
                    if (bWriteError)
                    {
                        WriteLog.Write(string.Format(errorMessage, "采集数据失败"));
                    }
                    return;
                }
                #endregion

                if (collectResult != null && collectResult.Count > 0)
                {
                    foreach (CollectResultEntity item in collectResult)
                    {
                        #region 计算数据
                        //28数据
                        List<SourceDataEntity> sourceDataList28 = CalculateCanada28Data(item);
                        if (sourceDataList28 == null || sourceDataList28.Count == 0)
                        {
                            WriteLog.Write(string.Format(errorMessage, "计算数据失败"));
                        }
                        #endregion

                        #region DB持久化
                        //28数据
                        result = Insert28Data(Source.Canadan, sourceDataList28);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format(errorMessage, "异常，异常信息：" + ex.ToString()));
            }
        }
        /// <summary>
        /// 加拿大数据采集是否写错误 北京时间19点04到21点不写错误，因为这个时间点加拿大可能不开奖
        /// </summary>
        /// <returns></returns>
        private bool IsWriteErrorMessage()
        {
            bool bWriteError = true;
            int proofreadTime = int.Parse(GetConfig.GetXMLValue("ProofreadTime"));
            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Canadan).AddSeconds(proofreadTime);
            if ((dtNow.Hour == 19 && dtNow.Minute > 0)
                || dtNow.Hour == 20
                || (dtNow.Hour == 21 && dtNow.Minute < 4))
            {
                bWriteError = false;
            }
            return bWriteError;
        }
        /// <summary>
        /// 读取下期期号
        /// </summary>
        /// <param name="source">采集计算源</param>
        /// <returns></returns>
        public CollectResultEntity GetNextPeriodNum(Source source)
        {
            CollectResultEntity result = new CollectResultEntity();

            switch (source)
            {
                case Source.Beijing:
                    result = SourceDataDA.Instance().GetBeijingNextPeriodNum();
                    break;
                case Source.Canadan:
                    result = SourceDataDA.Instance().GetCanadanNextPeriodNum();
                    break;
                default:
                    result.PeriodNum = -1;
                    break;
            }

            //如果期号为0，则需要从网站采集
            if (result.PeriodNum == 0)
            {
                switch (source)
                {
                    case Source.Beijing:
                        result = CollectBeijingData.Instance().CollectPeriodNum();
                        break;
                    case Source.Canadan:
                        result = CollectCanadanData.Instance().CollectPeriodNum();
                        break;
                    default:
                        result.PeriodNum = -1;
                        break;
                }
            }

            return result;
        }
        /// <summary>
        /// 读取失败的期
        /// </summary>
        /// <param name="source">采集计算源</param>
        /// <returns></returns>
        public List<CollectResultEntity> GetFailPeriodList(Source source)
        {
            switch (source)
            {
                case Source.Beijing:
                    return SourceDataDA.Instance().GetBeijingFailPeriodList();
                case Source.Canadan:
                    return SourceDataDA.Instance().GetCanadanFailPeriodList();
                default:
                    return null;
            }
        }
        /// <summary>
        /// 失败时写入失败的数据
        /// </summary>
        /// <param name="source">采集计算源</param>
        /// <param name="periodNum">期号</param>
        /// <param name="retTime">开奖时间</param>
        public void FailInsertData(Source source, long periodNum, DateTime retTime)
        {
            List<SourceDataEntity> dataList = new List<SourceDataEntity>();
            SourceDataEntity item = new SourceDataEntity();

            #region 龙虎网站数据
            item.SiteSysNo = (int)Site.LongHu;
            item.PeriodNum = periodNum;
            item.RetTime = retTime;
            dataList.Add(item);
            #endregion

            #region 计算71豆网站数据
            item = new SourceDataEntity();
            item.SiteSysNo = (int)Site.QiYiDou;
            item.PeriodNum = periodNum;
            item.RetTime = retTime;
            dataList.Add(item);
            #endregion

            #region 计算芝麻西西网站数据
            item = new SourceDataEntity();
            item.SiteSysNo = (int)Site.ZhiMaXiXi;
            item.PeriodNum = periodNum;
            item.RetTime = retTime;
            dataList.Add(item);
            #endregion

            Insert28Data(source, dataList);
        }
        #region 28
        /// <summary>
        /// 计算北京28数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        private List<SourceDataEntity> Calculate28Data(CollectResultEntity collectResult)
        {
            List<SourceDataEntity> result = null;

            ICalculate28Data calculateData = null;

            #region 计算龙虎网站数据
            calculateData = CalculateLongHu28Data.Instance();
            SourceDataEntity itemLongHu = calculateData.Calculate(collectResult);
            if (itemLongHu != null)
            {
                result = new List<SourceDataEntity>();
                result.Add(itemLongHu);
            }
            #endregion

            #region 计算71豆网站数据
            calculateData = CalculateQiYiDou28Data.Instance();
            SourceDataEntity itemQiYiDou = calculateData.Calculate(collectResult);
            if (itemQiYiDou != null)
            {
                if (result == null)
                    result = new List<SourceDataEntity>();
                result.Add(itemQiYiDou);
            }
            #endregion

            #region 计算芝麻西西网站数据
            calculateData = CalculateZhiMaXiXi28Data.Instance();
            SourceDataEntity itemZhiMaXiXi = calculateData.Calculate(collectResult);
            if (itemZhiMaXiXi != null)
            {
                if (result == null)
                    result = new List<SourceDataEntity>();
                result.Add(itemZhiMaXiXi);
            }
            #endregion

            return result;
        }
        /// <summary>
        /// 计算加拿大28数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        private List<SourceDataEntity> CalculateCanada28Data(CollectResultEntity collectResult)
        {
            List<SourceDataEntity> result = null;

            ICalculate28Data calculateData = null;

            #region 计算龙虎网站数据
            calculateData = CalculateLongHu28Data.Instance();
            SourceDataEntity itemLongHu = calculateData.CalculateCanada(collectResult);
            if (itemLongHu != null)
            {
                result = new List<SourceDataEntity>();
                result.Add(itemLongHu);
            }
            #endregion

            #region 计算71豆网站数据
            calculateData = CalculateQiYiDou28Data.Instance();
            SourceDataEntity itemQiYiDou = calculateData.CalculateCanada(collectResult);
            if (itemQiYiDou != null)
            {
                if (result == null)
                    result = new List<SourceDataEntity>();
                result.Add(itemQiYiDou);
            }
            #endregion

            #region 计算芝麻西西网站数据
            calculateData = CalculateZhiMaXiXi28Data.Instance();
            SourceDataEntity itemZhiMaXiXi = calculateData.CalculateCanada(collectResult);
            if (itemZhiMaXiXi != null)
            {
                if (result == null)
                    result = new List<SourceDataEntity>();
                result.Add(itemZhiMaXiXi);
            }
            #endregion

            return result;
        }
        /// <summary>
        /// 28数据持久化写入
        /// </summary>
        /// <param name="source">采集计算源</param>
        /// <param name="dataList">写入数据实体</param>
        /// <returns></returns>
        private bool Insert28Data(Source source, List<SourceDataEntity> dataList)
        {
            bool result = false;
            switch (source)
            {
                case Source.Beijing:
                    result = SourceDataDA.Instance().InsertSourceDataToBeijing28(dataList);
                    break;
                case Source.Canadan:
                    result = SourceDataDA.Instance().InsertSourceDataToCanadan28(dataList);
                    break;
                default:
                    result = false;
                    break;
            }

            //数据写入成功，则刷新遗漏期数
            if (result)
            {
                foreach (SourceDataEntity item in dataList)
                {
                    if (item.Status == 1)
                        SourceDataDA.Instance().RefreshOmitStatistics((int)Game.ErBa, (int)source, item.SiteSysNo);
                }
            }
            
            return result;
        }
        /// <summary>
        /// 28数据持久化更新
        /// </summary>
        /// <param name="source">采集计算源</param>
        /// <param name="dataList">写入数据实体</param>
        /// <returns></returns>
        private bool Update28Data(Source source, List<SourceDataEntity> dataList)
        {
            switch (source)
            {
                case Source.Beijing:
                    return SourceDataDA.Instance().UpdateSourceDataToBeijing28(dataList);
                case Source.Canadan:
                    return SourceDataDA.Instance().UpdateSourceDataToCanadan28(dataList);
                default:
                    return false;
            }
        }
        #endregion
    }
}
