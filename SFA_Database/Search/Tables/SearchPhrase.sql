CREATE TABLE [Search].[SearchPhrase]
(
	[SearchPhraseId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [Phrase] NVARCHAR(50) NOT NULL,
	[CreatedByUserId]      NVARCHAR(128)             NOT NULL,
    [CreatedDateTimeUtc]   DATETIME        NOT NULL DEFAULT GetUtcDate(),
    [ModifiedByUserId]     NVARCHAR(128)             NULL,
    [ModifiedDateTimeUtc]  DATETIME        NULL, 
    [Ordinal] INT NOT NULL, 
    [RecordStatusId] INT NOT NULL, 
    [RemovePhraseFromSearch] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_SearchPhrase_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus]([RecordStatusId]), 
    CONSTRAINT [FK_SearchPhrase_CreatedByUser] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]), 
    CONSTRAINT [FK_SearchPhrase_ModifiedByUser] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])	
)
