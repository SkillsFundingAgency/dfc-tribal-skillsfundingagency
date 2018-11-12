CREATE PROCEDURE [Search].[DAS_ProviderList]
AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT DISTINCT	P.ProviderId	AS PROVIDER_ID, 
		P.ProviderName				AS PROVIDER_NAME,
		P.Ukprn						AS UKPRN,
		P.Email						AS EMAIL,
		P.Website					AS WEBSITE,
		P.Telephone					AS PHONE,
		P.MarketingInformation		AS MARKETING_INFORMATION,
		FE.LearnerSatisfaction		AS LEARNER_SATISFACTION,
		FE.EmployerSatisfaction		AS EMPLOYER_SATISFACTION,
		P.[NationalApprenticeshipProvider],
		P.TradingName				AS TRADING_NAME
	FROM dbo.Provider P
		LEFT JOIN dbo.FEChoices FE ON FE.UPIN = P.UPIN
	WHERE P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1
		AND EXISTS (SELECT ApprenticeshipId FROM Apprenticeship A WHERE A.ProviderId = P.ProviderId)
		AND P.ApprenticeshipContract = 1
		AND P.PassedOverallQAChecks = 1
		AND COALESCE(P.MarketingInformation, '') <> ''
	ORDER BY PROVIDER_NAME;

END;
