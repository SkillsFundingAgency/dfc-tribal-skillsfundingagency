/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[CategoryCode]
(
	[CategoryCodeId]			INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 16384),
	[CategoryCodeName]		NVARCHAR(10) NOT NULL,	/* CATEGORY_CODE */
	[ParentCategoryCode]		NVARCHAR(10) NULL,		/* PARENT_CATEGORY_CODE */
	[Description]				NVARCHAR(200) NOT NULL,	/* DESCRIPTION */
	[IsSearchable]			BIT NOT NULL,			/* SEARCHABLE_FLAG */
	[TotalCourses]			INT NOT NULL DEFAULT 0,			/* COURSE_COUNT */
	[TotalUCASCourses]		INT NOT NULL DEFAULT 0,
    [Level] INT NULL, 
    [SortOrder] VARCHAR(4000) NULL
) WITH (MEMORY_OPTIMIZED = ON)