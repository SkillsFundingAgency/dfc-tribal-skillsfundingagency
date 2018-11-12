CREATE TABLE [dbo].[LanguageField](
	[LanguageFieldId] [int] IDENTITY(1,1) NOT NULL,
	[LanguageFieldName] [nvarchar](100) NOT NULL,
	[LanguageKeyChildId] [int] NOT NULL,
	[DefaultLanguageText] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_LanguageFieldNew] PRIMARY KEY CLUSTERED 
(
	[LanguageFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LanguageField]  ADD  CONSTRAINT [FK_LanguageFieldNew_LanguageKeyChild] FOREIGN KEY([LanguageKeyChildId])
REFERENCES [dbo].[LanguageKeyChild] ([LanguageKeyChildId])
GO

ALTER TABLE [dbo].[LanguageField] CHECK CONSTRAINT [FK_LanguageFieldNew_LanguageKeyChild]
GO

CREATE INDEX [IX_LanguageField_LanguageKeyChildId] ON [dbo].[LanguageField] ([LanguageKeyChildId]) INCLUDE ([LanguageFieldId], [LanguageFieldName], [DefaultLanguageText])
GO
