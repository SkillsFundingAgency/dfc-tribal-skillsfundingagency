CREATE TABLE [dbo].[BulkUploadStatus] (
    [BulkUploadStatusId]   INT            NOT NULL,
    [BulkUploadStatusName] NVARCHAR (50)  NOT NULL,
    [BulkUploadStatusText] NVARCHAR (500) NULL,
    CONSTRAINT [PK_BulkUploadStatus] PRIMARY KEY CLUSTERED ([BulkUploadStatusId] ASC)
);



