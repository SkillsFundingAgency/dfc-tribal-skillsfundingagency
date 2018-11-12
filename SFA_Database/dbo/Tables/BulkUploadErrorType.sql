CREATE TABLE [dbo].[BulkUploadErrorType] (
    [BulkUploadErrorTypeId]   INT           NOT NULL,
    [BulkUploadErrorTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BulkUploadErrorType] PRIMARY KEY CLUSTERED ([BulkUploadErrorTypeId] ASC)
);

