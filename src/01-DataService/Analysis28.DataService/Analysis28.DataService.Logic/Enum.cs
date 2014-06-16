namespace Analysis28.DataService.Logic
{
    /// <summary>
    /// 采集数据源
    /// </summary>
    public enum Source
    {
        /// <summary>
        /// 北京
        /// </summary>
        Beijing = 10001,
        /// <summary>
        /// 加拿大
        /// </summary>
        Canadan = 10002
    }
    /// <summary>
    /// 使用公式网站编号
    /// </summary>
    public enum Site
    {
        /// <summary>
        /// 龙虎
        /// </summary>
        LongHu = 10001,
        /// <summary>
        /// 71豆
        /// </summary>
        QiYiDou = 10002,
        /// <summary>
        /// 芝麻西西
        /// </summary>
        ZhiMaXiXi = 10003
    }
    /// <summary>
    /// DB操作类型
    /// </summary>
    public enum DBOperateType
    {
        /// <summary>
        /// 写
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 2
    }
    /// <summary>
    /// 游戏
    /// </summary>
    public enum Game
    {
        /// <summary>
        /// 28
        /// </summary>
        ErBa = 10001,
        /// <summary>
        /// PK10
        /// </summary>
        PK10 = 10002
    }
}
