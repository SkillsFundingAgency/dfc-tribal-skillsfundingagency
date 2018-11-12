CREATE TABLE [dbo].[LanguageKeyChild](
	[LanguageKeyChildId] [int] IDENTITY(1,1) NOT NULL,
	[LanguageKeyGroupId] [int] NOT NULL,
	[KeyChildName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LanguageKeyChild] PRIMARY KEY CLUSTERED 
(
	[LanguageKeyChildId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LanguageKeyChild] ADD CONSTRAINT [FK_LanguageKeyChild_LanguageKeyGroup] FOREIGN KEY([LanguageKeyGroupId])
REFERENCES [dbo].[LanguageKeyGroup] ([LanguageKeyGroupId])
GO

ALTER TABLE [dbo].[LanguageKeyChild] CHECK CONSTRAINT [FK_LanguageKeyChild_LanguageKeyGroup]