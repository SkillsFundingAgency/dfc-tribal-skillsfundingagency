CREATE PROCEDURE [remote].[CopyOverCourseInstanceStartDate]
AS
BEGIN

SET IDENTITY_INSERT [search].[CourseInstanceStartDate] ON

TRUNCATE TABLE [search].[CourseInstanceStartDate]
INSERT INTO [search].[CourseInstanceStartDate] (
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
FROM [remote].[CourseInstanceStartDate]

SET IDENTITY_INSERT [search].[CourseInstanceStartDate] OFF
	  
RETURN 0
END