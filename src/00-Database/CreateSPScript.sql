USE [Analysis28DB]
GO
/****** Object:  StoredProcedure [dbo].[SpecialAnalysis]    Script Date: 06/15/2014 21:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpecialAnalysis]
	-- Add the parameters for the stored procedure here
	@SourceSysNo INT,
	@SiteSysNo INT,
	@BeginHour INT,
	@EndHour INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--结果列表
	DECLARE @TblResultList TABLE(Idx INT IDENTITY(1,1), Date NVARCHAR(20),
		Big INT, Small INT, Middle INT, Side INT, Odd INT, Dual INT);
	--未出现的号码
	DECLARE @TblNotList TABLE(Idx INT IDENTITY(1,1), Date NVARCHAR(20), RetNum INT);
	--极品号
	DECLARE @TblBestList TABLE(Idx INT IDENTITY(1,1), Date NVARCHAR(20), RetNum INT);	
	--大/小，中/边，单/双 总开奖次数表
	DECLARE @TblCntList TABLE(AvgBig INT, AvgSmall INT, AvgMiddle INT,
		AvgSide INT, AvgOdd INT, AvgDual INT);
	--最稳定的号码
	DECLARE @TblStableList TABLE(RetNum INT, Days INT, Cnt INT);
	INSERT INTO @TblStableList
	SELECT RetNum, 0, 0 FROM dbo.ResultCategory_28(NOLOCK)
	--输赢天数
	DECLARE @TblWinList TABLE(BigCnt INT, SmallCnt INT, MiddleCnt INT,
		SideCnt INT, OddCnt INT, DualCnt INT);
	INSERT INTO @TblWinList VALUES(0,0,0,0,0,0)

	DECLARE @NowHour INT, @BeginDate DATETIME, @EndDate DATETIME, @n INT, @WinCnt INT;

	--赢的值
	IF(@SourceSysNo = 10001)
	BEGIN
		IF(@BeginHour = 9 AND @EndHour = 12)
		BEGIN
			SET @WinCnt = 17;
		END
		ELSE IF(@BeginHour = 9)
		BEGIN
			SET @WinCnt = 5;
		END
		ELSE IF(@BeginHour = @EndHour)
		BEGIN
			SET @WinCnt = 6;
		END
		ELSE
		BEGIN
			SET @WinCnt = 18;
		END
	END
	ELSE IF(@SourceSysNo = 10002)
	BEGIN
		IF(@BeginHour = @EndHour)
		BEGIN
			SET @WinCnt = 7;
		END
		ELSE
		BEGIN
			SET @WinCnt = 22;
		END
	END

	IF(@BeginHour = @EndHour)
	BEGIN
		SET @EndHour = @EndHour + 1;
	END
	SELECT @n = 15,
		   @NowHour = DATEPART(HOUR, GETDATE()),
		   @BeginDate = CONVERT(DATETIME, CONVERT(NVARCHAR(12), GETDATE(), 111) + N' ' + CONVERT(NVARCHAR(2), @BeginHour) + N':00:00');
	IF(@EndHour = 24)
	BEGIN
		SET @EndDate = CONVERT(DATETIME, CONVERT(NVARCHAR(12), GETDATE(), 111) + N' ' + N'23:59:59');
	END
	ELSE
	BEGIN
		SET @EndDate = CONVERT(DATETIME, CONVERT(NVARCHAR(12), GETDATE(), 111) + N' ' + CONVERT(NVARCHAR(2), @EndHour) + N':00:00');
	END
	IF(@NowHour < @EndHour)
	BEGIN
		SELECT @BeginDate = DATEADD(DAY, -1, @BeginDate),
			   @EndDate = DATEADD(DAY, -1, @EndDate);
	END

	--临时结果表
	DECLARE @TblTempResult TABLE(RetNum INT, BigOrSmall NVARCHAR(2), MiddleOrSide NVARCHAR(2), OddOrDual NVARCHAR(2));
	DECLARE @Date NVARCHAR(20), @BigCnt INT, @SmallCnt INT, @MiddleCnt INT, @SideCnt INT,
		@OddCnt INT, @DualCnt INT, @Month INT, @Day INT;

	WHILE(@n > 0)
	BEGIN
		DELETE @TblTempResult;
		--北京
		IF(@SourceSysNo = 10001)
		BEGIN
			INSERT INTO @TblTempResult
			SELECT A.RetNum, B.BigOrSmall, B.MiddleOrSide, B.OddOrDual
				FROM dbo.SourceData_28_Beijing(NOLOCK) A
				LEFT JOIN dbo.ResultCategory_28(NOLOCK) B
					ON A.RetNum = B.RetNum
				WHERE A.RetTime >= @BeginDate AND A.RetTime < @EndDate AND A.SiteSysNo = @SiteSysNo
		END
		--加拿大
		ELSE IF(@SourceSysNo = 10002)
		BEGIN
			INSERT INTO @TblTempResult
			SELECT A.RetNum, B.BigOrSmall, B.MiddleOrSide, B.OddOrDual
				FROM dbo.SourceData_28_Canada(NOLOCK) A
				LEFT JOIN dbo.ResultCategory_28(NOLOCK) B
					ON A.RetNum = B.RetNum
				WHERE A.RetTime >= @BeginDate AND A.RetTime < @EndDate AND A.SiteSysNo = @SiteSysNo
		END
		--计算
		SELECT @BigCnt = COUNT(1) FROM @TblTempResult WHERE BigOrSmall = N'大'
		SELECT @SmallCnt = COUNT(1) FROM @TblTempResult WHERE BigOrSmall = N'小'
		SELECT @MiddleCnt = COUNT(1) FROM @TblTempResult WHERE MiddleOrSide = N'中'
		SELECT @SideCnt = COUNT(1) FROM @TblTempResult WHERE MiddleOrSide = N'边'
		SELECT @OddCnt = COUNT(1) FROM @TblTempResult WHERE OddOrDual = N'单'
		SELECT @DualCnt = COUNT(1) FROM @TblTempResult WHERE OddOrDual = N'双'
		SELECT @Month = DATEPART(MONTH, @BeginDate), @Day = DATEPART(DAY, @BeginDate);
		--输赢天数
		IF(@BigCnt > @WinCnt)
		BEGIN
			UPDATE @TblWinList SET BigCnt = BigCnt + 1
		END
		ELSE IF(@SmallCnt > @WinCnt)
		BEGIN
			UPDATE @TblWinList SET SmallCnt = SmallCnt + 1
		END
		IF(@MiddleCnt > @WinCnt)
		BEGIN
			UPDATE @TblWinList SET MiddleCnt = MiddleCnt + 1
		END
		ELSE IF(@SideCnt > @WinCnt)
		BEGIN
			UPDATE @TblWinList SET SideCnt = SideCnt + 1
		END
		IF(@OddCnt > @WinCnt)
		BEGIN
			UPDATE @TblWinList SET OddCnt = OddCnt + 1
		END
		ELSE IF(@DualCnt > @WinCnt)
		BEGIN
			UPDATE @TblWinList SET DualCnt = DualCnt + 1
		END

		SET @Date = CONVERT(NVARCHAR(2), @Month) + N'月' + CONVERT(NVARCHAR(2), @Day) + N'日';
		IF(@BeginHour + 1 = @EndHour)
		BEGIN
			SET @Date = @Date + CONVERT(NVARCHAR(2), @BeginHour) + N'时';
		END
		ELSE
		BEGIN
			SET @Date = @Date + CONVERT(NVARCHAR(2), @BeginHour) + N'-'
			SET @Date = @Date + CONVERT(NVARCHAR(2), @EndHour) + N'时';
		END
		--结果
		INSERT INTO @TblResultList VALUES(@Date, @BigCnt, @SmallCnt, @MiddleCnt, @SideCnt, @OddCnt, @DualCnt)
		--未出现的号码
		INSERT INTO @TblNotList
		SELECT @Date, RetNum FROM dbo.ResultCategory_28(NOLOCK)
			WHERE RetNum NOT IN (SELECT RetNum FROM @TblTempResult)
			AND RetNum NOT IN (0,1,2,3,25,26,27)
		--极品号
		INSERT INTO @TblBestList
		SELECT @Date, RetNum FROM @TblTempResult
			WHERE RetNum IN (0,1,2,3,25,26,27)
		--出现天数次数
		UPDATE @TblStableList SET Days = Days + 1, Cnt = Cnt + T.TempCnt FROM
		(SELECT RetNum AS TempRetNum, COUNT(1) AS TempCnt FROM @TblTempResult GROUP BY RetNum) AS T
		WHERE RetNum = T.TempRetNum

		SELECT @BeginDate = DATEADD(DAY, -1, @BeginDate), @EndDate = DATEADD(DAY, -1, @EndDate);
		SET @n = @n - 1;
	END
	SELECT * FROM @TblResultList ORDER BY Idx ASC
	SELECT * FROM @TblNotList ORDER BY Idx ASC
	SELECT * FROM @TblBestList ORDER BY Idx ASC
	SELECT ISNULL(SUM(Big), 0) AS AvgBig,
		   ISNULL(SUM(Small), 0) AS AvgSmall,
		   ISNULL(SUM(Middle), 0) AS AvgMiddle,
		   ISNULL(SUM(Side), 0) AS AvgSide,
		   ISNULL(SUM(Odd), 0) AS AvgOdd,
		   ISNULL(SUM(Dual), 0) AS AvgDual
		FROM @TblResultList
	SELECT TOP 3 * FROM @TblStableList ORDER BY Days DESC, Cnt DESC
	SELECT * FROM @TblWinList

END
GO
/****** Object:  StoredProcedure [dbo].[CreatePayCard]    Script Date: 06/15/2014 21:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick
-- Create date: 2013/09/22
-- Description:	生成充值卡
-- =============================================
CREATE PROCEDURE [dbo].[CreatePayCard]
	-- Add the parameters for the stored procedure here
	@DataXML NVARCHAR(max),
	@CategorySysNo INT,
	@BeginTime DATETIME,
	@EndTime DATETIME

AS
BEGIN

	DECLARE @Result INT;

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Result = 0;

    -- Insert statements for procedure here

	DECLARE @docHandle INT;
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @DataXML
	INSERT INTO [Helpmate].[dbo].[PayCard]([PayCardID],[PayCardPwd],[CategorySysNo],[Status],[InDate],[BeginTime],[EndTime])
		SELECT [ID],[Pwd],@CategorySysNo,1,GETDATE(),@BeginTime,@EndTime
		FROM OPENXML(@docHandle, N'/root/row')
		WITH ([ID] NVARCHAR(32) './@ID', [Pwd] NVARCHAR(32) './@Pwd')
	EXEC sp_xml_removedocument @docHandle

	SET @Result = 1;

	RETURN @Result;

END
GO
/****** Object:  StoredProcedure [dbo].[RefreshRemind]    Script Date: 06/15/2014 21:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RefreshRemind] 
	-- Add the parameters for the stored procedure here
	@GameSysNo INT,
	@SourceSysNo INT,
	@SiteSysNo INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	DECLARE @NowPeriodNum INT, @NowRetNum INT, @Cnt INT;
	DECLARE @NowRetNumBig NVARCHAR(10), @NowRetNumMiddle NVARCHAR(10), @NowRetNumOdd NVARCHAR(10);

	SELECT @NowPeriodNum = 0, @NowRetNum = -1, @Cnt = 0,
			@NowRetNumBig = N'', @NowRetNumMiddle = N'', @NowRetNumOdd = N'';
	
	--开始刷新
	IF(@GameSysNo = 10001)
	BEGIN
		IF(@SourceSysNo = 10001)
		BEGIN
			--当前期
			SELECT @NowPeriodNum = ISNULL(MAX(PeriodNum), 0)
				FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK)
				WHERE SiteSysNo = @SiteSysNo AND [Status] = 1
			--当前期开奖结果
			SELECT @NowRetNum = A.RetNum,
				@NowRetNumBig = B.BigOrSmall,
				@NowRetNumMiddle = B.MiddleOrSide,
				@NowRetNumOdd = B.OddOrDual
				FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK) A
				LEFT JOIN dbo.ResultCategory_28(NOLOCK) B
				ON A.RetNum = B.RetNum
				WHERE A.SiteSysNo = @SiteSysNo and A.PeriodNum = @NowPeriodNum
			--当前最大期未刷新，则刷新；已刷新过则不予刷新
			IF(NOT EXISTS(SELECT 1 FROM Helpmate.dbo.RemindRefreshTag(NOLOCK) WHERE GameSysNo = @GameSysNo
				AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo AND NowPeriodNum = @NowPeriodNum))
			BEGIN
				--都置为不提醒
				UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 0
					WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo

				--大/小
				SELECT @Cnt = COUNT(1) FROM
					(SELECT @NowPeriodNum - (ROW_NUMBER() OVER (ORDER BY PeriodNum DESC)) AS TempPeriodNum, PeriodNum
					FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK)
					WHERE RetNum IN (SELECT RetNum FROM Helpmate.dbo.ResultCategory_28(NOLOCK) WHERE BigOrSmall = @NowRetNumBig)
					AND PeriodNum < @NowPeriodNum AND SiteSysNo = @SiteSysNo) AS T
					WHERE T.TempPeriodNum = T.PeriodNum
				IF(@Cnt > 0)
				BEGIN
					--将订阅该开奖结果连续次数的记录更新为提醒状态,1.提醒，0.不提醒（已提醒过也是0）
					UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 1
						WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo
						AND SiteSysNo = @SiteSysNo AND Cnt = @Cnt + 1
						AND RetNum = @NowRetNumBig
				END
				--中/边
				SELECT @Cnt = COUNT(1) FROM
					(SELECT @NowPeriodNum - (ROW_NUMBER() OVER (ORDER BY PeriodNum DESC)) AS TempPeriodNum, PeriodNum
					FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK)
					WHERE RetNum IN (SELECT RetNum FROM Helpmate.dbo.ResultCategory_28(NOLOCK) WHERE MiddleOrSide = @NowRetNumMiddle)
					AND PeriodNum < @NowPeriodNum AND SiteSysNo = @SiteSysNo) AS T
					WHERE T.TempPeriodNum = T.PeriodNum
				IF(@Cnt > 0)
				BEGIN
					--将订阅该开奖结果连续次数的记录更新为提醒状态,1.提醒，0.不提醒（已提醒过也是0）
					UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 1
						WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo
						AND SiteSysNo = @SiteSysNo AND Cnt = @Cnt + 1
						AND RetNum = @NowRetNumMiddle
				END
				--单/双
				SELECT @Cnt = COUNT(1) FROM
					(SELECT @NowPeriodNum - (ROW_NUMBER() OVER (ORDER BY PeriodNum DESC)) AS TempPeriodNum, PeriodNum
					FROM Helpmate.dbo.SourceData_28_Beijing(NOLOCK)
					WHERE RetNum IN (SELECT RetNum FROM Helpmate.dbo.ResultCategory_28(NOLOCK) WHERE OddOrDual = @NowRetNumOdd)
					AND PeriodNum < @NowPeriodNum AND SiteSysNo = @SiteSysNo) AS T
					WHERE T.TempPeriodNum = T.PeriodNum
				IF(@Cnt > 0)
				BEGIN
					--将订阅该开奖结果连续次数的记录更新为提醒状态,1.提醒，0.不提醒（已提醒过也是0）
					UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 1
						WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo
						AND SiteSysNo = @SiteSysNo AND Cnt = @Cnt + 1
						AND RetNum = @NowRetNumOdd
				END
				--更新当前期已刷新
				UPDATE [Helpmate].[dbo].[RemindRefreshTag] SET NowPeriodNum = @NowPeriodNum
					WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo
			END
		END
		ELSE IF(@SourceSysNo = 10002)
		BEGIN
			--当前期
			SELECT @NowPeriodNum = ISNULL(MAX(PeriodNum), 0)
				FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK)
				WHERE SiteSysNo = @SiteSysNo AND [Status] = 1
			--当前期开奖结果
			SELECT @NowRetNum = A.RetNum,
				@NowRetNumBig = B.BigOrSmall,
				@NowRetNumMiddle = B.MiddleOrSide,
				@NowRetNumOdd = B.OddOrDual
				FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK) A
				LEFT JOIN dbo.ResultCategory_28(NOLOCK) B
				ON A.RetNum = B.RetNum
				WHERE A.SiteSysNo = @SiteSysNo and A.PeriodNum = @NowPeriodNum
			--当前最大期未刷新，则刷新；已刷新过则不予刷新
			IF(NOT EXISTS(SELECT 1 FROM Helpmate.dbo.RemindRefreshTag(NOLOCK) WHERE GameSysNo = @GameSysNo
				AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo AND NowPeriodNum = @NowPeriodNum))
			BEGIN
				--都置为不提醒
				UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 0
					WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo
				--大/小
				SELECT @Cnt = COUNT(1) FROM
					(SELECT @NowPeriodNum - (ROW_NUMBER() OVER (ORDER BY PeriodNum DESC)) AS TempPeriodNum, PeriodNum
					FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK)
					WHERE RetNum IN (SELECT RetNum FROM Helpmate.dbo.ResultCategory_28(NOLOCK) WHERE BigOrSmall = @NowRetNumBig)
					AND PeriodNum < @NowPeriodNum AND SiteSysNo = @SiteSysNo) AS T
					WHERE T.TempPeriodNum = T.PeriodNum
				IF(@Cnt > 0)
				BEGIN
					--将订阅该开奖结果连续次数的记录更新为提醒状态,1.提醒，0.不提醒（已提醒过也是0）
					UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 1
						WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo
						AND SiteSysNo = @SiteSysNo AND Cnt = @Cnt + 1
						AND RetNum = @NowRetNumBig
				END
				--中/边
				SELECT @Cnt = COUNT(1) FROM
					(SELECT @NowPeriodNum - (ROW_NUMBER() OVER (ORDER BY PeriodNum DESC)) AS TempPeriodNum, PeriodNum
					FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK)
					WHERE RetNum IN (SELECT RetNum FROM Helpmate.dbo.ResultCategory_28(NOLOCK) WHERE MiddleOrSide = @NowRetNumMiddle)
					AND PeriodNum < @NowPeriodNum AND SiteSysNo = @SiteSysNo) AS T
					WHERE T.TempPeriodNum = T.PeriodNum
				IF(@Cnt > 0)
				BEGIN
					--将订阅该开奖结果连续次数的记录更新为提醒状态,1.提醒，0.不提醒（已提醒过也是0）
					UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 1
						WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo
						AND SiteSysNo = @SiteSysNo AND Cnt = @Cnt + 1
						AND RetNum = @NowRetNumMiddle
				END
				--单/双
				SELECT @Cnt = COUNT(1) FROM
					(SELECT @NowPeriodNum - (ROW_NUMBER() OVER (ORDER BY PeriodNum DESC)) AS TempPeriodNum, PeriodNum
					FROM Helpmate.dbo.SourceData_28_Canada(NOLOCK)
					WHERE RetNum IN (SELECT RetNum FROM Helpmate.dbo.ResultCategory_28(NOLOCK) WHERE OddOrDual = @NowRetNumOdd)
					AND PeriodNum < @NowPeriodNum AND SiteSysNo = @SiteSysNo) AS T
					WHERE T.TempPeriodNum = T.PeriodNum
				IF(@Cnt > 0)
				BEGIN
					--将订阅该开奖结果连续次数的记录更新为提醒状态,1.提醒，0.不提醒（已提醒过也是0）
					UPDATE Helpmate.dbo.RemindStatistics SET [Status] = 1
						WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo
						AND SiteSysNo = @SiteSysNo AND Cnt = @Cnt + 1
						AND RetNum = @NowRetNumOdd
				END
				--更新当前期已刷新
				UPDATE [Helpmate].[dbo].[RemindRefreshTag] SET NowPeriodNum = @NowPeriodNum
					WHERE GameSysNo= @GameSysNo AND SourceSysNo = @SourceSysNo AND SiteSysNo = @SiteSysNo
			END
		END
	END
	

	
END
GO
