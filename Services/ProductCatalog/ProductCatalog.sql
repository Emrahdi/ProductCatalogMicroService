
CREATE DATABASE ProductCatalog
GO
USE [ProductCatalog]
GO

/****** Object:  Table [Prd].[Product]    Script Date: 22.05.2019 21:09:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Prd].[Product](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](25) COLLATE Turkish_CI_AS NOT NULL,
	[Name] [varchar](50) COLLATE Turkish_CI_AS NOT NULL,
	[Photo] [varbinary](max) NULL,
	[Price] [decimal](22, 2) NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
	[LastUpdatedUser] [varchar](100) COLLATE Turkish_CI_AS NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_PADDING ON

GO

/****** Object:  Index [IX_Product_Code]    Script Date: 22.05.2019 21:09:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_Code] ON [Prd].[Product]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON

GO

/****** Object:  Index [IX_Product_Name]    Script Date: 22.05.2019 21:09:23 ******/
CREATE NONCLUSTERED INDEX [IX_Product_Name] ON [Prd].[Product]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


