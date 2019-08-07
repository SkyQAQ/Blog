USE [WY_IMS]
GO

/****** Object:  Table [dbo].[UserInRole]    Script Date: 2018/11/30 16:56:28 ******/
--id默认空值  '00000000-0000-0000-0000-000000000000'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserInRole](
	[UserInRoleId] [uniqueidentifier] NOT NULL,
	[UserInfoId] [uniqueidentifier] NOT NULL,
	[UserCode] [nvarchar](100) NOT NULL,
	[RoleInfoId] [uniqueidentifier] NOT NULL,
	[RoleCode] [nvarchar](100) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](10) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](10) NOT NULL,
	[IsDeleted] [int] NOT NULL,
 CONSTRAINT [PK_UserInRole] PRIMARY KEY CLUSTERED 
(
	[UserInRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserInRole] ADD  CONSTRAINT [DF_UserInRole_UserInRoleId]  DEFAULT (newid()) FOR [UserInRoleId]
GO

ALTER TABLE [dbo].[UserInRole] ADD  CONSTRAINT [DF_UserInRole_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[UserInRole] ADD  CONSTRAINT [DF_UserInRole_CreatedBy]  DEFAULT (N'System') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[UserInRole] ADD  CONSTRAINT [DF_UserInRole_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO

ALTER TABLE [dbo].[UserInRole] ADD  CONSTRAINT [DF_UserInRole_ModifiedBy]  DEFAULT (N'System') FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[UserInRole] ADD  CONSTRAINT [DF_UserInRole_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInRole', @level2type=N'COLUMN',@level2name=N'CreatedOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInRole', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInRole', @level2type=N'COLUMN',@level2name=N'ModifiedOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更改人账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInRole', @level2type=N'COLUMN',@level2name=N'ModifiedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除：0-否；1-是；' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInRole', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO


