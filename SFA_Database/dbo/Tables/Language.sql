CREATE TABLE [dbo].[Language](
	[LanguageID] [int] IDENTITY(1,1) NOT NULL,
	[IETF] [nvarchar](20) NOT NULL,
	[DefaultText] [nvarchar](400) NOT NULL,
	[LanguageFieldName] [nvarchar](400) NULL,
	[SqlLanguageId] [int] NULL,
	[IsDefaultLanguage] [bit] NOT NULL CONSTRAINT [DF_Language_IsDefaultLanguage]  DEFAULT ((0)),
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[LanguageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]