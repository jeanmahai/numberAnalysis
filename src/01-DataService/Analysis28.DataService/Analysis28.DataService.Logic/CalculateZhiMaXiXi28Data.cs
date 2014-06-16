using Analysis28.DataService.Entity;

namespace Analysis28.DataService.Logic
{
    /// <summary>
    /// 计算芝麻西西网站数据实现类
    /// </summary>
    public class CalculateZhiMaXiXi28Data : ICalculate28Data
    {
        #region 单例模式

        private CalculateZhiMaXiXi28Data()
        { }
        private static CalculateZhiMaXiXi28Data _Instance;
        public static CalculateZhiMaXiXi28Data Instance()
        {
            if (_Instance == null)
                _Instance = new CalculateZhiMaXiXi28Data();
            return _Instance;
        }

        #endregion

        #region ICalculateData 成员

        /// <summary>
        /// 计算北京芝麻西西网站数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        public SourceDataEntity Calculate(CollectResultEntity collectResult)
        {
            SourceDataEntity result = new SourceDataEntity();

            result.PeriodNum = collectResult.PeriodNum;
            result.RetTime = collectResult.RetTime;
            result.SiteSysNo = (int)Site.ZhiMaXiXi;
            result.CollectRet = collectResult.Result;
            result.Status = 1;

            #region retMidNum
            int retMidNum1 = 0;
            int retMidNum2 = 0;
            int retMidNum3 = 0;
            //取2 5 8 11 14 17位数字之和
            int i = 1;
            while (i < 17)
            {
                retMidNum1 += collectResult.Group[i];
                i += 3;
            }
            //取3 6 9 12 15 18位数字之和
            int j = 2;
            while (j < 18)
            {
                retMidNum2 += collectResult.Group[j];
                j += 3;
            }
            //取4 7 10 13 16 19位数字之和
            int k = 3;
            while (k < 19)
            {
                retMidNum3 += collectResult.Group[k];
                k += 3;
            }
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
        /// 计算加拿大芝麻西西网站数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        public SourceDataEntity CalculateCanada(CollectResultEntity collectResult)
        {
            SourceDataEntity result = new SourceDataEntity();

            result.PeriodNum = collectResult.PeriodNum;
            result.RetTime = collectResult.RetTime;
            result.SiteSysNo = (int)Site.ZhiMaXiXi;
            result.CollectRet = collectResult.Result;
            result.Status = 1;

            #region retMidNum
            int retMidNum1 = 0;
            int retMidNum2 = 0;
            int retMidNum3 = 0;
            //取2 5 8 11 14 17位数字之和
            int i = 1;
            while (i < 17)
            {
                retMidNum1 += collectResult.Group[i];
                i += 3;
            }
            //取3 6 9 12 15 18位数字之和
            int j = 2;
            while (j < 18)
            {
                retMidNum2 += collectResult.Group[j];
                j += 3;
            }
            //取4 7 10 13 16 19位数字之和
            int k = 3;
            while (k < 19)
            {
                retMidNum3 += collectResult.Group[k];
                k += 3;
            }
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
