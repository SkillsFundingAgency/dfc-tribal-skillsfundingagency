CREATE TABLE [dbo].[Content]
(
	[ContentId]				INT				IDENTITY(1,1)	NOT NULL,
	[Version]				INT				NOT NULL,
	[Path]					NVARCHAR(1000)	NULL,
	[Title]					NVARCHAR(1000)	NULL,
	[Body]					NVARCHAR(max)	NULL,
	[Scripts]				NVARCHAR(max)	NULL,
	[Styles]				NVARCHAR(max)	NULL,
	[Summary]				NVARCHAR(1000)	NULL,
	[UserContext]			INT				NOT NULL,
	[Embed]					BIT				NOT NULL,
	[RecordStatusId]		INT				NOT NULL,
	[LanguageId]			INT				NOT NULL,
    [CreatedByUserId]		NVARCHAR(128)	NOT NULL,
    [CreatedDateTimeUtc]	DATETIME		NOT NULL,
    [ModifiedByUserId]		NVARCHAR(128)	NULL,
    [ModifiedDateTimeUtc]	DATETIME		NULL,
    CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED ([ContentId] ASC),
    CONSTRAINT [FK_Content_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]), 
    CONSTRAINT [FK_Content_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]), 
    CONSTRAINT [FK_Content_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Content_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers]([Id])
)
