CREATE PROCEDURE [dbo].[up_ReportQualityAssuredProviders]
AS

/*
*	Name:		[up_ReportQualityAssuredProviders]
*	System: 	Stored procedure interface module
*	Description:	List QAd providers
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2017
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	DECLARE
		@LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1)
		-- We pass @QABands in explicitly as this is faster than getting it from the SP
		, @QABands nvarchar(max) = (SELECT Value FROM ConfigurationSettings WHERE Name = 'ApprenticeshipQABands')

	SELECT
		p.ProviderId
		, p.UKPRN
		, p.ProviderName
		, aqa.NumberOfApprenticeships NumberOfApprenticeships
		, [dbo].[GetNumberOfApprenticeshipsToQa](aqa.NumberOfApprenticeships, @QABands) NumberRequiredToQA
		, CASE WHEN p.PassedOverallQAChecks = 1 THEN 1 ELSE 0 END PassedRaw
		, CASE
			WHEN aqa.QAdForCompliance = 0 THEN NULL
			WHEN pqac.Passed != 0 AND aqa.QAdForCompliance = aqa.PassedCompliance THEN 1
			ELSE 0
		  END PassedComplianceRaw
		, aqa.QAdForCompliance NumberQAdForCompliance
		, CASE
			WHEN aqa.QAdForStyle = 0 THEN NULL
			WHEN pqas.Passed != 0 AND aqa.QAdForStyle = aqa.PassedStyle THEN 1
			ELSE 0
		  END PassedStyleRaw
		, aqa.QAdForStyle NumberQAdForStyle
		, STUFF(( SELECT  ', ' + b.ImportBatchName 
				FROM ImportBatch b
                JOIN ImportBatchProvider pb on b.ImportBatchId = pb.ImportBatchId
                WHERE   pb.ProviderId = p.ProviderId
                FOR XML PATH('')
            ), 1, 2, '') as ImportBatchNames
	FROM
		Provider p
		LEFT JOIN (
			-- Get Passed flag from most recent Provider Compliance QA
			SELECT DISTINCT b.ProviderId, a1.Passed, a1.CreatedDateTimeUtc
			FROM ProviderQACompliance a1
			JOIN Provider b
			  ON b.ProviderId = a1.ProviderId
			LEFT JOIN ProviderQACompliance a2
			  ON a2.ProviderId = a1.ProviderId
			  AND a2.CreatedDateTimeUtc > a1.CreatedDateTimeUtc
			WHERE a2.ProviderId IS NULL AND b.RecordStatusId = @LiveStatus
		) pqac ON pqac.ProviderId = p.ProviderId
		LEFT JOIN (
			-- Get Passed flag from most recent Provider Style QA
			SELECT DISTINCT b.ProviderId, a1.Passed, a1.CreatedDateTimeUtc
			FROM ProviderQAStyle a1
			JOIN Provider b
			  ON b.ProviderId = a1.ProviderId
			LEFT JOIN ProviderQAStyle a2
			  ON a2.ProviderId = a1.ProviderId
			  AND a2.CreatedDateTimeUtc > a1.CreatedDateTimeUtc
			WHERE a2.ProviderId IS NULL AND b.RecordStatusId = @LiveStatus
		) pqas ON pqas.ProviderId = p.ProviderId
		LEFT JOIN (
			SELECT
				a.ProviderId
				, Count(*) NumberOfApprenticeships
				, Sum(CASE WHEN aqac.Passed IS NOT NULL THEN 1 ELSE 0 END) QAdForCompliance
				, Sum(CASE WHEN aqas.Passed IS NOT NULL THEN 1 ELSE 0 END) QAdForStyle
				, Sum(CASE WHEN aqac.Passed = 1 THEN 1 ELSE 0 END) PassedCompliance
				, Sum(CASE WHEN aqas.Passed = 1 THEN 1 ELSE 0 END) PassedStyle
				, Max(CASE WHEN aqac.ApprenticeshipId IS NOT NULL OR aqas.ApprenticeshipId IS NOT NULL THEN 1 ELSE 0 END) HasApprenticeshipQA
			FROM Apprenticeship a
				LEFT JOIN (
					-- Get Passed flag from most recent Apprenticeship Compliance QA
					SELECT DISTINCT b.ApprenticeshipId, a1.Passed, a1.CreatedDateTimeUtc
					FROM ApprenticeshipQACompliance a1
					JOIN Apprenticeship b
					  ON b.ApprenticeshipId = a1.ApprenticeshipId
					LEFT JOIN ApprenticeshipQACompliance a2
					  ON a2.ApprenticeshipId = a1.ApprenticeshipId
					  AND a2.CreatedDateTimeUtc > a1.CreatedDateTimeUtc
					WHERE a2.ApprenticeshipId IS NULL AND b.RecordStatusId = @LiveStatus
				) aqac ON aqac.ApprenticeshipId = a.ApprenticeshipId
				LEFT JOIN (
					-- Get Passed flag from most recent Apprenticeship Style QA
					SELECT DISTINCT b.ApprenticeshipId, a1.Passed, a1.CreatedDateTimeUtc
					FROM ApprenticeshipQAStyle a1
					JOIN Apprenticeship b
					  ON b.ApprenticeshipId = a1.ApprenticeshipId
					LEFT JOIN ApprenticeshipQAStyle a2
					  ON a2.ApprenticeshipId = a1.ApprenticeshipId
					  AND a2.CreatedDateTimeUtc > a1.CreatedDateTimeUtc
					WHERE a2.ApprenticeshipId IS NULL AND b.RecordStatusId = @LiveStatus
				) aqas ON aqas.ApprenticeshipId = a.ApprenticeshipId
			WHERE a.RecordStatusId = @LiveStatus
			GROUP BY a.ProviderId
		) aqa on aqa.ProviderId = p.ProviderId
	WHERE
		p.RecordStatusId = @LiveStatus
		AND (
			pqac.ProviderId IS NOT NULL
			OR pqas.ProviderId IS NOT NULL
			OR aqa.HasApprenticeshipQA = 1
		)
		AND aqa.NumberOfApprenticeships IS NOT NULL
	ORDER BY p.ProviderName


	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;