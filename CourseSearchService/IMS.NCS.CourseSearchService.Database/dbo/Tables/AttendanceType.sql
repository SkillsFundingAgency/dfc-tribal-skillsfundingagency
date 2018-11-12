/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[AttendanceType]
(
	[AttendanceTypeId]			INT				NOT NULL 
    , [AttendanceTypeName]		NVARCHAR(50)	NOT NULL
    ,[BulkUploadRef]			NVARCHAR(10)	NOT NULL, 
    CONSTRAINT [PK_AttendanceType] PRIMARY KEY NONCLUSTERED HASH ([AttendanceTypeId]) WITH (BUCKET_COUNT = 32) 
) WITH (MEMORY_OPTIMIZED = ON)