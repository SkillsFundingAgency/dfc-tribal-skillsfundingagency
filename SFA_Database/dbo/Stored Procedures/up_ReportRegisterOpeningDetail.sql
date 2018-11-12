CREATE PROCEDURE [dbo].[up_ReportRegisterOpeningDetail]
	@ImportBatchId INT = NULL -- Pass -1 to run report for all import batches

AS

BEGIN

	IF (@ImportBatchId IS NULL)
	BEGIN
		SET @ImportBatchId = (SELECT ImportBatchId FROM ImportBatch WHERE [Current] = 1);
	END;

	DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	SELECT P.ProviderId,
		P.ProviderName,
		P.UKPRN,
		CASE WHEN P.MarketingInformation IS NULL OR Len(LTrim(RTrim(P.MarketingInformation))) = 0 THEN 'No' ELSE 'Yes' END AS HasProviderLevelData,  /* 1.1 */
		CASE WHEN COALESCE(AppData.NumberOfApprenticeships, 0) = 0 THEN 'No' ELSE 'Yes' END AS HasApprenticeshipLevelData,  /* 1.2 */
		COALESCE(AppData.NumberOfApprenticeships, 0) AS NumberOfApprenticeshipOffers, /* 1.3 */
		CASE WHEN COALESCE(IBP.HasApprenticeshipLevelData, 0) = 0 THEN 'No' ELSE 'Yes' END AS HadApprenticeshipLevelData,   /* 1.5 */
		CASE WHEN PQA.ProviderId IS NOT NULL THEN 'Yes' ELSE 'No' END AS HasBeenOverallQAd,  /* 1.6 */
		CASE WHEN P.PassedOverallQAChecks = 1 THEN 'Yes' ELSE 'No' END AS HasPassedOverallQA,  /* 1.6 */
		CASE WHEN P.PassedOverallQAChecks = 0 THEN 'Yes' ELSE 'No' END AS HasFailedOverallQA,  /* 1.6 */
		CASE WHEN COALESCE(PQA.SpecificEmployerNamed, 0) = 0 THEN 'No' ELSE 'Yes' END AS SpecificEmployerNamed,  /* 1.7 */
		CASE WHEN COALESCE(PQA.UnverifiableClaim, 0) = 0 THEN 'No' ELSE 'Yes' END AS UnverifiableClaim,  /* 1.7 */
		CASE WHEN COALESCE(PQA.IncorrectOfstedGrade, 0) = 0 THEN 'No' ELSE 'Yes' END AS IncorrectOfstedGrade,  /* 1.7 */
		CASE WHEN COALESCE(PQA.InsufficientDetail, 0) = 0 THEN 'No' ELSE 'Yes' END AS InsufficientDetail,  /* 1.7 */
		CASE WHEN COALESCE(PQA.NotAimedAtEmployer, 0) = 0 THEN 'No' ELSE 'Yes' END AS NotAimedAtEmployer,  /* 1.7 */
		COALESCE(AQA.NumberOfApprenticeshipsQAd, 0) AS NumberOfApprenticeshipOffersQAd,  /* 1.8 */
		COALESCE(AQA.Failed, 0) AS NumberOfApprenticeshipsOffersFailed,  /* 1.8 */
		STUFF(
				(	SELECT ', ' + REPLACE(InnerIB.ImportBatchName, ',', '')
					FROM [Provider] InnerP
						INNER JOIN ImportBatchProvider InnerIBP ON InnerIBP.ProviderId = InnerP.ProviderId
						INNER JOIN ImportBatch InnerIB ON InnerIB.ImportBatchId = InnerIBP.ImportBatchId
					WHERE InnerP.ProviderId = P.ProviderId
					FOR XML PATH ('')), 1, 1, '')  AS ImportBatches    /* 2.3 */
	FROM [dbo].[Provider] P
		LEFT OUTER JOIN (SELECT ProviderId, Count(*) AS NumberOfApprenticeships, Max(COALESCE(ModifiedDateTimeUtc, CreatedDateTimeUtc)) AS LastUpdate FROM [dbo].[Apprenticeship] WHERE RecordStatusId = @LiveStatus GROUP BY ProviderId) AppData ON AppData.ProviderId = P.ProviderId
		LEFT OUTER JOIN (SELECT ProviderId, Count(*) AS NumberOfImportBatches FROM [dbo].[ImportBatchProvider] GROUP BY ProviderId) BatchCount ON BatchCount.ProviderId = P.ProviderId
		LEFT OUTER JOIN (SELECT ProviderId, Max(ImportBatchId) AS ImportBatchId FROM [dbo].[ImportBatchProvider] WHERE ImportBatchId = @ImportBatchId OR @ImportBatchId = -1 GROUP BY ProviderId) LatestImportBatch ON LatestImportBatch.ProviderId = P.ProviderId
		LEFT OUTER JOIN (
							SELECT PQAC.ProviderId,
								Passed,
								Sum(CASE WHEN PQACFR.QAComplianceFailureReasonId = 1 THEN 1 ELSE 0 END) AS SpecificEmployerNamed, 
								Sum(CASE WHEN PQACFR.QAComplianceFailureReasonId = 2 THEN 1 ELSE 0 END) AS UnverifiableClaim, 
								Sum(CASE WHEN PQACFR.QAComplianceFailureReasonId = 3 THEN 1 ELSE 0 END) AS IncorrectOfstedGrade, 
								Sum(CASE WHEN PQACFR.QAComplianceFailureReasonId = 4 THEN 1 ELSE 0 END) AS InsufficientDetail, 
								Sum(CASE WHEN PQACFR.QAComplianceFailureReasonId = 5 THEN 1 ELSE 0 END) AS NotAimedAtEmployer
							FROM [dbo].[ProviderQACompliance] PQAC
								LEFT OUTER JOIN [dbo].[ProviderQAComplianceFailureReason] PQACFR ON PQACFR.ProviderQAComplianceId = PQAC.ProviderQAComplianceId
							WHERE EXISTS (
											SELECT *
											FROM ProviderQACompliance 
											WHERE ProviderId = PQAC.ProviderId
											GROUP BY ProviderId
											HAVING Max(ProviderQAComplianceId) = PQAC.ProviderQAComplianceId
										 )
							GROUP BY ProviderId,
								Passed
					    ) PQA ON PQA.ProviderId = P.ProviderId
		LEFT OUTER JOIN (
							SELECT ProviderId,
								Count(*) AS NumberOfApprenticeshipsQAd,
								Sum(CASE WHEN AQA.Passed = 1 THEN 1 ELSE 0 END) AS Passed,
								Sum(CASE WHEN AQA.Passed = 0 THEN 1 ELSE 0 END) AS Failed
							FROM Apprenticeship A
								INNER JOIN (
												SELECT *
												FROM [dbo].[ApprenticeshipQACompliance] AQC
												WHERE EXISTS (
																SELECT ApprenticeshipQAComplianceId
																FROM ApprenticeshipQACompliance 
																WHERE ApprenticeshipId = AQC.ApprenticeshipId
																	AND ApprenticeshipQAComplianceId = AQC.ApprenticeshipQAComplianceId
															 )
											) AQA ON AQA.ApprenticeshipId = A.ApprenticeshipId
							GROUP BY ProviderId
						) AQA ON AQA.ProviderId = P.ProviderId
		INNER JOIN [dbo].[ImportBatchProvider] IBP ON IBP.ProviderId = P.ProviderId
		INNER JOIN (SELECT * FROM [dbo].[ImportBatch] WHERE ImportBatchId = @ImportBatchId OR @ImportBatchId = -1) IB ON IB.ImportBatchId = IBP.ImportBatchId
	WHERE P.RecordStatusId = @LiveStatus
		AND LatestImportBatch.ImportBatchId = IB.ImportBatchId
	ORDER BY ProviderName;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
