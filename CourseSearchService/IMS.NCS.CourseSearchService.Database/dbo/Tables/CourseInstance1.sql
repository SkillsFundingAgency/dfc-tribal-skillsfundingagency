/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[CourseInstance1]
(
	[CourseInstanceId]					INT				NOT NULL	PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 524288) /* OPPORTUNITY_ID */
	, [Url]								NVARCHAR(511)	NULL		/* URL */
) WITH (MEMORY_OPTIMIZED = ON)