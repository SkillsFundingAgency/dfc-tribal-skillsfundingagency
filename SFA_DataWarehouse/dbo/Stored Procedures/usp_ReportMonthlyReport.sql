CREATE PROCEDURE [dbo].[usp_ReportMonthlyReport]
	@PeriodToRun		VARCHAR(7) = NULL,
	@PeriodType			VARCHAR(1) = 'M'
AS

BEGIN
	
	IF (@PeriodType IS NULL OR @PeriodType <> 'W')
	BEGIN
		SET @PeriodType = 'M';
	END;

	IF (@PeriodToRun IS NULL)
	BEGIN
		SELECT @PeriodToRun = [Period] FROM DWH_Period_Latest WHERE PeriodType = @PeriodType;
	END;

	CREATE TABLE #PeriodsToInclude (
		[Period]		VARCHAR(7)
	);

	INSERT INTO #PeriodsToInclude ([Period]) SELECT TOP 12 [Period] FROM DWH_Period WHERE PeriodType = LEFT(@PeriodToRun, 1) AND [Period] <= @PeriodToRun ORDER BY [Period] DESC; 

	SELECT FORMAT(DWH_Period.PeriodStartDate, 'MMM-yy') AS PeriodHeading,
		MonthlyReport_Usage.*
	FROM MonthlyReport_Usage
		INNER JOIN #PeriodsToInclude ON #PeriodsToInclude.[Period] = MonthlyReport_Usage.[Period]
		INNER JOIN DWH_Period ON DWH_Period.[Period] = MonthlyReport_Usage.[Period]
	ORDER BY MonthlyReport_Usage.[Period];
	
	SELECT MonthlyReport_Provision.*
	FROM MonthlyReport_Provision
		INNER JOIN #PeriodsToInclude ON #PeriodsToInclude.[Period] = MonthlyReport_Provision.[Period]
		INNER JOIN DWH_Period ON DWH_Period.[Period] = MonthlyReport_Provision.[Period]
	ORDER BY MonthlyReport_Provision.[Period];

	SELECT MonthlyReport_Quality.*
	FROM MonthlyReport_Quality
		INNER JOIN #PeriodsToInclude ON #PeriodsToInclude.[Period] = MonthlyReport_Quality.[Period]
		INNER JOIN DWH_Period ON DWH_Period.[Period] = MonthlyReport_Quality.[Period]
	ORDER BY MonthlyReport_Quality.[Period];

	SELECT Avg(SQS.AutoAggregateQualityRating) AS AverageQualityScore 
	FROM Snapshot_QualityScore SQS
		INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQS.ProviderId AND SP.[Period] = SQS.[Period]
	WHERE SQS.[Period] = @PeriodToRun
		AND SP.IsTASOnly = 0;

	DROP TABLE #PeriodsToInclude;

END;
