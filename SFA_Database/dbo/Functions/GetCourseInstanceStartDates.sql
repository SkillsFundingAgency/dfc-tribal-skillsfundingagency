CREATE FUNCTION [dbo].[GetCourseInstanceStartDates] (
	@CourseInstanceId int
	)
RETURNS nvarchar(4000)
AS 
BEGIN
	DECLARE @Output VARCHAR(4000)

	SELECT @Output = COALESCE(@Output + ', ' + CASE WHEN sd.IsMonthOnlyStartDate = 1 THEN CONVERT(VARCHAR(7),GETDATE(),120) ELSE CAST(sd.StartDate AS NVARCHAR) END, CASE WHEN  sd.IsMonthOnlyStartDate = 1 THEN CONVERT(VARCHAR(7),GETDATE(),120) ELSE CAST(sd.StartDate AS NVARCHAR) END)
	FROM CourseInstanceStartDate sd
	WHERE sd.CourseInstanceId = @CourseInstanceId
	ORDER BY sd.StartDate

	RETURN @Output
END
