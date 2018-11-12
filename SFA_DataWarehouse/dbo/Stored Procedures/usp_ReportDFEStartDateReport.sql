CREATE PROCEDURE [dbo].[usp_ReportDFEStartDateReport]
	@PeriodToRun		VARCHAR(7) = NULL,
	@LastActive		DATE = NULL,
	@YearStart		DATE = null
	
AS

DECLARE @PeriodType VARCHAR(7) = 'M'
DECLARE @LiveStatusId INT = (SELECT RecordStatusId FROM [Remote].RecordStatus WHERE IsPublished = 1);
BEGIN
	
	
	IF (@PeriodToRun IS NULL)
	BEGIN
		SELECT @PeriodToRun = [Period] FROM DWH_Period_Latest WHERE PeriodType = @PeriodType;
	END;

	CREATE TABLE #PeriodsToInclude (
		[Period]		VARCHAR(7)
	);

	INSERT INTO #PeriodsToInclude ([Period]) SELECT TOP 13 [Period] FROM DWH_Period WHERE PeriodType = LEFT(@PeriodToRun, 1) AND [Period] <= @PeriodToRun ORDER BY [Period] DESC; 

	-- DfE Providers historic course and opportunity data

	SELECT DWH_Period.PeriodStartDate AS Period,
		NumberOfCourses, NumberOfLiveCourses, NumberOfOpportunities, NumberOfLiveOpportunities
	FROM DFEReport_Provision DFEP
		INNER JOIN #PeriodsToInclude ON #PeriodsToInclude.[Period] = DFEP.[Period]
		INNER JOIN DWH_Period ON DWH_Period.[Period] = DFEP.[Period]
	ORDER BY DFEP.[Period];
	
	-- DfE Providers Last month and previous quality data 

	SELECT TOP 2 DFEQ.*
	FROM DFEReport_Quality DFEQ
		INNER JOIN #PeriodsToInclude ON #PeriodsToInclude.[Period] = DFEQ.[Period]
		INNER JOIN DWH_Period ON DWH_Period.[Period] = DFEQ.[Period]
	ORDER BY DFEQ.[Period] DESC;

	-- DfE Providers Average quality score

	SELECT 
		CASE WHEN Avg(SQS.AutoAggregateQualityRating) < 51 THEN 'Poor'
			 WHEN Avg(SQS.AutoAggregateQualityRating) < 71 THEN 'Average'
			 WHEN Avg(SQS.AutoAggregateQualityRating) < 91 THEN 'Good'
			 ELSE 'Very Good' END AS AverageQualityScore 
	FROM Snapshot_QualityScore SQS
		INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQS.ProviderId AND SP.[Period] = SQS.[Period]
	WHERE 
		SQS.[Period] = @PeriodToRun AND SP.DFE1619Funded = 1 and SP.RecordStatusId=@LiveStatusId;

	-- DfE providers with no courses
	
	SELECT DFEP.[ProvidersWithNoCourses] AS ProvidersWithNoCourses
	FROM DFEReport_Provision DFEP
	WHERE DFEP.[Period] = @PeriodToRun

	-- Total DfE Providers

	SELECT count(*) AS TotalProviders
	FROM [Snapshot_Provider] SP
	WHERE 
		SP.[Period] = @PeriodToRun and SP.DFE1619Funded = 1 and SP.RecordStatusId=@LiveStatusId;

	-- DfE Quality numpers of providers

	SELECT
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating BETWEEN 51 AND 71 THEN 1 ELSE 0 END), 0) AS Average,
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating BETWEEN 71 AND 91 THEN 1 ELSE 0 END), 0) AS Good,
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
	FROM Snapshot_QualityScore SQ
		INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
	WHERE 
		SQ.[Period] = @PeriodToRun AND SP.DFE1619Funded = 1 and SP.RecordStatusId=@LiveStatusId;

	-- DfE Quality Numbers of providers who are also SFA funded

	SELECT
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating BETWEEN 51 AND 71 THEN 1 ELSE 0 END), 0) AS Average,
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating BETWEEN 71 AND 91 THEN 1 ELSE 0 END), 0) AS Good,
		COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
	FROM Snapshot_QualityScore SQ
		INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
	WHERE 
		SQ.[Period] = @PeriodToRun AND SP.DFE1619Funded = 1 and SP.SFAFunded = 1 and SP.RecordStatusId=@LiveStatusId;

	-- DfE Providers updated since

	SELECT count(DISTINCT SQS.ProviderId) AS TotalProvidersUpdatedSince
	FROM [Snapshot_QualityScore] SQS
		LEFT JOIN Snapshot_Provider SP on SP.ProviderId=SQS.ProviderId  AND SP.[Period] = SQS.[Period] 
	WHERE 
		SP.Period = @PeriodToRun and SP.DFE1619Funded = 1 and SP.RecordStatusId=@LiveStatusId and SQS.LastActivity>@LastActive;
		
	SELECT count(DISTINCT SC.CourseId) AS FundedCoursesUploaded
	FROM [Snapshot_CourseInstance] CI
		LEFT JOIN [Snapshot_Course] SC on SC.CourseId = CI.CourseId and SC.Period = CI.Period and SC.RecordStatusId = @LiveStatusId
		LEFT JOIN [Snapshot_Provider] SP on SP.ProviderId = SC.ProviderId and SP.Period = SC.Period and SP.RecordStatusId = @LiveStatusId
		LEFT JOIN [Snapshot_CourseInstanceA10FundingCode] CIF on CIF.CourseInstanceId = CI.CourseInstanceId and CIF.Period = CI.Period
	where CI.Period = @PeriodToRun and CI.RecordStatusId = @LiveStatusId
	and (
			(SP.DFE1619Funded = 1 and SP.SFAFunded = 0)
		or 
			(SP.DFE1619Funded = 1 and SP.SFAFunded = 1 and CIF.A10FundingCode = 25)
		);

	SELECT count(DISTINCT SC.CourseId) AS FundedCoursesUploadedNextYear
	FROM [Snapshot_CourseInstance] CI
		LEFT JOIN [Snapshot_Course] SC on SC.CourseId = CI.CourseId and SC.Period = CI.Period and SC.RecordStatusId = @LiveStatusId
		LEFT JOIN [Snapshot_CourseInstanceStartDate] CISD on CISD.CourseInstanceId = CI.CourseInstanceId and CI.Period = CISD.Period
		LEFT JOIN [Snapshot_Provider] SP on SP.ProviderId = SC.ProviderId and SP.Period = SC.Period and SP.RecordStatusId = @LiveStatusId
		LEFT JOIN [Snapshot_CourseInstanceA10FundingCode] CIF on CIF.CourseInstanceId = CI.CourseInstanceId and CIF.Period = CI.Period
	where CI.Period = @PeriodToRun and CI.RecordStatusId = @LiveStatusId
	and (
			(SP.DFE1619Funded = 1 and SP.SFAFunded = 0)
		or 
			(SP.DFE1619Funded = 1 and SP.SFAFunded = 1 and CIF.A10FundingCode = 25)
		)
	and CISD.StartDate > @YearStart;

	DROP TABLE #PeriodsToInclude;

END;