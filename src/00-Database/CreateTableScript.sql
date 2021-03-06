USE [Analysis28DB]
GO
/****** Object:  Table [dbo].[UseSource]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UseSource](
	[SysNo] [int] NOT NULL,
	[SourceName] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_UseSource] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UseSite]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UseSite](
	[SysNo] [int] NOT NULL,
	[SiteName] [nvarchar](40) NOT NULL,
	[Comment] [nvarchar](40) NULL,
 CONSTRAINT [PK_UseSite] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[SysNo] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](40) NOT NULL,
	[UserPwd] [nvarchar](40) NOT NULL,
	[UserName] [nvarchar](40) NULL,
	[SecurityQuestion1] [nvarchar](40) NOT NULL,
	[SecurityAnswer1] [nvarchar](40) NOT NULL,
	[SecurityQuestion2] [nvarchar](40) NULL,
	[SecurityAnswer2] [nvarchar](40) NULL,
	[Phone] [nvarchar](40) NULL,
	[QQ] [nvarchar](40) NULL,
	[Status] [int] NOT NULL,
	[RegIP] [nvarchar](40) NULL,
	[RegDate] [datetime] NOT NULL,
	[PayUseBeginTime] [datetime] NOT NULL,
	[PayUseEndTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UseGame]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UseGame](
	[SysNo] [int] NOT NULL,
	[GameName] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_UseGame] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUsers]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUsers](
	[SysNo] [int] IDENTITY(100000,1) NOT NULL,
	[LoginName] [nvarchar](40) NOT NULL,
	[LoginPwd] [nvarchar](40) NOT NULL,
	[Status] [int] NOT NULL,
	[LastLoginTime] [datetime] NULL,
	[LastLoginIP] [nvarchar](32) NULL,
	[LoginTimes] [bigint] NULL,
 CONSTRAINT [PK_SystemUsers] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SourceData_28_Canada]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SourceData_28_Canada](
	[PeriodNum] [bigint] NOT NULL,
	[RetTime] [datetime] NOT NULL,
	[SiteSysNo] [int] NOT NULL,
	[RetOddNum] [int] NOT NULL,
	[RetNum] [int] NOT NULL,
	[RetMidNum] [nvarchar](40) NOT NULL,
	[CollectRet] [nvarchar](100) NOT NULL,
	[CollectTime] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_SourceData_28_Canada] PRIMARY KEY CLUSTERED 
(
	[PeriodNum] ASC,
	[SiteSysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SourceData_28_Beijing]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SourceData_28_Beijing](
	[PeriodNum] [bigint] NOT NULL,
	[RetTime] [datetime] NOT NULL,
	[SiteSysNo] [int] NOT NULL,
	[RetOddNum] [int] NOT NULL,
	[RetNum] [int] NOT NULL,
	[RetMidNum] [nvarchar](40) NOT NULL,
	[CollectRet] [nvarchar](100) NOT NULL,
	[CollectTime] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_SourceData_28_Beijing] PRIMARY KEY CLUSTERED 
(
	[PeriodNum] ASC,
	[SiteSysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResultCategory_28]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResultCategory_28](
	[RetNum] [int] NOT NULL,
	[BigOrSmall] [nvarchar](50) NOT NULL,
	[MiddleOrSide] [nvarchar](50) NOT NULL,
	[OddOrDual] [nvarchar](50) NOT NULL,
	[MantissaBigOrSmall] [nvarchar](50) NOT NULL,
	[ThreeRemainder] [nvarchar](50) NOT NULL,
	[FourRemainder] [nvarchar](50) NOT NULL,
	[FiveRemainder] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ResultCategory_28] PRIMARY KEY CLUSTERED 
(
	[RetNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemindStatistics]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemindStatistics](
	[SysNo] [int] IDENTITY(1,1) NOT NULL,
	[UserSysNo] [int] NOT NULL,
	[GameSysNo] [int] NOT NULL,
	[SourceSysNo] [int] NOT NULL,
	[SiteSysNo] [int] NOT NULL,
	[RetNum] [nvarchar](10) NOT NULL,
	[Cnt] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_RemindStatistics] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemindRefreshTag]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemindRefreshTag](
	[SysNo] [int] IDENTITY(1,1) NOT NULL,
	[GameSysNo] [int] NOT NULL,
	[SourceSysNo] [int] NOT NULL,
	[SiteSysNo] [int] NOT NULL,
	[NowperiodNum] [bigint] NOT NULL,
 CONSTRAINT [PK_RemindRefreshTag] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayLog]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayLog](
	[SysNo] [int] IDENTITY(1,1) NOT NULL,
	[CardSysNo] [bigint] NOT NULL,
	[UserSysNo] [int] NOT NULL,
	[InDate] [datetime] NOT NULL,
	[IP] [nvarchar](32) NULL,
 CONSTRAINT [PK_PayLog] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayCardCategory]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayCardCategory](
	[SysNo] [int] NOT NULL,
	[CategoryName] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_PayCardCategory] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayCard]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayCard](
	[SysNo] [bigint] IDENTITY(10001,1) NOT NULL,
	[PayCardID] [nvarchar](40) NOT NULL,
	[PayCardPwd] [nvarchar](40) NOT NULL,
	[CategorySysNo] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[InDate] [datetime] NOT NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
 CONSTRAINT [PK_PayCard] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OmitStatistics]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OmitStatistics](
	[SysNo] [int] IDENTITY(1,1) NOT NULL,
	[GameSysNo] [int] NOT NULL,
	[SourceSysNo] [int] NOT NULL,
	[SiteSysNo] [int] NOT NULL,
	[RetNum] [int] NOT NULL,
	[OmitCnt] [int] NOT NULL,
	[MaxOmitCnt] [int] NOT NULL,
	[StandardCnt] [int] NOT NULL,
	[NowPeriodNum] [bigint] NOT NULL,
 CONSTRAINT [PK_OmitStatistics] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notices]    Script Date: 06/15/2014 21:13:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notices](
	[SysNo] [int] IDENTITY(1001,1) NOT NULL,
	[Contents] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[Rank] [int] NOT NULL,
	[InDate] [datetime] NOT NULL,
	[PublishUser] [nvarchar](40) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Notices] PRIMARY KEY CLUSTERED 
(
	[SysNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Notices_InDate]    Script Date: 06/15/2014 21:13:59 ******/
ALTER TABLE [dbo].[Notices] ADD  CONSTRAINT [DF_Notices_InDate]  DEFAULT (getdate()) FOR [InDate]
GO
/****** Object:  Default [DF_PayLog_InDate]    Script Date: 06/15/2014 21:13:59 ******/
ALTER TABLE [dbo].[PayLog] ADD  CONSTRAINT [DF_PayLog_InDate]  DEFAULT (getdate()) FOR [InDate]
GO
/****** Object:  Default [DF_SystemUsers_LoginTimes]    Script Date: 06/15/2014 21:13:59 ******/
ALTER TABLE [dbo].[SystemUsers] ADD  CONSTRAINT [DF_SystemUsers_LoginTimes]  DEFAULT ((0)) FOR [LoginTimes]
GO
