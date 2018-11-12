CREATE PROCEDURE [dbo].[up_ReportRoATPProviderNotRefreshed]
	@StartDate DATE,
	@EndDate DATE
AS
BEGIN
	--List of providers who NOT have confirmed they have refreshed their apprenticeships this period (i.e. after refresh start date)

	DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	With rankedBatches as
		 (select ProviderId, b.ImportBatchName,
		  ROW_NUMBER() over (PARTITION BY ProviderId Order by ImportDateTimeUtc desc) as rowNum
		  from ImportBatchProvider ibp 
		  join ImportBatch b on ibp.ImportBatchId = b.ImportBatchId)
	select p.ProviderId, p.ProviderName, p.Ukprn, p.TASRefreshOverride, p.RoATPStartDate, t.Description as RoATPProviderType, bLatest.ImportBatchName
	from Provider p 
	left join RoATPProviderType t on p.RoATPProviderTypeId = t.RoATPProviderTypeId
	join (select ProviderId, COUNT(apprenticeshipId) as NumApprenticeships 
			from Apprenticeship a where a.RecordStatusId = @LiveStatus group by ProviderId) al on al.ProviderId = p.ProviderId
	left join ProviderTASRefresh r on p.ProviderId = r.ProviderId and r.RefreshTimeUtc > @StartDate and r.RefreshTimeUtc < DateAdd(d,1,@EndDate)
	left join (select ProviderId, b.[Current] from ImportBatchProvider ibp 
				join ImportBatch b on ibp.ImportBatchId = b.ImportBatchId)  bCurrent on p.ProviderId = bCurrent.ProviderId and bCurrent.[Current] = 1
	left join rankedBatches bLatest on p.ProviderId = bLatest.ProviderId and rowNum=1
	where p.RecordStatusId = @LiveStatus AND p.ApprenticeshipContract = 1 AND p.RoATPFFlag = 1 and p.PassedOverallQAChecks = 1
		and r.TASRefreshId is null and (bCurrent.ProviderId is null or bCurrent.[Current] <> 1);

	RETURN 0;
END;

