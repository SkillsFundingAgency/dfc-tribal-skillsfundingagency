CREATE TABLE [dbo].[AttendancePattern] (
    [AttendancePatternId]   INT           NOT NULL,
    [AttendancePatternName] NVARCHAR (50) NOT NULL,
    [DisplayOrder] INT NOT NULL, 
    [BulkUploadRef] NVARCHAR(10) NOT NULL, 
    CONSTRAINT [PK_AttendancePattern] PRIMARY KEY CLUSTERED ([AttendancePatternId] ASC), 
    CONSTRAINT [CK_AttendancePattern_BulkUploadRef] UNIQUE (BulkUploadRef)
);

