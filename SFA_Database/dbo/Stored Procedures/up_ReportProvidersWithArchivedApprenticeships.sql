CREATE PROCEDURE [dbo].[up_ReportProvidersWithArchivedApprenticeships]
	@StartDate DATE,
	@EndDate DATE
AS

BEGIN

	DECLARE @ArchiveStatus INT = (SELECT RecordStatusId FROM RecordStatus WHERE IsArchived = 1);

	SELECT P.ProviderId,
		P.ProviderName,
		P.Ukprn,
		COALESCE(Archived.NumberArchived, 0) AS NumberOfArchivedApprenticeships,
		COALESCE(Unarchived.NumberUnarchived, 0) AS NumberOfUnarchivedApprenticeships,
		SUM(CASE WHEN A.RecordStatusId = 3 THEN 1 ELSE 0 END) AS NumberOfCurrentArchivedApprenticeships,
		SUM(CASE WHEN A.RecordStatusId = 2 THEN 1 ELSE 0 END) AS NumberOfCurrentLiveApprenticeships
	FROM Provider P
		LEFT OUTER JOIN (
							SELECT AA.ProviderId,
								Count(DISTINCT AA.ApprenticeshipId) AS NumberArchived
							FROM Audit_Apprenticeship AA
							WHERE AuditOperation = 'U'
								AND	AuditDateUtc BETWEEN @StartDate AND DATEADD(d, 1, @EndDate)
								AND RecordStatusId = @ArchiveStatus
								AND (SELECT TOP 1 RecordStatusId FROM Audit_Apprenticeship 
									WHERE ApprenticeshipId = AA.ApprenticeshipId AND AuditSeq < AA.AuditSeq ORDER BY AuditSeq DESC) <> @ArchiveStatus
							GROUP BY ProviderId
						) Archived ON Archived.ProviderId = P.ProviderId
		LEFT OUTER JOIN (
							SELECT AA.ProviderId,
								Count(DISTINCT AA.ApprenticeshipId) AS NumberUnarchived
							FROM Audit_Apprenticeship AA
							WHERE AuditOperation = 'U'
								AND	AuditDateUtc BETWEEN @StartDate AND DATEADD(d, 1, @EndDate)
								AND RecordStatusId <> @ArchiveStatus
								AND (SELECT TOP 1 RecordStatusId FROM Audit_Apprenticeship 
									WHERE ApprenticeshipId = AA.ApprenticeshipId AND AuditSeq < AA.AuditSeq ORDER BY AuditSeq DESC) = @ArchiveStatus
							GROUP BY ProviderId
						) Unarchived ON Unarchived.ProviderId = P.ProviderId
		LEFT JOIN Apprenticeship A on A.ProviderId = P.ProviderID AND A.RecordStatusId in (2, 3) --All current live / archived apprenticeships
	WHERE Archived.ProviderId IS NOT NULL OR Unarchived.ProviderId IS NOT NULL
	GROUP BY 
		P.ProviderId,
		P.ProviderName,
		P.Ukprn,
		COALESCE(Archived.NumberArchived, 0),
		COALESCE(Unarchived.NumberUnarchived, 0)

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
