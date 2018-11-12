CREATE TABLE [dbo].[Snapshot_QualityScore]
(
	[Period] VARCHAR(7) NOT NULL ,
	[ProviderId] INT NOT NULL,
    -- Provision
    [Courses] INT NOT NULL,
    [CourseInstances] INT NOT NULL,
    [EstimatedFundingAllocation] INT NOT NULL,
    [EstimatedFundingAllocationPerCourseInstance] INT NOT NULL,
    [DistinctCourseTitles] INT NOT NULL,
    [DistinctLearningAims] INT NOT NULL,
    [DistinctCourseUrls] INT NOT NULL,
    [CoursesWithBookingUrls] INT NOT NULL,
    [DistinctCourseBookingUrls] INT NOT NULL,
    [CoursesWithSpecificStartDates] INT NOT NULL,
    [CoursesWithSpecificPrices] INT NOT NULL,
    -- Quality Scoring
    [CoursesWithLongSummary] INT NOT NULL,
    [CoursesWithUrls] INT NOT NULL,
    [CoursesWithDistinctLongSummary] INT NOT NULL,
    [CoursesWithFutureStartDates] DECIMAL(18,1) NOT NULL,
    [CoursesWithLearningAims] INT NOT NULL,
    [CoursesWithAnEntryRequirement] INT NOT NULL,
	-- This is the current score based on stats and is recalculated regularily displayed in the header
    [AutoAggregateQualityRating] DECIMAL(4,1) NULL,
	-- This is a copy of [AutoAggregateQualityRating] at the time of the last audit this value is displayed but not used in calculations
    [AutoAuditQualityRating] DECIMAL(4,1) NULL,
	-- Set at the time of the last audit (Poor, Average, Good, Very Good)
    [ManualAuditQualityRating] INT NULL,
	-- Overall based on [AutoAggregateQualityRating] * [ManualAuditQualityRating] (Poor, Average, Good, Very Good)
    [OverallQualityRating] INT NULL,
	[ModifiedDateTimeUtc] DATETIME NULL,
	[LastActivity] DATETIME NULL,
	[ModifiedByApplicationId] INT NULL,
	[ModifiedByUserId] NVARCHAR(128) NULL,
	[CalculatedDateTimeUtc] DATETIME NOT NULL,
	[AuditedByUserId] NVARCHAR(128) NULL,
	[AuditedDateTimeUtc] DATETIME NULL, 
    PRIMARY KEY ([Period], [ProviderId])
);
