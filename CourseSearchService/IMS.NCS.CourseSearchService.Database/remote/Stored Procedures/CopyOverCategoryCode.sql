CREATE PROCEDURE [remote].[CopyOverCategoryCode]
AS
BEGIN

SET IDENTITY_INSERT [search].[CategoryCode] ON

TRUNCATE TABLE [search].[CategoryCode]
INSERT INTO [search].[CategoryCode] (
	[CategoryCodeId]
	, [CategoryCode]
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
	, [dbo].[CategoryTotal]([CategoryCode], 0)
	, [dbo].[CategoryTotal]([CategoryCode], 1)
	, [dbo].[CategoryLevel]([CategoryCode])
	, [dbo].[CategorySortOrder]([CategoryCode])
FROM [remote].[CategoryCode]

SET IDENTITY_INSERT [search].[CategoryCode] OFF
  
RETURN 0
END
