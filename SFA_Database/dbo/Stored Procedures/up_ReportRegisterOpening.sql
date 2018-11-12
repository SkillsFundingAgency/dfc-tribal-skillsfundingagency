CREATE PROCEDURE [dbo].[up_ReportRegisterOpening]
	@ImportBatchId INT = NULL -- Pass -1 to run report for all import batches

AS

BEGIN

	IF (@ImportBatchId IS NULL)
	BEGIN
		SET @ImportBatchId = (SELECT ImportBatchId FROM ImportBatch WHERE [Current] = 1);
	END;

	DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	SELECT COALESCE(Sum(CASE WHEN P.MarketingInformation IS NULL OR Len(LTrim(RTrim(P.MarketingInformation))) = 0 THEN 0 ELSE 1 END), 0) AS NumberOfProvidersWithProviderLevelData,  /* 1.1 */
		COALESCE(Sum(CASE WHEN COALESCE(AppData.NumberOfApprenticeships, 0) = 0 THEN 0 ELSE 1 END), 0) AS NumberOfProvidersWithApprenticeshipLevelData,  /* 1.2 */
		COALESCE(Sum(COALESCE(AppData.NumberOfApprenticeships, 0)), 0) AS NumberOfApprenticeshipOffers, /* 1.3 */
		Count(*) AS NumberOfProvidersWhoHaveAppliedInRound, /* 1.5 */
		COALESCE(Sum(CASE WHEN COALESCE(IBP.HasApprenticeshipLevelData, 0) = 0 THEN 1 ELSE 0 END), 0) AS NumberOfProvidersWithoutApprenticeshipLevelData,   /* 1.5 */
		COALESCE(Sum(CASE WHEN PQA.ProviderId IS NOT NULL THEN 1 ELSE 0 END), 0) AS NumberOfProvidersWhoHaveBeenOverallQAd,  /* 1.6 */
		COALESCE(Sum(CASE WHEN P.PassedOverallQAChecks = 1 THEN 1 ELSE 0 END), 0) AS NumberOfProvidersWhoHavePassedOverallQA,  /* 1.6 */
		COALESCE(Sum(CASE WHEN P.PassedOverallQAChecks = 0 THEN 1 ELSE 0 END), 0) AS NumberOfProvidersWhoHaveFailedOverallQA,  /* 1.6 */
		COALESCE(Sum(COALESCE(PQA.SpecificEmployerNamed, 0)), 0) AS SpecificEmployerNamed,  /* 1.7 */
		COALESCE(Sum(COALESCE(PQA.UnverifiableClaim, 0)), 0) AS UnverifiableClaim,  /* 1.7 */
		COALESCE(Sum(COALESCE(PQA.IncorrectOfstedGrade, 0)), 0) AS IncorrectOfstedGrade,  /* 1.7 */
		COALESCE(Sum(COALESCE(PQA.InsufficientDetail, 0)), 0) AS InsufficientDetail,  /* 1.7 */
		COALESCE(Sum(COALESCE(PQA.NotAimedAtEmployer, 0)), 0) AS NotAimedAtEmployer,  /* 1.7 */
		COALESCE(Sum(COALESCE(AQA.NumberOfApprenticeshipsQAd, 0)), 0) AS NumberOfApprenticeshipOffersQAd,  /* 1.8 */
		COALESCE(Sum(COALESCE(AQA.Failed, 0)), 0) AS NumberOfApprenticeshipsOffersFailed  /* 1.8 */
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
		AND LatestImportBatch.ImportBatchId = IB.ImportBatchId;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
