/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[ProviderType]
(
	[ProviderTypeId]		INT				NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 32)
    , [ProviderTypeName]	NVARCHAR(50)	NOT NULL
) WITH (MEMORY_OPTIMIZED = ON)
