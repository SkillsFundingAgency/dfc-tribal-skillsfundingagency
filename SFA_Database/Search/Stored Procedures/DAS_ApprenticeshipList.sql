CREATE PROCEDURE [Search].[DAS_ApprenticeshipList]
AS

BEGIN

	DECLARE	@LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT A.ApprenticeshipId AS APPRENTICESHIP_ID,
		A.ProviderId AS PROVIDER_ID,
		A.StandardCode AS STANDARD_CODE,
		A.FrameworkCode AS FRAMEWORK_CODE,
		A.ProgType AS PROG_TYPE,
		A.PathwayCode AS PATHWAY_CODE,
		A.MarketingInformation AS MARKETING_INFORMATION,
		A.Url AS URL,
		A.ContactTelephone AS CONTACT_TELEPHONE,
		A.ContactEmail AS CONTACT_EMAIL,
		A.ContactWebsite AS CONTACT_WEBSITE
	FROM dbo.Apprenticeship A
		INNER JOIN dbo.Provider P on P.ProviderId=A.ProviderId
	WHERE A.RecordStatusId = @LiveRecordStatusId
		AND	P.RecordStatusId = @LiveRecordStatusId
		AND	P.PublishData = 1
		AND P.ApprenticeshipContract = 1
		AND P.PassedOverallQAChecks = 1
		AND COALESCE(P.MarketingInformation, '') <> ''
	ORDER BY A.ApprenticeshipId;

	RETURN 0;

END;
