CREATE TABLE [dbo].[EmailTemplateGroup]
(
	[EmailTemplateGroupId] INT NOT NULL,
	[Name] nvarchar(100) NOT NULL,
	[Description] NVARCHAR(MAX) NULL, 
    --[Ordinal] INT NOT NULL,
    CONSTRAINT [PK_EmailTemplateGroup] PRIMARY KEY CLUSTERED ([EmailTemplateGroupId] ASC)
)
