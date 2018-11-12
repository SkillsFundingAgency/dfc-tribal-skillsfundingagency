CREATE TABLE [dbo].[BulkUploadStatusHistory]
(
	[BulkUploadStatusHistoryId] INT NOT NULL IDENTITY,
	[BulkUploadId]				INT NOT NULL,
	[BulkUploadStatusId]		INT NOT NULL,
	[LoggedInUserId]			NVARCHAR (128)  NOT NULL,
    [CreatedDateTimeUtc]		DATETIME CONSTRAINT [DF_BulkUploadStatusHistory_CreatedDateTimeUtc] DEFAULT (GETUTCDATE()) NOT NULL,
    CONSTRAINT [PK_BulkUploadStatusHistory] PRIMARY KEY CLUSTERED ([BulkUploadStatusHistoryId] ASC),
	CONSTRAINT [FK_BulkUploadStatusHistory_BulkUpload] FOREIGN KEY ([BulkUploadId]) REFERENCES [dbo].[BulkUpload] ([BulkUploadId]),
    CONSTRAINT [FK_BulkUploadStatusHistory_AspNetUsers] FOREIGN KEY ([LoggedInUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_BulkUploadStatusHistory_BulkUploadStatus] FOREIGN KEY ([BulkUploadStatusId]) REFERENCES [dbo].[BulkUploadStatus] ([BulkUploadStatusId])
)
GO

CREATE INDEX [IX_BulkUploadStatusHistory_BulkUploadId] ON [dbo].[BulkUploadStatusHistory] ([BulkUploadId])
GO

CREATE INDEX [IX_BulkUploadStatusHistory_BulkUploadStatusId] ON [dbo].[BulkUploadStatusHistory] ([BulkUploadStatusId])
GO

CREATE INDEX [IX_BulkUploadStatusHistory_BulkUploadStatusId_Inc] ON [dbo].[BulkUploadStatusHistory] ([BulkUploadStatusId]) INCLUDE ([BulkUploadId], [CreatedDateTimeUtc])
GO
