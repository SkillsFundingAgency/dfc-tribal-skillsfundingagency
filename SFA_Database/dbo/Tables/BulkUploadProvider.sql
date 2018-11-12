CREATE TABLE [dbo].[BulkUploadProvider] (

    [BulkUploadProviderId] INT IDENTITY (1, 1) NOT NULL,
    [BulkUploadId]         INT NOT NULL,
    [ProviderId]           INT NOT NULL,
    [IsAuthorisedUpload]   BIT CONSTRAINT [DF_BulkUploadProvider_IsAuthorisedUpload] DEFAULT ((0)) NULL,

    CONSTRAINT [PK_BulkUploadProvider] PRIMARY KEY CLUSTERED ([BulkUploadProviderId] ASC),
    CONSTRAINT [FK_BulkUploadProvider_BulkUpload] FOREIGN KEY ([BulkUploadId]) REFERENCES [dbo].[BulkUpload] ([BulkUploadId]),
	CONSTRAINT [FK_BulkUploadProvider_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId])
)
GO

CREATE INDEX [IX_BulkUploadProvider_BulkUploadId] ON dbo.[BulkUploadProvider] ([BulkUploadId])
GO

CREATE INDEX [IX_BulkUploadProvider_ProviderId] ON dbo.[BulkUploadProvider] ([ProviderId])
GO
