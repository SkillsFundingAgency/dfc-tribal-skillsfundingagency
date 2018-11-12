CREATE TABLE [dbo].[ConfigurationSettings] (
	[Name] NVARCHAR(200) NOT NULL
	, [Value] NVARCHAR(2000) NULL
	, [ValueDefault] NVARCHAR(2000) NULL
	, [DataType] NVARCHAR(50) NULL
	, [Description] NVARCHAR(2000) NULL
	, [LastUpdated] [datetime] NULL
	, [LastUpdatedBy] [uniqueidentifier] NULL
	, [RequiresSiteRestart] [bit] NOT NULL CONSTRAINT [DF_ConfigurationSettings_RequiresSiteRestart] DEFAULT((1))
	, CONSTRAINT [PK_ConfigurationSettings] PRIMARY KEY NONCLUSTERED ([Name] ASC) WITH (
		PAD_INDEX = OFF
		, STATISTICS_NORECOMPUTE = OFF
		, IGNORE_DUP_KEY = OFF
		, ALLOW_ROW_LOCKS = ON
		, ALLOW_PAGE_LOCKS = ON
		, FILLFACTOR = 90
		) ON [PRIMARY]
	) ON [PRIMARY]
GO


