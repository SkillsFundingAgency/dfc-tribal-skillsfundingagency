CREATE PROCEDURE [dbo].[API_CategoryCode_GetAll_v2]
(
 	@PublicAPI		INT = 1,
	@APIKey			NVARCHAR(50) = NULL
)
AS 

BEGIN

	-- If this is the public API then ensure that we have a valid API Key
	IF ([dbo].[IsValidAPIKey](@PublicAPI, @APIKey) = 0)
	BEGIN
		RETURN 0;
	END;

	SELECT [CategoryCodeId],
		[CategoryCodeName],
		[ParentCategoryCode],
		[Description],
		[IsSearchable],
		[TotalCourses] + CASE WHEN @PublicAPI = 0 THEN [TotalUCASCourses] ELSE 0 END AS [TotalCourses],
		[Level]
	FROM [dbo].[CategoryCode]
	WHERE [Level] <> -1
		AND [TotalCourses] + CASE WHEN @PublicAPI = 0 THEN [TotalUCASCourses] ELSE 0 END > 0
	ORDER BY [SortOrder];

END;