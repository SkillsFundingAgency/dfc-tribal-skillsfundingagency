CREATE PROCEDURE [dbo].[up_CourseInstanceStartDatesListForCsvExport]
	
AS
/*
*	Name:		[up_CourseInstanceStartDatesListForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all CourseInstanceStartDates that are live in a format expected for the Csv Export
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the O_OPP_START_DATES.csv file

DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)

SELECT 
	CI.CourseInstanceId AS OPPORTUNITY_ID,
	dbo.GetCsvDateTimeString(CISD.StartDate) AS [START_DATE],
	CISD.PlacesAvailable AS PLACES_AVAILABLE,
	CASE WHEN CISD.IsMonthOnlyStartDate = 1 THEN 'Mon-RRRR' ELSE 'DD-Mon-RR' END AS DATE_FORMAT
FROM CourseInstance CI
	JOIN CourseInstanceStartDate CISD ON CI.CourseInstanceId = CISD.CourseInstanceId
	JOIN Course C on C.CourseId = CI.CourseId
	JOIN Provider P on P.ProviderId = C.ProviderId
WHERE CI.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1
ORDER BY CI.CourseInstanceId

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0

GO