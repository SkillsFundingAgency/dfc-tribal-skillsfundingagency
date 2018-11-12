CREATE PROCEDURE [Search].[DAS_ApprenticeshipLocationList]
AS

BEGIN

	DECLARE	@LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT AL.ApprenticeshipLocationId AS APPRENTICESHIP_LOCATION_ID,
		AL.ApprenticeshipId AS APPRENTICESHIP_ID,
		AL.LocationId AS LOCATION_ID,
		AL.Radius AS RADIUS
	FROM dbo.ApprenticeshipLocation AL
		INNER JOIN dbo.Location L on L.LocationId = AL.LocationId
		INNER JOIN dbo.Apprenticeship A on A.ApprenticeshipId = AL.ApprenticeshipId
		INNER JOIN dbo.Provider P on P.ProviderId= A.ProviderId
	WHERE L.RecordStatusId = @LiveRecordStatusId
		AND	A.RecordStatusId = @LiveRecordStatusId
		AND	P.RecordStatusId = @LiveRecordStatusId
		AND	P.PublishData = 1
		AND P.ApprenticeshipContract = 1
		AND P.PassedOverallQAChecks = 1
		AND COALESCE(P.MarketingInformation, '') <> ''
	ORDER BY AL.ApprenticeshipId;

	RETURN 0;

END;
