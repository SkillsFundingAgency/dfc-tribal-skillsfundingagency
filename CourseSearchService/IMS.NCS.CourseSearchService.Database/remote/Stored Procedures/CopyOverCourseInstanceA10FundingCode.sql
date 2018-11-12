CREATE PROCEDURE [remote].[CopyOverCourseInstanceA10FundingCode]
AS
BEGIN

TRUNCATE TABLE [search].[CourseInstanceA10FundingCode]
INSERT INTO [search].[CourseInstanceA10FundingCode] (
	[CourseInstanceId]
	, [A10FundingCode]
)
SELECT
	[CourseInstanceId]
	, [A10FundingCode]
FROM [remote].[CourseInstanceA10FundingCode]
	  
RETURN 0
END