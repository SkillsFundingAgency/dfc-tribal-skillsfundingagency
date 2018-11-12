CREATE PROCEDURE [Search].[DAS_ApprenticeshipLocationDeliveryModeList]
AS

BEGIN

	DECLARE	@LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT ALDM.ApprenticeshipLocationId AS APPRENTICESHIP_LOCATION_ID,
		ALDM.DeliveryModeId AS DELIVERY_MODE_ID
	FROM dbo.ApprenticeshipLocationDeliveryMode ALDM
	    INNER JOIN dbo.ApprenticeshipLocation AL on AL.ApprenticeshipLocationId = ALDM.ApprenticeshipLocationId
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

END;
