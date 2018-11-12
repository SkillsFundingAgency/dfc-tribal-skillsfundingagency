CREATE TABLE [dbo].[StudyMode] (
    [StudyModeId]   INT           NOT NULL,
    [StudyModeName] NVARCHAR (50) NOT NULL,
    [BulkUploadRef] NVARCHAR(10) NOT NULL, 
    [DisplayOrder] INT NOT NULL, 
    CONSTRAINT [PK_StudyType] PRIMARY KEY CLUSTERED ([StudyModeId] ASC), 
    CONSTRAINT [CK_StudyMode_BulkUploadRef] UNIQUE (BulkUploadRef)
);

