CREATE PROCEDURE [dbo].[up_ReportRoATPProviderRefreshed]
	@StartDate DATE,
	@EndDate DATE
AS
BEGIN
	--List of providers who have confirmed they have refreshed their apprenticeships this period (i.e. after refresh start date)
	select p.ProviderId, p.ProviderName, p.Ukprn, p.RoATPStartDate, t.Description as RoATPProviderType, b.ImportBatchName, r.RefreshTimeUtc, u.Name as ConfirmedBy 
	from ProviderTASRefresh r
	left join ImportBatch b on r.ImportBatchId = b.ImportBatchId
	join Provider p on r.ProviderId = p.ProviderId
	left join RoATPProviderType t on p.RoATPProviderTypeId = t.RoATPProviderTypeId
	join AspNetUsers u on r.RefreshUserId = u.Id
	where r.RefreshTimeUtc > @StartDate and r.RefreshTimeUtc < DateAdd(d,1,@EndDate)

	RETURN 0;
END;
