CREATE TABLE [dbo].[AttendanceType] (
    [AttendanceTypeId]   INT            NOT NULL,
    [AttendanceTypeName] NVARCHAR (100) NOT NULL,
    [BulkUploadRef] NVARCHAR(10) NOT NULL, 
    [DisplayOrder] INT NOT NULL, 
    CONSTRAINT [PK_AttendanceType] PRIMARY KEY CLUSTERED ([AttendanceTypeId] ASC), 
    CONSTRAINT [CK_AttendanceType_BulkUploadRef] UNIQUE (BulkUploadRef)
);

