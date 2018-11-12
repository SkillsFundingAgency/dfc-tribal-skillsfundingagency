CREATE PROCEDURE [search].[PublishCourseInstanceA10FundingCode]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

DELETE FROM [dbo].[CourseInstanceA10FundingCode]
INSERT INTO [dbo].[CourseInstanceA10FundingCode] (
	[CourseInstanceId]
	, [A10FundingCode]
)
SELECT
	[CourseInstanceId]
	, [A10FundingCode]
FROM [search].[CourseInstanceA10FundingCode]
	  
RETURN 0
END