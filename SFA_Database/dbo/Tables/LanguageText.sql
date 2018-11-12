CREATE TABLE [dbo].[LanguageText](
	[LanguageFieldId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[LanguageText] [nvarchar](2000) NOT NULL,
	[ModifiedDateTimeUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_LanguageText] PRIMARY KEY CLUSTERED 
(
	[LanguageFieldId] ASC,
	[LanguageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LanguageText]  ADD CONSTRAINT [FK_LanguageText_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([LanguageID])
GO

ALTER TABLE [dbo].[LanguageText] CHECK CONSTRAINT [FK_LanguageText_Language]
GO
ALTER TABLE [dbo].[LanguageText] ADD CONSTRAINT [FK_LanguageText_LanguageFieldNew] FOREIGN KEY([LanguageFieldId])
REFERENCES [dbo].[LanguageField] ([LanguageFieldId])
GO

ALTER TABLE [dbo].[LanguageText] CHECK CONSTRAINT [FK_LanguageText_LanguageFieldNew]