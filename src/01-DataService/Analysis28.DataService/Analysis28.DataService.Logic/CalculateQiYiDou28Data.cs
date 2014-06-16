using Analysis28.DataService.Entity;

namespace Analysis28.DataService.Logic
{
    /// <summary>
    /// 计算71豆网站数据实现类
    /// </summary>
    public class CalculateQiYiDou28Data : ICalculate28Data
    {
        #region 单例模式

        private CalculateQiYiDou28Data()
        { }
        private static CalculateQiYiDou28Data _Instance;
        public static CalculateQiYiDou28Data Instance()
        {
            if (_Instance == null)
                _Instance = new CalculateQiYiDou28Data();
            return _Instance;
        }

        #endregion

        #region ICalculateData 成员

        /// <summary>
        /// 计算北京71豆网站数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        public SourceDataEntity Calculate(CollectResultEntity collectResult)
        {
            SourceDataEntity result = new SourceDataEntity();

            result.PeriodNum = collectResult.PeriodNum;
            result.RetTime = collectResult.RetTime;
            result.SiteSysNo = (int)Site.QiYiDou;
            result.CollectRet = collectResult.Result;
            result.Status = 1;

            #region retMidNum
            int retMidNum1 = 0;
            int retMidNum2 = 0;
            int retMidNum3 = 0;
            //取1-6位数字之和
            for (int i = 0; i < 6; i++)
                retMidNum1 += collectResult.Group[i];
            //取7-12位数字之和
            for (int i = 6; i < 12; i++)
                retMidNum2 += collectResult.Group[i];
            //取13-18位数字之和
            for (int i = 12; i < 18; i++)
                retMidNum3 += collectResult.Group[i];
            #endregion

            #region RetNum
            int retNum = retMidNum1 % 10;
            retNum += retMidNum2 % 10;
            retNum += retMidNum3 % 10;
            #endregion

            #region RetOddNum
            int retOddNum = (retMidNum1 % 10) * 100 + (retMidNum2 % 10) * 10 + (retMidNum3 % 10);
            #endregion

            result.RetOddNum = retOddNum;
            result.RetNum = retNum;
            result.RetMidNum = string.Format("{0}|{1}|{2}", retMidNum1, retMidNum2, retMidNum3);

            return result;
        }

        /// <summary>
        /// 计算加拿大71豆网站数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        public SourceDataEntity CalculateCanada(CollectResultEntity collectResult)
        {
            SourceDataEntity result = new SourceDataEntity();

            result.PeriodNum = collectResult.PeriodNum;
            result.RetTime = collectResult.RetTime;
            result.SiteSysNo = (int)Site.QiYiDou;
            result.CollectRet = collectResult.Result;
            result.Status = 1;

            #region retMidNum
            int retMidNum1 = 0;
            int retMidNum2 = 0;
            int retMidNum3 = 0;
            //取3-8位数字之和
            for (int i = 2; i < 8; i++)
                retMidNum1 += collectResult.Group[i];
            //取9-14位数字之和
            for (int i = 8; i < 14; i++)
                retMidNum2 += collectResult.Group[i];
            //取15-20位数字之和
            for (int i = 14; i < 20; i++)
                retMidNum3 += collectResult.Group[i];
            #endregion

            #region RetNum
            int retNum = retMidNum1 % 10;
            retNum += retMidNum2 % 10;
            retNum += retMidNum3 % 10;
            #endregion

            #region RetOddNum
            int retOddNum = (retMidNum1 % 10) * 100 + (retMidNum2 % 10) * 10 + (retMidNum3 % 10);
            #endregion

            result.RetOddNum = retOddNum;
            result.RetNum = retNum;
            result.RetMidNum = string.Format("{0}|{1}|{2}", retMidNum1, retMidNum2, retMidNum3);

            return result;
        }

        #endregion
    }
}
