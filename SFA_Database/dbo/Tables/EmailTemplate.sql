CREATE TABLE [dbo].[EmailTemplate]
(
	[EmailTemplateId] INT NOT NULL,
	[EmailTemplateGroupId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Params] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[HtmlBody] [nvarchar](max) NOT NULL,
	[TextBody] [nvarchar](max) NOT NULL,
	[Priority] [int] NOT NULL,
	--[Ordinal] [int] NOT NULL,
	[UserDescription] [nvarchar](max) NULL,
	CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED ([EmailTemplateId] ASC),
    CONSTRAINT [FK_EmailTemplate_EmailTemplateGroup] FOREIGN KEY ([EmailTemplateGroupId]) REFERENCES [dbo].[EmailTemplateGroup] ([EmailTemplateGroupId])
)