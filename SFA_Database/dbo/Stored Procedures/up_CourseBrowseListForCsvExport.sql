CREATE PROCEDURE [dbo].[up_CourseBrowseListForCsvExport]

AS
/*
*	Name:		[up_CourseBrowseListForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all Course categories and the number of courses in each
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the W_COURSE_BROWSE.csv file

DECLARE @LiveStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

-- Temporary table to store the results of total number of courses
DECLARE @Totals TABLE
(
	CategoryCodeId NVARCHAR(10),
	ParentCategoryCodeId NVARCHAR(10),
	[Description] NVARCHAR(200),
	IsSearchable BIT,
	TotalCourses INT
)
   
-- Get the total number of courses in each category
INSERT INTO @Totals
SELECT CategoryCodeId, ParentCategoryCodeId, LCSBC.[Description], LCSBC.IsSearchable, COUNT(CategoryCodeId) AS Total
FROM LegacyCourseSubjectBrowseCategories LCSBC
LEFT OUTER JOIN CourseLearnDirectClassification CLDC ON LCSBC.CategoryCodeId = CLDC.LearnDirectClassificationRef
LEFT OUTER JOIN Course C ON CLDC.CourseId = C.CourseId AND C.RecordStatusId = @LiveStatusId
LEFT OUTER JOIN Provider P ON C.ProviderId = P.ProviderId AND P.RecordStatusId = @LiveStatusId AND P.PublishData = 1
LEFT OUTER JOIN [Address] A ON P.AddressId = A.AddressId
GROUP BY CategoryCodeId, ParentCategoryCodeId, [Description], LCSBC.IsSearchable;
   
-- Recursive, total up the courses in each category following the parent child path
WITH CTE
	AS ( SELECT  A.CategoryCodeId,
			A.CategoryCodeId AS idRoot ,
			1 AS Level ,
			CAST('\\' AS NVARCHAR(MAX)) + CAST(A.CategoryCodeId AS NVARCHAR(MAX)) AS Path ,
			A.ParentCategoryCodeId,
			A.[Description],
			A.TotalCourses - 1 AS TotalCourses,
			A.IsSearchable
		FROM @Totals A
		WHERE  A.ParentCategoryCodeId IS NULL OR A.ParentCategoryCodeId = ''
		UNION ALL
		SELECT  C.CategoryCodeId,
			P.idRoot,
			P.Level + 1,
			P.Path + CAST('\' AS NVARCHAR(MAX)) + CAST(C.CategoryCodeId AS NVARCHAR(MAX)),
			C.ParentCategoryCodeId,
			C.[Description],
			C.TotalCourses - 1 AS TotalCourses,
			C.IsSearchable
		FROM @Totals C
			INNER JOIN CTE P ON P.CategoryCodeId = C.ParentCategoryCodeId
		)
	SELECT CategoryCodeId AS CATEGORY_CODE, 
		( SELECT ISNULL(SUM(I.TotalCourses), 0) 
		FROM   CTE I
		WHERE   I.Path LIKE O.Path + '\%'	  
		) AS COURSE_COUNT, 
		ParentCategoryCodeId AS PARENT_CATEGORY_CODE,
		[Description] AS [DESCRIPTION],
		CASE WHEN IsSearchable = 1 THEN 'Y' ELSE 'N' END AS SEARCHABLE_FLAG		  
	FROM  CTE O
	ORDER BY CategoryCodeId
	  
IF(@@ERROR <> 0) RETURN 1 ELSE RETURN 0


	
