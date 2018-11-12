CREATE PROCEDURE [dbo].[up_ProviderUpdateQualityScore]
		@PeriodToRun		VARCHAR(7),
		@ProviderId			int
AS

BEGIN
/*
*	Name:		[up_ProviderUpdateQualityScore]
*	System: 	Stored procedure interface module
*	Description:	Update provider quality score
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/


-- ******************************************************************************************************************
-- NOTE: There is an equivalent version of this stored procedure in the SFA_CourseDirectory database project.
--       Any logic changes made here should be duplicated there
-- ******************************************************************************************************************

	DECLARE
		@LastRun					datetime
		,@LastCourseUpdate			datetime
		,@LastCourseInstanceUpdate	datetime
		,@LastUploadDate			datetime
		,@UpdateLastUploadDate		datetime
		,@LastActivity				DATETIME = (SELECT LastAllDataUpToDateTimeUtc FROM Snapshot_Provider WHERE ProviderId = @ProviderId AND [Period] = @PeriodToRun);

	DECLARE
		-- Where size counts things must be longer than this
		@MinLength int = 200
		-- Live status code
		, @LiveStatus int = (SELECT RecordStatusId FROM [Remote].RecordStatus WHERE IsPublished = 1)
		-- Useful constants
		, @NeverUpdated int = 0
		, @Poor int = 1
		, @Average int = 2
		, @Good int = 3
		, @VeryGood int = 4;

	DECLARE
		-- Provision
		@Courses int
		, @CourseInstances int
		, @EstimatedFundingAllocation int
		, @EstimatedFundingAllocationPerCourseInstance int
		, @DistinctCourseTitles int
		, @DistinctLearningAims int
		, @DistinctCourseUrls int
		, @CoursesWithBookingUrls int
		, @DistinctCourseBookingUrls int
		, @CoursesWithSpecificStartDates int
		, @CoursesWithSpecificPrices int
		-- Quality Scoring
		, @CoursesWithLongSummary int
		, @CoursesWithUrls int
		, @CoursesWithDistinctLongSummary int
		, @CoursesWithFutureStartDates decimal(18,1)
		, @CoursesWithLearningAims int
		, @CoursesWithAnEntryRequirement int
		, @AutoAuditQualityRating decimal(4,1)
		, @AutoAggregateQualityRating decimal(4,1)
		, @OverallQualityRating int
		, @ManualAuditQualityRating int
		, @ModifiedDateTimeUtc datetime
		, @ModifiedByApplicationId int
		, @ModifiedByUserId nvarchar(128)
		, @AuditedByUserId nvarchar(128)
		, @AuditedDateTimeUtc datetime;

	SELECT
		@Courses = Count(*)
		, @CoursesWithLongSummary = Sum(CASE WHEN Len(c.CourseSummary) > @MinLength THEN 1 ELSE 0 END) 
		, @CoursesWithLearningAims = Sum(CASE WHEN c.LearningAimRefId IS NOT NULL AND Len(c.LearningAimRefId) > 0 THEN 1 ELSE 0 END)
		, @CoursesWithUrls = Sum(CASE WHEN c.Url IS NOT NULL AND Len(c.Url) > 0 THEN 1 ELSE 0 END)
		, @CoursesWithAnEntryRequirement = Sum(CASE WHEN c.EntryRequirements IS NOT NULL AND Len(c.EntryRequirements) > 0 THEN 1 ELSE 0 END)
		, @CoursesWithBookingUrls = Sum(CASE WHEN c.BookingUrl IS NOT NULL AND Len(c.BookingUrl) > 0 THEN 1 ELSE 0 END)
	FROM Snapshot_Course c
	WHERE C.[Period] = @PeriodToRun
		AND c.ProviderId = @ProviderId
		AND c.RecordStatusId = @LiveStatus;

	CREATE TABLE #CourseInfo (
		CourseId				int				not null
		, CourseTitle			nvarchar(255)	null
		, CourseSummary			nvarchar(2000)	null
		, BookingUrl			nvarchar(255)	null
		, Url					nvarchar(255)	null
		, LearningAimRefId		nvarchar(10)	null
		, ApplicationId			int				null
		, ModifiedDateTimeUtc	datetime		null
		, ModifiedByUserId		nvarchar(128)	null
	);
	INSERT INTO #CourseInfo
		(CourseId, CourseTitle, CourseSummary, BookingUrl, Url, LearningAimRefId,
		ApplicationId, ModifiedDateTimeUtc, ModifiedByUserId)
	SELECT CourseId, CourseTitle, CourseSummary, BookingUrl, Url, LearningAimRefId,
			c.AddedByApplicationId, Coalesce(c.ModifiedDateTimeUtc, c.CreatedDateTimeUtc),
			Coalesce(c.ModifiedByUserId, c.CreatedByUserId)
	FROM Snapshot_Course c
	WHERE c.[Period] = @PeriodToRun
		AND c.ProviderId = @ProviderId
		AND c.RecordStatusId = @LiveStatus;
	
	-- Courses with unique non-NULL summaries
	SELECT @CoursesWithDistinctLongSummary = Sum(NumThings)
	FROM (
		SELECT Count(*) NumThings
		FROM #CourseInfo
		WHERE CourseSummary IS NOT NULL
			AND Len(CourseSummary) > @MinLength
		GROUP BY CourseSummary
		HAVING Count(*) = 1
	) r;

	-- Courses with unique non-NULL titles
	SELECT @DistinctCourseTitles = Sum(NumThings)
	FROM (
		SELECT Count(*) NumThings
		FROM #CourseInfo
		WHERE CourseTitle IS NOT NULL AND Len(CourseTitle) > 0
		GROUP BY CourseTitle
		HAVING Count(*) = 1
	) r;

	-- Courses with unique non-NULL booking URLs
	SELECT @DistinctCourseBookingUrls = Sum(NumThings)
	FROM (
		SELECT Count(*) NumThings
		FROM #CourseInfo
		WHERE BookingUrl IS NOT NULL AND Len(BookingUrl) > 0
		GROUP BY BookingUrl
		HAVING Count(*) = 1
	) r;

	-- Courses with unique non-NULL URLs
	SELECT @DistinctCourseUrls = Sum(NumThings)
	FROM (
		SELECT Count(*) NumThings
		FROM #CourseInfo
		WHERE Url IS NOT NULL AND Len(Url) > 0
		GROUP BY Url
		HAVING Count(*) = 1
	) r;

	-- Courses with unique learning aims
	SELECT @DistinctLearningAims = Sum(NumThings)
	FROM (
		SELECT Count(*) NumThings
		FROM #CourseInfo
		WHERE LearningAimRefId IS NOT NULL AND Len(LearningAimRefId) > 0
		GROUP BY LearningAimRefId
		HAVING Count(*) = 1
	) r;

	-- Estimated funding of live courses
	SELECT @EstimatedFundingAllocation = Count(*)
	FROM #CourseInfo
	WHERE LearningAimRefId IS NOT NULL AND Len(LearningAimRefId) > 0;

	-- CourseInstance information
	CREATE TABLE #CourseInstanceInfo (
		CourseInstanceId		int				not null
		, LearningAimRefId		nvarchar(10)	null
		, Price					decimal(10,2)	null
		, StartDate				date			null
		, StartDateDescription	nvarchar(150)	null
		, ApplicationId			int				null
		, ModifiedDateTimeUtc	datetime		null
		, ModifiedByUserId		nvarchar(128)	null
	);

	INSERT INTO #CourseInstanceInfo
		(CourseInstanceId, LearningAimRefId, Price, StartDate,
		StartDateDescription,
		ApplicationId, ModifiedDateTimeUtc, ModifiedByUserId)
	SELECT ci.CourseInstanceId, c.LearningAimRefId, ci.Price, cisd.StartDate,
			ci.StartDateDescription,
			ci.AddedByApplicationId, Coalesce(ci.ModifiedDateTimeUtc, ci.CreatedDateTimeUtc),
			Coalesce(ci.ModifiedByUserId, ci.CreatedByUserId)
	FROM #CourseInfo c
		JOIN (SELECT * FROM Snapshot_CourseInstance WHERE [Period] = @PeriodToRun) ci on ci.CourseId = c.CourseId
		LEFT JOIN (SELECT * FROM Snapshot_CourseInstanceStartDate WHERE [Period] = @PeriodToRun) cisd on cisd.CourseInstanceId = ci.CourseInstanceId
	WHERE 
		 ci.RecordStatusId = @LiveStatus;

	SELECT @CourseInstances = Count(DISTINCT ci.CourseInstanceId)
	FROM #CourseInstanceInfo ci;
	
	SELECT @CoursesWithSpecificPrices = Count(DISTINCT ci.CourseInstanceId)
	FROM #CourseInstanceInfo ci
	WHERE Price IS NOT NULL;

	SELECT @CoursesWithSpecificStartDates = Count(DISTINCT ci.CourseInstanceId)
	FROM #CourseInstanceInfo ci
	WHERE StartDate IS NOT NULL;

	DECLARE @today datetime;
	SELECT @today = NextPeriodStartDate FROM DWH_Period_Latest WHERE [Period] = @PeriodToRun;

	SELECT @CoursesWithFutureStartDates = Sum(
		CASE
			WHEN ci.StartDate > @today OR (StartDateDescription IS NOT NULL AND Len(StartDateDescription) > 0) THEN 1
			WHEN ci.StartDate <= @today AND [$(SFA_CourseDirectory)].dbo.GetMonthsBetween(ci.StartDate, @today) < 2 THEN 1
			WHEN ci.StartDate <= @today AND [$(SFA_CourseDirectory)].dbo.GetMonthsBetween(ci.StartDate, @today) = 2 THEN 0.5
			ELSE 0
		END
	)
	FROM (
		SELECT DISTINCT CourseInstanceId, Max(StartDate) StartDate, StartDateDescription
		FROM #CourseInstanceInfo ci
		GROUP BY ci.CourseInstanceId, ci.StartDateDescription
	) ci;

	SELECT @EstimatedFundingAllocationPerCourseInstance = Count(DISTINCT ci.CourseInstanceId)
	FROM #CourseInstanceInfo ci
	WHERE LearningAimRefId IS NOT NULL;
	
	-- Work out who made the last change when and how
	CREATE TABLE #UpdateInfo (
		ModifiedByUserId			nvarchar(128)	null
		, ModifiedDateTimeUtc		datetime		null
		, ModifiedByApplicationId	int				null
	);
	
	INSERT INTO #UpdateInfo
	SELECT Coalesce(ModifiedByUserId, CreatedByUserId)
		, ModifiedDateTimeUtc
		, AddedByApplicationId
	FROM Snapshot_Course
	WHERE [Period] = @PeriodToRun	
		AND ProviderId = @ProviderId		
	UNION 
	SELECT Coalesce(ci.ModifiedByUserId, ci.CreatedByUserId)
		, Coalesce(ci.ModifiedDateTimeUtc, ci.CreatedDateTimeUtc)
		, ci.AddedByApplicationId
	FROM Snapshot_Course c
		JOIN Snapshot_CourseInstance ci on ci.CourseId = c.CourseId AND CI.[Period] = C.[Period]
	WHERE C.[Period] = @PeriodToRun
		AND ProviderId = @ProviderId
	UNION
	SELECT null, null, null;

	SELECT TOP 1
		@ModifiedByUserId = ModifiedByUserId,
		@ModifiedDateTimeUtc = ModifiedDateTimeUtc,
		@ModifiedByApplicationId = ModifiedByApplicationId		
	FROM #UpdateInfo
	ORDER BY ModifiedDateTimeUtc DESC;

	IF (@LastActivity IS NULL OR @ModifiedDateTimeUtc IS NULL OR @ModifiedDateTimeUtc > @LastActivity)
	BEGIN
		SET @LastActivity = @ModifiedDateTimeUtc;
	END;

	-- Get the last update score
	DECLARE @LastUpdateScore int
	SET @LastUpdateScore =
		CASE
			WHEN @LastActivity IS NULL THEN @NeverUpdated							-- Never updated
			WHEN Max(@LastActivity) >= DateAdd(m, -1, GetUtcDate()) THEN @VeryGood	-- Up to 1 month
			WHEN Max(@LastActivity) >= DateAdd(m, -3, GetUtcDate()) THEN @Good		-- Up to 3 months
			WHEN Max(@LastActivity) >=  DateAdd(m, -12, GetUtcDate()) THEN @Average	-- Up to 12 months
			ELSE @Poor																-- Over 12 months
		 END;

	-- Previous Quality Scores and Audit Information
	SELECT @AutoAggregateQualityRating = AutoAggregateQualityRating
		, @AutoAuditQualityRating = AutoAuditQualityRating
		, @ManualAuditQualityRating = ManualAuditQualityRating
		, @OverallQualityRating = OverallQualityRating
		, @AuditedByUserId = AuditedByUserId
		, @AuditedDateTimeUtc = AuditedDateTimeUtc
	FROM Snapshot_QualityScore
	WHERE [Period] = @PeriodToRun
		AND ProviderId = @ProviderId;

	-- Calculate the current auto aggregate score
	DECLARE @divisor decimal (13,5) = 4;
	SET @AutoAggregateQualityRating = ((@LastUpdateScore / @divisor) * 10);
	-- Ensure all our numerators have values
	SET @CoursesWithLongSummary = Coalesce(@CoursesWithLongSummary, 0)
	SET @CoursesWithLongSummary = Coalesce(@CoursesWithLongSummary, 0)
	SET @CoursesWithDistinctLongSummary = Coalesce(@CoursesWithDistinctLongSummary, 0)
	SET @CoursesWithLearningAims = Coalesce(@CoursesWithLearningAims, 0)
	SET @CoursesWithUrls = Coalesce(@CoursesWithUrls, 0)
	SET @CoursesWithAnEntryRequirement = Coalesce(@CoursesWithAnEntryRequirement, 0)
	IF @Courses > 0
	BEGIN
		SET @divisor = @Courses;
		SET @AutoAggregateQualityRating = @AutoAggregateQualityRating
			+ ((@CoursesWithLongSummary / @divisor) * 20)
			+ ((@CoursesWithDistinctLongSummary / @divisor) * 20)
			+ ((@CoursesWithLearningAims / @divisor) * 10)
			+ ((@CoursesWithUrls / @divisor) * 10)
			+ ((@CoursesWithAnEntryRequirement / @divisor) * 10);
	END;
	IF @CourseInstances > 0
	BEGIN
		SET @divisor = @CourseInstances;
		SET @AutoAggregateQualityRating = @AutoAggregateQualityRating
			+ ((@CoursesWithFutureStartDates / @divisor) * 20);
	END;

	-- If a manual audit score exists calcualte the oevrall score
	IF @ManualAuditQualityRating IS NOT NULL
	BEGIN
		-- Turn it into @Poor to @VeryGood
		DECLARE @Aggregate int;
		SET @Aggregate = 
			CASE
				WHEN @AutoAggregateQualityRating >= 91 THEN @VeryGood
				WHEN @AutoAggregateQualityRating >= 71 THEN @Good
				WHEN @AutoAggregateQualityRating >= 51 THEN @Average
				ELSE @Poor
			END;

		SET @OverallQualityRating =
			CASE @Aggregate
				WHEN @Poor THEN
					CASE @ManualAuditQualityRating
						WHEN @Poor THEN @Poor
						WHEN @Average THEN @Average
						WHEN @Good THEN @Average
						WHEN @VeryGood THEN @Good
						ELSE NULL
					END
				WHEN @Average THEN
					CASE @ManualAuditQualityRating
						WHEN @Poor THEN @Poor
						WHEN @Average THEN @Average
						WHEN @Good THEN @Good
						WHEN @VeryGood THEN @Good
						ELSE NULL
					END
				WHEN @Good THEN
					CASE @ManualAuditQualityRating
						WHEN @Poor THEN @Average
						WHEN @Average THEN @Average
						WHEN @Good THEN @Good
						WHEN @VeryGood THEN @VeryGood
						ELSE NULL
					END
				WHEN @VeryGood THEN
					CASE @ManualAuditQualityRating
						WHEN @Poor THEN @Average
						WHEN @Average THEN @Good
						WHEN @Good THEN @Good
						WHEN @VeryGood THEN @VeryGood
						ELSE NULL
					END
				ELSE NULL
			END;
	END;

	DELETE FROM Snapshot_QualityScore 
	WHERE [Period] = @PeriodToRun
		AND ProviderId = @ProviderId;
	
	INSERT INTO Snapshot_QualityScore (
		[Period]
		,ProviderId
		-- Provision
		, Courses
		, CourseInstances
		, EstimatedFundingAllocation
		, EstimatedFundingAllocationPerCourseInstance
		, DistinctCourseTitles
		, DistinctLearningAims
		, DistinctCourseUrls
		, CoursesWithBookingUrls
		, DistinctCourseBookingUrls
		, CoursesWithSpecificStartDates
		, CoursesWithSpecificPrices
		-- Quality Scoring
		, CoursesWithLongSummary
		, CoursesWithUrls
		, CoursesWithDistinctLongSummary
		, CoursesWithFutureStartDates
		, CoursesWithLearningAims
		, CoursesWithAnEntryRequirement
		, AutoAuditQualityRating
		, AutoAggregateQualityRating
		, OverallQualityRating
		, ManualAuditQualityRating
		, ModifiedByUserId
		, ModifiedDateTimeUtc
		, LastActivity
		, ModifiedByApplicationId
		, CalculatedDateTimeUtc
		, AuditedByUserId
		, AuditedDateTimeUtc
	)
	SELECT @PeriodToRun
		,@ProviderId
		-- Provision
		, Coalesce(@Courses,0) Courses
		, Coalesce(@CourseInstances, 0)					CourseInstances
		, Coalesce(@EstimatedFundingAllocation, 0)		EstimatedFundingAllocation
		, Coalesce(@EstimatedFundingAllocationPerCourseInstance, 0) EstimatedFundingAllocationPerCourseInstance
		, Coalesce(@DistinctCourseTitles, 0)			DistinctCourseTitles
		, Coalesce(@DistinctLearningAims, 0)			DistinctLearningAims
		, Coalesce(@DistinctCourseUrls, 0)				DistinctCourseUrls
		, Coalesce(@CoursesWithBookingUrls, 0)			CoursesWithBookingUrls
		, Coalesce(@DistinctCourseBookingUrls, 0)		DistinctCourseBookingUrls
		, Coalesce(@CoursesWithSpecificStartDates, 0)	CoursesWithSpecificStartDates
		, Coalesce(@CoursesWithSpecificPrices, 0)		CoursesWithSpecificPrices
		-- Quality Scoring
		, Coalesce(@CoursesWithLongSummary, 0)			CoursesWithLongSummary
		, Coalesce(@CoursesWithUrls, 0)					CoursesWithUrls
		, Coalesce(@CoursesWithDistinctLongSummary, 0)	CoursesWithDistinctLongSummary
		, Coalesce(@CoursesWithFutureStartDates, 0)		CoursesWithFutureStartDates
		, Coalesce(@CoursesWithLearningAims, 0)			CoursesWithLearningAims
		, Coalesce(@CoursesWithAnEntryRequirement, 0)	CoursesWithAnEntryRequirement
		, @AutoAuditQualityRating						AutoAuditQualityRating
		, Coalesce(@AutoAggregateQualityRating, 0)		AutoAggregateQualityRating
		, @OverallQualityRating							OverallQualityRating
		, @ManualAuditQualityRating						ManualAuditQualityRating
		, @ModifiedByUserId				
		, @ModifiedDateTimeUtc
		, @LastActivity
		, @ModifiedByApplicationId
		, GetUtcDate()
		, @AuditedByUserId
		, @AuditedDateTimeUtc;

	DROP TABLE #CourseInfo;
	DROP TABLE #CourseInstanceInfo;
	DROP TABLE #UpdateInfo;

END;
