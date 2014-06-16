using Analysis28.DataService.Entity;

namespace Analysis28.DataService.Logic
{
    /// <summary>
    /// 计算数据接口
    /// </summary>
    public interface ICalculate28Data
    {
        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        SourceDataEntity Calculate(CollectResultEntity collectResult);

        /// <summary>
        /// 计算加拿大数据
        /// </summary>
        /// <param name="collectResult">采集结果</param>
        /// <returns></returns>
        SourceDataEntity CalculateCanada(CollectResultEntity collectResult);
    }
}
