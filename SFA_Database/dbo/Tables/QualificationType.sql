CREATE TABLE [dbo].[QualificationType]
(
	[QualificationTypeId] INT NOT NULL PRIMARY KEY, 
    [QualificationTypeName] NVARCHAR(100) NOT NULL, 
    [DisplayOrder] INT NOT NULL, 
    [BulkUploadRef] NVARCHAR(10) NOT NULL, 
    [IsHidden] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [CK_QualificationType_BulkUploadRef] UNIQUE (BulkUploadRef)
)
