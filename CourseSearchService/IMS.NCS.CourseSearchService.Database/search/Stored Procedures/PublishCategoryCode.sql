CREATE PROCEDURE [search].[PublishCategoryCode]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

DELETE FROM [dbo].[CategoryCode]
INSERT INTO [dbo].[CategoryCode] (
	[CategoryCodeId]
	, [CategoryCodeName]
	, [ParentCategoryCode]
	, [Description]
	, [IsSearchable]
	, [TotalCourses]
	, [TotalUCASCourses]
	, [Level]
	, [SortOrder]
)
SELECT
	[CategoryCodeId]
	, [CategoryCode]
	, [ParentCategoryCode]
	, [Description]
	, [IsSearchable]
	, [TotalCourses]
	, [TotalUCASCourses]
	, [Level]
	, [SortOrder]
FROM [search].[CategoryCode]
	  
RETURN 0
END