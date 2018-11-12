CREATE TABLE [dbo].[BulkUploadExceptionItem] (

    [BulkUploadExceptionItemId] INT             IDENTITY (1, 1) NOT NULL,
    [BulkUploadId]				INT             NOT NULL,
    [BulkUploadSectionId]		INT             NOT NULL,
    [ProviderId]				INT             NULL,
    [BulkUploadErrorTypeId]		INT             NOT NULL,
    [LineNumber]				INT             NULL,
    [ColumnName]				NVARCHAR (4000) NOT NULL,
    [ColumnValue]				NVARCHAR (4000) NOT NULL,
    [Details]					NVARCHAR (4000) NOT NULL,
    [CreatedDateTimeUtc]		DATETIME        CONSTRAINT [DF_BulkUploadExceptionItem_CreatedDateTimeUtc] DEFAULT (getutcdate()) NOT NULL,

    CONSTRAINT [PK_BulkUploadExceptionItem] PRIMARY KEY CLUSTERED ([BulkUploadExceptionItemId] ASC),
    CONSTRAINT [FK_BulkUploadExceptionItem_BulkUpload] FOREIGN KEY ([BulkUploadId]) REFERENCES [dbo].[BulkUpload] ([BulkUploadId]),
    CONSTRAINT [FK_BulkUploadExceptionItem_BulkUploadSection] FOREIGN KEY ([BulkUploadSectionId]) REFERENCES [dbo].[BulkUploadSection] ([BulkUploadSectionId]),
    CONSTRAINT [FK_BulkUploadExceptionItem_BulkUploadErrorType] FOREIGN KEY ([BulkUploadErrorTypeId]) REFERENCES [dbo].[BulkUploadErrorType] ([BulkUploadErrorTypeId]),
    CONSTRAINT [FK_BulkUploadExceptionItem_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId]),
)
GO

CREATE INDEX [IX_BulkUploadExceptionItem_BulkUploadId] ON dbo.[BulkUploadExceptionItem] ([BulkUploadId])
GO

CREATE INDEX [IX_BulkUploadExceptionItem_BulkUploadSectionId] ON dbo.[BulkUploadExceptionItem] ([BulkUploadSectionId])
GO

CREATE INDEX [IX_BulkUploadExceptionItem_ProviderId] ON dbo.[BulkUploadExceptionItem] ([ProviderId])
GO

CREATE INDEX [IX_BulkUploadExceptionItem_BulkUploadErrorTypeId] ON dbo.[BulkUploadExceptionItem] ([BulkUploadErrorTypeId])
GO
