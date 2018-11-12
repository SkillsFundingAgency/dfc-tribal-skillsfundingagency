/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[DurationUnit]
(
	[DurationUnitId]		INT				NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 32)
	, [DurationUnitName]	NVARCHAR(50)	NOT NULL
	, [BulkUploadRef]		NVARCHAR(10)	NOT NULL
    , [DisplayOrder]		INT				NOT NULL
    , [WeekEquivalent]		FLOAT			NOT NULL DEFAULT 0
) WITH (MEMORY_OPTIMIZED = ON)