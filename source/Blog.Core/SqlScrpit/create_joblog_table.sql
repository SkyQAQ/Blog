USE [WY_IMS]
GO

/****** Object:  Table [dbo].[tbl_joblog]    Script Date: 2019/3/1 14:53:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_joblog](
	[JobLogId] [uniqueidentifier] NOT NULL,
	[JobGroup] [nvarchar](50) NOT NULL,
	[JobName] [nvarchar](50) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[Result] [nvarchar](3000) NULL,
	[Host] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_joblog] PRIMARY KEY CLUSTERED 
(
	[JobLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO