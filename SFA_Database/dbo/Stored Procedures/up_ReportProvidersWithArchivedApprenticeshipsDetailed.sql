CREATE PROCEDURE [dbo].[up_ReportProvidersWithArchivedApprenticeshipsDetailed]
	@StartDate DATE,
	@EndDate DATE
AS

BEGIN

	DECLARE @ArchiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsArchived = 1);

	SELECT P.ProviderId,
		P.ProviderName,
		P.Ukprn,
		CASE WHEN S.StandardCode IS NOT NULL THEN 'Standard' ELSE 'Framework' END AS StandardOrFramework,
		CASE WHEN S.StandardCode IS NOT NULL THEN SSC.StandardSectorCodeDesc + ' - ' + S.StandardName + ' - Level ' + S.NotionalEndLevel
			 ELSE F.NasTitle + CASE WHEN F.PathwayCode IS NULL THEN ' - ' + F.PathwayName ELSE '' END + ' - ' + PT.ProgTypeDesc END AS ApprenticeshipDetails,
		Sum(CASE WHEN Archived.[Type] = 'A' THEN COALESCE(Archived.[Number], 0) ELSE 0 END) AS NumberOfTimesArchived,
		Sum(CASE WHEN Archived.[Type] = 'U' THEN COALESCE(Archived.[Number], 0) ELSE 0 END) AS NumberOfTimesUnarchived,
		COALESCE(RS.RecordStatusName, 'Deleted') AS CurrentStatus
	FROM Provider P
		INNER JOIN (
						SELECT AA.ProviderId,
							AA.ApprenticeshipId,
							AA.StandardCode,
							AA.FrameworkCode,
							AA.ProgType,
							AA.PathwayCode,				
							'A' AS [Type],
							Count(*) AS [Number],
							Max(AA.AuditDateUtc) as LastChange
						FROM Audit_Apprenticeship AA
						WHERE AA.AuditOperation = 'U'
							AND AA.AuditDateUtc BETWEEN @StartDate AND DATEADD(d, 1, @EndDate)
							AND AA.RecordStatusId = @ArchiveStatus
							AND (SELECT TOP 1 RecordStatusId FROM Audit_Apprenticeship 
								WHERE ApprenticeshipId = AA.ApprenticeshipId AND AuditSeq < AA.AuditSeq ORDER BY AuditSeq DESC) <> @ArchiveStatus
						GROUP BY AA.ProviderId,
							AA.ApprenticeshipId,
							AA.StandardCode,
							AA.FrameworkCode,
							AA.ProgType,
							AA.PathwayCode

						UNION ALL

						SELECT AA.ProviderId,
							AA.ApprenticeshipId,
							AA.StandardCode,
							AA.FrameworkCode,
							AA.ProgType,
							AA.PathwayCode,													
							'U' AS [Type],
							Count(*) AS [Number],
							Max(AA.AuditDateUtc) as LastChange
						FROM Audit_Apprenticeship AA
						WHERE AA.AuditOperation = 'U'
							AND AA.AuditDateUtc BETWEEN @StartDate AND DATEADD(d, 1, @EndDate)
							AND AA.RecordStatusId <> @ArchiveStatus
							AND (SELECT TOP 1 RecordStatusId FROM Audit_Apprenticeship 
								WHERE ApprenticeshipId = AA.ApprenticeshipId AND AuditSeq < AA.AuditSeq ORDER BY AuditSeq DESC) = @ArchiveStatus
						GROUP BY AA.ProviderId,
							AA.ApprenticeshipId,
							AA.StandardCode,
							AA.FrameworkCode,
							AA.ProgType,
							AA.PathwayCode
					) Archived ON Archived.ProviderId = P.ProviderId
		LEFT OUTER JOIN [Standard] S ON S.StandardCode = Archived.StandardCode
		LEFT OUTER JOIN [StandardSectorCode] SSC ON SSC.StandardSectorCodeId = S.StandardSectorCode
		LEFT OUTER JOIN [Framework] F ON F.FrameworkCode = Archived.FrameworkCode AND F.ProgType = Archived.ProgType AND F.PathwayCode = Archived.PathwayCode
		LEFT OUTER JOIN [ProgType] PT ON PT.ProgTypeId = F.ProgType
		LEFT OUTER JOIN Apprenticeship AP ON AP.ApprenticeshipId = Archived.ApprenticeshipId
		LEFT OUTER JOIN RecordStatus RS ON AP.RecordStatusId = RS.RecordStatusId
	GROUP BY P.ProviderId,
		P.ProviderName,
		P.Ukprn,
		Archived.ApprenticeshipId,
		S.StandardCode,
		F.FrameworkCode,
		F.ProgType,
		F.PathwayCode,
		CASE WHEN S.StandardCode IS NOT NULL THEN SSC.StandardSectorCodeDesc + ' - ' + S.StandardName + ' - Level ' + S.NotionalEndLevel
			ELSE F.NasTitle + CASE WHEN F.PathwayCode IS NULL THEN ' - ' + F.PathwayName ELSE '' END + ' - ' + PT.ProgTypeDesc END,
		RS.RecordStatusName;

	IF (@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;