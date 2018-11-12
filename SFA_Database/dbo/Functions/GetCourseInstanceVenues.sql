CREATE FUNCTION [dbo].[GetCourseInstanceVenues] (
	@CourseInstanceId int
	)
RETURNS nvarchar(4000)
AS 
BEGIN
	DECLARE @Output VARCHAR(4000)

	DECLARE @DeletedStatus int = (
		SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1
	)

	SELECT @Output = COALESCE(@Output + ', ' + CAST(v.VenueName AS NVARCHAR), CAST(v.VenueName AS NVARCHAR))
	FROM CourseInstanceVenue civ
		JOIN Venue v on v.VenueId = civ.VenueId
	WHERE civ.CourseInstanceId = @CourseInstanceId
		AND RecordStatusId != @DeletedStatus
	ORDER BY v.VenueName

	RETURN @Output
END
