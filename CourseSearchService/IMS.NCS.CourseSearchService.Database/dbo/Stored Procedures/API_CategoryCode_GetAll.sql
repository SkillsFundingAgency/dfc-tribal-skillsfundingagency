CREATE PROCEDURE [dbo].[API_CategoryCode_GetAll]
AS 

BEGIN

	SELECT [CategoryCodeId],
		[CategoryCodeName],
		[ParentCategoryCode],
		[Description],
		[IsSearchable],
		[TotalCourses],
		[Level]
	FROM [dbo].[CategoryCode]
	WHERE [Level] <> -1
		AND [TotalCourses] > 0
	ORDER BY [SortOrder]
END