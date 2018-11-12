CREATE TABLE [dbo].[QualificationLevel] (
    [QualificationLevelId]   INT           NOT NULL,
    [QualificationLevelName] NVARCHAR (50) NOT NULL,
    [BulkUploadRef] NVARCHAR(10) NOT NULL, 
    [DisplayOrder] INT NOT NULL, 
    CONSTRAINT [PK_QualificationLevel] PRIMARY KEY CLUSTERED ([QualificationLevelId] ASC), 
    CONSTRAINT [CK_QualificationLevel_BulkUploadRef] UNIQUE (BulkUploadRef)
);

