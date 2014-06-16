using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Analysis28.DataService.Entity;
using Analysis28.DataService.Utility;

namespace Analysis28.DataService.DataAccess
{
    public class SourceDataDA
    {
        #region 单例模式

        private SourceDataDA()
        { }
        private static SourceDataDA _Instance;
        public static SourceDataDA Instance()
        {
            if (_Instance == null)
                _Instance = new SourceDataDA();
            return _Instance;
        }

        #endregion

        /// <summary>
        /// 读取北京下期期号
        /// </summary>
        /// <returns></returns>
        public CollectResultEntity GetBeijingNextPeriodNum()
        {
            CollectResultEntity result = new CollectResultEntity();

            string sql = @"SELECT TOP 1 [PeriodNum],[RetTime] FROM [Helpmate].[dbo].[SourceData_28_Beijing] ORDER BY [PeriodNum] DESC";
            DBHelper db = new DBHelper();
            try
            {
                DataTable dt = db.ExeSqlDataAdapter(CommandType.Text, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.PeriodNum = long.Parse(dt.Rows[0]["PeriodNum"].ToString()) + 1;
                    result.RetTime = DateTime.Parse(dt.Rows[0]["RetTime"].ToString());
                    //到23:55当天开奖就结束了，下一期在第二天9:05
                    if (result.RetTime.Hour == 23 && result.RetTime.Minute == 55)
                        result.RetTime = DateTime.Parse(string.Format("{0} 9:05", result.RetTime.ToShortDateString())).AddDays(1);
                    else
                        result.RetTime = result.RetTime.AddMinutes(5);
                }
                else
                {
                    result.PeriodNum = 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("GetBeijingNextPeriodNum读取SQL Server数据库期号失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }
        /// <summary>
        /// 读取加拿大下期期号
        /// </summary>
        /// <returns></returns>
        public CollectResultEntity GetCanadanNextPeriodNum()
        {
            CollectResultEntity result = new CollectResultEntity();

            string sql = @"SELECT TOP 1 [PeriodNum],[RetTime] FROM [Helpmate].[dbo].[SourceData_28_Canada] ORDER BY [PeriodNum] DESC";
            DBHelper db = new DBHelper();
            try
            {
                DataTable dt = db.ExeSqlDataAdapter(CommandType.Text, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.PeriodNum = long.Parse(dt.Rows[0]["PeriodNum"].ToString()) + 1;
                    result.RetTime = DateTime.Parse(dt.Rows[0]["RetTime"].ToString());
                    result.RetTime = result.RetTime.AddMinutes(4);
                }
                else
                {
                    result.PeriodNum = 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("GetCanadanNextPeriodNum读取SQL Server数据库期号失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 读取北京失败期列表
        /// </summary>
        /// <returns></returns>
        public List<CollectResultEntity> GetBeijingFailPeriodList()
        {
            List<CollectResultEntity> result = null;

            string sql = @"SELECT [PeriodNum],[RetTime] FROM [Helpmate].[dbo].[SourceData_28_Beijing] WHERE [Status] = -1";
            DBHelper db = new DBHelper();
            try
            {
                DataTable dt = db.ExeSqlDataAdapter(CommandType.Text, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<CollectResultEntity>();
                    foreach (DataRow row in dt.Rows)
                    {
                        CollectResultEntity item = new CollectResultEntity();
                        item.PeriodNum = long.Parse(row["PeriodNum"].ToString());
                        item.RetTime = DateTime.Parse(row["RetTime"].ToString());
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("GetBeijingFailPeriodList读取SQL Server数据库失败期列表失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }
        /// <summary>
        /// 读取加拿大失败期列表
        /// </summary>
        /// <returns></returns>
        public List<CollectResultEntity> GetCanadanFailPeriodList()
        {
            List<CollectResultEntity> result = null;

            string sql = @"SELECT [PeriodNum],[RetTime] FROM [Helpmate].[dbo].[SourceData_28_Canada] WHERE [Status] = -1";
            DBHelper db = new DBHelper();
            try
            {
                DataTable dt = db.ExeSqlDataAdapter(CommandType.Text, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<CollectResultEntity>();
                    foreach (DataRow row in dt.Rows)
                    {
                        CollectResultEntity item = new CollectResultEntity();
                        item.PeriodNum = long.Parse(row["PeriodNum"].ToString());
                        item.RetTime = DateTime.Parse(row["RetTime"].ToString());
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("GetCanadanFailPeriodList读取SQL Server数据库失败期列表失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 写入北京28数据
        /// </summary>
        /// <param name="dataList">写入数据实体</param>
        /// <returns></returns>
        public bool InsertSourceDataToBeijing28(List<SourceDataEntity> dataList)
        {
            bool result = false;

            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Beijing);
            string sql = string.Empty;
            string sqlFormat = @"INSERT INTO [Helpmate].[dbo].[SourceData_28_Beijing]
([PeriodNum],[RetTime],[SiteSysNo],[RetOddNum],[RetNum],[RetMidNum],[CollectRet],[CollectTime],[Status])
VALUES({0},'{1}',{2},{3},{4},'{5}','{6}','{7}',{8});";
            foreach (SourceDataEntity item in dataList)
            {
                sql += string.Format(sqlFormat, item.PeriodNum, item.RetTime, item.SiteSysNo, item.RetOddNum,
                    item.RetNum, item.RetMidNum, item.CollectRet, dtNow.ToString(), item.Status);
            }
            DBHelper db = new DBHelper();
            try
            {
                int retVal = db.ExecuteNonQuery(CommandType.Text, sql);
                result = retVal > 0 ? true : false;
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("InsertSourceDataToBeijing28方法写SQL Server数据库失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }
        /// <summary>
        /// 写入加拿大28数据
        /// </summary>
        /// <param name="dataList">写入数据实体</param>
        /// <returns></returns>
        public bool InsertSourceDataToCanadan28(List<SourceDataEntity> dataList)
        {
            bool result = false;

            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Canadan);
            string sql = string.Empty;
            string sqlFormat = @"INSERT INTO [Helpmate].[dbo].[SourceData_28_Canada]
([PeriodNum],[RetTime],[SiteSysNo],[RetOddNum],[RetNum],[RetMidNum],[CollectRet],[CollectTime],[Status])
VALUES({0},'{1}',{2},{3},{4},'{5}','{6}','{7}',{8});";
            foreach (SourceDataEntity item in dataList)
            {
                if (item.SiteSysNo == 10002 || item.SiteSysNo == 10003)
                    item.RetTime = item.RetTime.AddMinutes(1);
                sql += string.Format(sqlFormat, item.PeriodNum, item.RetTime, item.SiteSysNo, item.RetOddNum,
                    item.RetNum, item.RetMidNum, item.CollectRet, dtNow.ToString(), item.Status);
            }
            DBHelper db = new DBHelper();
            try
            {
                int retVal = db.ExecuteNonQuery(CommandType.Text, sql);
                result = retVal > 0 ? true : false;
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("InsertSourceDataToCanadan28方法写SQL Server数据库失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 更新北京28数据
        /// </summary>
        /// <param name="dataList">更新数据实体</param>
        /// <returns></returns>
        public bool UpdateSourceDataToBeijing28(List<SourceDataEntity> dataList)
        {
            bool result = false;

            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Beijing);
            string sql = string.Empty;
            string sqlFormat = @"UPDATE [Helpmate].[dbo].[SourceData_28_Beijing] SET [RetOddNum] = {0},
[RetNum] = {1}, [RetMidNum] = '{2}', [CollectRet] = '{3}', [CollectTime] = '{4}',
[Status] = 1 WHERE [PeriodNum] = {5} AND [SiteSysNo] = {6};";
            foreach (SourceDataEntity item in dataList)
            {
                sql += string.Format(sqlFormat, item.RetOddNum, item.RetNum, item.RetMidNum, item.CollectRet,
                    dtNow.ToString(), item.PeriodNum, item.SiteSysNo);
            }
            DBHelper db = new DBHelper();
            try
            {
                int retVal = db.ExecuteNonQuery(CommandType.Text, sql);
                result = retVal > 0 ? true : false;
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("UpdateSourceDataToBeijing28方法更新SQL Server数据库失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }
        /// <summary>
        /// 更新加拿大28数据
        /// </summary>
        /// <param name="dataList">更新数据实体</param>
        /// <returns></returns>
        public bool UpdateSourceDataToCanadan28(List<SourceDataEntity> dataList)
        {
            bool result = false;

            DateTime dtNow = (new GetTime()).NowTime(ConfigSource.Canadan);
            string sql = string.Empty;
            string sqlFormat = @"UPDATE [Helpmate].[dbo].[SourceData_28_Canada] SET [RetOddNum] = {0},
[RetNum] = {1}, [RetMidNum] = '{2}', [CollectRet] = '{3}', [CollectTime] = '{4}',
[Status] = 1 WHERE [PeriodNum] = {5} AND [SiteSysNo] = {6};";
            foreach (SourceDataEntity item in dataList)
            {
                sql += string.Format(sqlFormat, item.RetOddNum, item.RetNum, item.RetMidNum, item.CollectRet,
                    dtNow.ToString(), item.PeriodNum, item.SiteSysNo);
            }
            DBHelper db = new DBHelper();
            try
            {
                int retVal = db.ExecuteNonQuery(CommandType.Text, sql);
                result = retVal > 0 ? true : false;
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("UpdateSourceDataToCanadan28方法更新SQL Server数据库失败，sql：{0}，错误信息：{1}", sql, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 刷新遗漏期数
        /// </summary>
        /// <param name="gameSysNo">游戏编号</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="siteSysNo">网站编号</param>
        /// <returns></returns>
        public bool RefreshOmitStatistics(int gameSysNo,int sourceSysNo, int siteSysNo)
        {
            bool result = false;

            #region SQL
            string sql = @"DECLARE @NowPeriodNum INT, @MinPeriodNum INT, @Idx INT;
	SELECT @NowPeriodNum = 0, @MinPeriodNum = 0, @Idx = 0;
	IF(@GameSysNo = 10001)
	BEGIN
		IF(@SourceSysNo = 10001)
		BEGIN
			SELECT @NowPeriodNum = ISNULL(MAX(PeriodNum), 0), @MinPeriodNum = ISNULL(MIN(PeriodNum), 0)
				FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK)
				WHERE SiteSysNo = @SiteSysNo AND [Status] = 1
		END
		ELSE IF(@SourceSysNo = 10002)
		BEGIN
			SELECT @NowPeriodNum = ISNULL(MAX(PeriodNum), 0), @MinPeriodNum = ISNULL(MIN(PeriodNum), 0)
				FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK)
				WHERE SiteSysNo = @SiteSysNo AND [Status] = 1
		END
	END

	--当前最大期未刷新，则刷新；已刷新过则不予刷新
	IF(NOT EXISTS(SELECT 1 FROM Helpmate.dbo.OmitStatistics(NOLOCK) WHERE GameSysNo = @GameSysNo
		AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo AND NowPeriodNum = @NowPeriodNum))
	BEGIN
		DECLARE @TblList TABLE(TempRetNum INT, TempOmitCnt INT);
		IF(@GameSysNo = 10001)
		BEGIN
			IF(@SourceSysNo = 10001)
			BEGIN
				INSERT INTO @TblList
				SELECT RetNum, (@NowPeriodNum - ISNULL(MAX(PeriodNum), @MinPeriodNum)) AS OmitCnt
					FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK)
					WHERE SiteSysNo = @SiteSysNo AND [Status] = 1
					GROUP BY SiteSysNo, RetNum
					ORDER BY RetNum ASC
			END
			ELSE IF(@SourceSysNo = 10002)
			BEGIN
				INSERT INTO @TblList
				SELECT RetNum, (@NowPeriodNum - ISNULL(MAX(PeriodNum), @MinPeriodNum)) AS OmitCnt
					FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK)
					WHERE SiteSysNo = @SiteSysNo AND [Status] = 1
					GROUP BY SiteSysNo, RetNum
					ORDER BY RetNum ASC
			END
		END
		--最大遗漏期数
		UPDATE Helpmate.dbo.OmitStatistics SET MaxOmitCnt = OmitCnt
		WHERE RetNum IN
		(SELECT TempRetNum FROM @TblList WHERE TempOmitCnt = 0)
		AND GameSysNo = @GameSysNo AND SourceSysNo = @SourceSysNo
		AND SiteSysNo = @SiteSysNo AND MaxOmitCnt < OmitCnt
		--遗漏期数
		UPDATE Helpmate.dbo.OmitStatistics SET OmitCnt = TempOmitCnt, NowPeriodNum = @NowPeriodNum
			FROM @TblList AS Temp
			WHERE RetNum = Temp.TempRetNum AND GameSysNo = @GameSysNo
			AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo
		--未出现的
		UPDATE Helpmate.dbo.OmitStatistics SET OmitCnt = @NowPeriodNum - @MinPeriodNum,
			MaxOmitCnt = @NowPeriodNum - @MinPeriodNum, NowPeriodNum = @NowPeriodNum
			WHERE NowPeriodNum = 0 AND GameSysNo = @GameSysNo AND SourceSysNo = @SourceSysNo
			AND SiteSysNo = @SiteSysNo
			
	END";
            #endregion

            DBHelper db = new DBHelper();
            SqlParameter[] para = new SqlParameter[]{ 
                new SqlParameter("@GameSysNo", gameSysNo),
                new SqlParameter("@SourceSysNo", sourceSysNo),
                new SqlParameter("@SiteSysNo", siteSysNo)
            };
            try
            {
                int retVal = db.ExecuteNonQuery(CommandType.Text, sql, para);
                if (retVal > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                WriteLog.Write(string.Format("RefreshOmitStatistics方法刷新遗漏期数失败，sql：{0},GameSysNo={1},SourceSysNo={2},SiteSysNo={3}，错误信息：{4}", sql, gameSysNo, sourceSysNo, siteSysNo, ex.ToString()));
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }
    }
}
