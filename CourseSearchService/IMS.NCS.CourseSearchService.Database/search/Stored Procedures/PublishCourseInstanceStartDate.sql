CREATE PROCEDURE [search].[PublishCourseInstanceStartDate]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

DELETE FROM [dbo].[CourseInstanceStartDate]
INSERT INTO [dbo].[CourseInstanceStartDate] (
	[CourseInstanceStartDateId]
	, [CourseInstanceId]
	, [StartDate]
	, [PlacesAvailable]
	, [DateFormat]
)
SELECT
	[CourseInstanceStartDateId]
	, [CourseInstanceId]
	, [StartDate]
	, [PlacesAvailable]
	, [DateFormat]
FROM [search].[CourseInstanceStartDate]
	  
RETURN 0
END