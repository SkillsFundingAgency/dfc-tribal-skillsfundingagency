CREATE PROCEDURE [dbo].[up_ReportProviderOpportunities]
		@ProviderId int
AS

/*
*	Name:		[up_ReportProviderOpportunities]
*	System: 	Stored procedure interface module
*	Description:	List provider opportunities
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

DECLARE @DeletedStatus int = (
	SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1
)

SELECT
	rs.RecordStatusName
	,ci.ProviderOwnCourseInstanceRef
	,sm.StudyModeName
	,at.AttendanceTypeName
	,ap.AttendancePatternName
	,ci.DurationUnit
	,du.DurationUnitName
	,ci.DurationAsText
	,dbo.GetCourseInstanceStartDates(ci.CourseInstanceId) StartDates
	,ci.StartDateDescription
	,ci.EndDate
	,ci.Price
	,ci.PriceAsText
	,vl.LocationName Region
	,dbo.GetCourseInstanceVenues(ci.CourseInstanceId) Venues
FROM
	CourseInstance ci
	JOIN Course c on c.CourseId = ci.CourseId
	JOIN RecordStatus rs on rs.RecordStatusId = ci.RecordStatusId
	LEFT JOIN StudyMode sm on sm.StudyModeId = ci.StudyModeId
	LEFT JOIN AttendanceType at on at.AttendanceTypeId = ci.AttendanceTypeId
	LEFT JOIN DurationUnit du on du.DurationUnitId = ci.DurationUnitId
	LEFT JOIN AttendancePattern ap on ap.AttendancePatternId = ci.AttendancePatternId
	LEFT JOIN VenueLocation vl on vl.VenueLocationId = ci.VenueLocationId
WHERE c.ProviderId = @ProviderId
	AND ci.RecordStatusId != @DeletedStatus
	AND c.RecordStatusId != @DeletedStatus

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0

