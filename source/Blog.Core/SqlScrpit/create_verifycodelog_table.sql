USE [WY_IMS]
GO

/****** Object:  Table [dbo].[tbl_verifycodelog]    Script Date: 2019/3/7 11:37:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_verifycodelog](
	[VerifycodeLogId] [uniqueidentifier] NOT NULL,
	[Receive] [nvarchar](50) NOT NULL,
	[ReceiveType] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[CodeType] [nvarchar](50) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsSuccess] [int] NOT NULL,
 CONSTRAINT [PK_tbl_verifycodelog] PRIMARY KEY CLUSTERED 
(
	[VerifycodeLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

