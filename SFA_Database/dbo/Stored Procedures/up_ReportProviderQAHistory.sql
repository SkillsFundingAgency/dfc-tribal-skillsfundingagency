CREATE PROCEDURE [dbo].[up_ReportProviderQAHistory]
		@ShowAllProviders bit
		, @ProviderId int
AS

/*
*	Name:		[up_ReportProviderQAHistory]
*	System: 	Stored procedure interface module
*	Description:	List provider QA history
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2016
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	SELECT *
	FROM (
		SELECT
			qac.CreatedDateTimeUtc QADateTimeUtc
			, u.Name QAUserDisplayName
			, p.ProviderName
			, Passed Status
			, [dbo].[GetApprenticeshipName](a.ApprenticeshipId) EntityQAed
			, [dbo].[GetApprenticeshipQAComplianceFailureReasonsCommaDelimited](qac.ApprenticeshipQAComplianceId) ComplianceChecks
			, NULL StyleChecks
			, qac.TextQAd
			, NULL DetailsOfQA
			, STUFF(( SELECT  ', ' + b.ImportBatchName 
					FROM ImportBatch b
					JOIN ImportBatchProvider pb on b.ImportBatchId = pb.ImportBatchId
					WHERE   pb.ProviderId = p.ProviderId
					FOR XML PATH('')
				), 1, 2, '') as ImportBatchNames
		FROM ApprenticeshipQACompliance qac
			JOIN Apprenticeship a on a.ApprenticeshipId = qac.ApprenticeshipId
			JOIN Provider p on p.ProviderId = a.ProviderId
			JOIN AspNetUsers u on u.Id = qac.CreatedByUserId
		WHERE (@ShowAllProviders = 1 OR a.ProviderId = @ProviderId)
		UNION ALL

		SELECT
			qas.CreatedDateTimeUtc
			, u.Name
			, p.ProviderName
			, Passed Status
			, [dbo].[GetApprenticeshipName](a.ApprenticeshipId) Data
			, NULL ComplianceChecks
			, [dbo].[GetApprenticeshipQAStyleFailureReasonsCommaDelimited](qas.ApprenticeshipQAStyleId) StyleChecks
			, qas.TextQAd
			, qas.DetailsOfQA
			, STUFF(( SELECT  ', ' + b.ImportBatchName 
					FROM ImportBatch b
					JOIN ImportBatchProvider pb on b.ImportBatchId = pb.ImportBatchId
					WHERE   pb.ProviderId = p.ProviderId
					FOR XML PATH('')
				), 1, 2, '') as ImportBatchNames
		FROM ApprenticeshipQAStyle qas
			JOIN Apprenticeship a on a.ApprenticeshipId = qas.ApprenticeshipId
			JOIN Provider p on p.ProviderId = a.ProviderId
			JOIN AspNetUsers u on u.Id = qas.CreatedByUserId
		WHERE (@ShowAllProviders = 1 OR a.ProviderId = @ProviderId)
		UNION ALL

		SELECT
			qac.CreatedDateTimeUtc
			, u.Name CreatedByUserName
			, p.ProviderName
			, Passed Status
			, 'Provider' Data
			, [dbo].[GetProviderQAComplianceFailureReasonsCommaDelimited](qac.ProviderQAComplianceId) ComplianceChecks
			, NULL StyleChecks
			, qac.TextQAd
			, NULL DetailsOfQA
			, STUFF(( SELECT  ', ' + b.ImportBatchName 
					FROM ImportBatch b
					JOIN ImportBatchProvider pb on b.ImportBatchId = pb.ImportBatchId
					WHERE   pb.ProviderId = p.ProviderId
					FOR XML PATH('')
				), 1, 2, '') as ImportBatchNames
		FROM ProviderQACompliance qac
			JOIN Provider p on p.ProviderId = qac.ProviderId
			JOIN AspNetUsers u on u.Id = qac.CreatedByUserId
		WHERE (@ShowAllProviders = 1 OR qac.ProviderId = @ProviderId)
		UNION ALL

		SELECT
			qas.CreatedDateTimeUtc
			, u.Name
			, p.ProviderName
			, Passed Status
			, 'Provider' Data
			, NULL ComplianceChecks
			, [dbo].[GetProviderQAStyleFailureReasonsCommaDelimited](qas.ProviderQAStyleId) StyleChecks
			, qas.TextQAd
			, qas.DetailsOfQA
			, STUFF(( SELECT  ', ' + b.ImportBatchName 
					FROM ImportBatch b
					JOIN ImportBatchProvider pb on b.ImportBatchId = pb.ImportBatchId
					WHERE   pb.ProviderId = p.ProviderId
					FOR XML PATH('')
				), 1, 2, '') as ImportBatchNames
		FROM ProviderQAStyle qas
			JOIN Provider p on p.ProviderId = qas.ProviderId
			JOIN AspNetUsers u on u.Id = qas.CreatedByUserId
		WHERE (@ShowAllProviders = 1 OR qas.ProviderId = @ProviderId)
	) r
	ORDER BY r.QADateTimeUtc DESC;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;