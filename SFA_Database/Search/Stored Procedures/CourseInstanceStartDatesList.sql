CREATE PROCEDURE [search].[CourseInstanceStartDatesList]
AS	

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);
	DECLARE @OneYearAgo DATE = CAST(DATEADD(YEAR, -1, GETUTCDATE()) AS DATE);

	SELECT CI.CourseInstanceId AS OPPORTUNITY_ID,
		dbo.GetCsvDateTimeString(CISD.StartDate) AS [START_DATE],
		CISD.PlacesAvailable AS PLACES_AVAILABLE,
		CASE WHEN CISD.IsMonthOnlyStartDate = 1 THEN 'Mon-RRRR' ELSE 'DD-Mon-RR' END AS DATE_FORMAT
	FROM dbo.CourseInstance CI
		JOIN dbo.CourseInstanceStartDate CISD ON CI.CourseInstanceId = CISD.CourseInstanceId
		JOIN dbo.Course C on C.CourseId = CI.CourseId
		JOIN dbo.Provider P on P.ProviderId = C.ProviderId
	WHERE CI.RecordStatusId = @LiveRecordStatusId
		AND P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1
		AND StartDate >= @OneYearAgo
	ORDER BY CI.CourseInstanceId;

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
