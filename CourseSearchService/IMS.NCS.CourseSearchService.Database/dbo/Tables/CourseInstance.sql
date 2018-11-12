/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[CourseInstance]
(
	[CourseInstanceId]					INT				NOT NULL	PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 524288) /* OPPORTUNITY_ID */
	, [ProviderOwnCourseInstanceRef]	NVARCHAR(255)	NULL		/* PROVIDER_OPPORTUNITY_ID */
	, [Price]							DECIMAL(10,2)				/* PRICE */
	, [DurationUnitId]					INT				NULL		/* DURATION_VALUE */
	, [DurationUnitBulkUploadRef]		NVARCHAR(10)	NULL		/* DURATION_UNITS */
    , [DurationUnitName]				NVARCHAR (50)   NULL
	, [DurationAsText]					NVARCHAR(150)	NULL		/* DURATION_DESCRIPTION */
	, [StartDateDescription]			NVARCHAR(150)	NULL		/* START_DATE_DESCRIPTION */
	, [EndDate]							DATE			NULL		/* END_DATE */
	, [StudyModeBulkUploadRef]			NVARCHAR(10)	NOT NULL	/* STUDY_MODE */
    , [StudyModeName]				    NVARCHAR (50)   NULL
	, [AttendanceModeBulkUploadRef]		NVARCHAR(10)	NOT NULL	/* ATTENDANCE_MODE */
    , [AttendanceModeName]				NVARCHAR (50)   NULL
	, [AttendancePatternBulkUploadRef]	NVARCHAR(10)	NOT NULL	/* ATTENDANCE_PATTERN */
    , [AttendancePatternName]			NVARCHAR (50)   NULL
	, [LanguageOfInstruction]			NVARCHAR(100)	NULL		/* LANGUAGE_OF_INSTRUCTION */
	, [LanguageOfAssessment]			NVARCHAR(100)	NULL		/* LANGUAGE_OF_ASSESSMENT */
	, [PlacesAvailable]					INT				NULL		/* PLACES_AVAILABLE */
	, [EnquiryTo]						NVARCHAR(255)	NULL		/* ENQUIRE_TO */
	, [ApplyTo]							NVARCHAR(255)	NULL		/* APPLY_TO */
	, [ApplyFromDate]					DATE			NULL		/* APPLY_FROM */
	, [ApplyUntilDate]					DATE			NULL		/* APPLY_UNTIL */
	, [ApplyUntilText]					NVARCHAR(100)	NULL		/* APPLY_UNTI_DESC */
	-- Next Table --, [Url]								NVARCHAR(511)	NULL		/* URL */
	, [TimeTable]						NVARCHAR(200)	NULL		/* TIMETABLE */
	, [CourseId]						INT				NOT NULL	/* COURSE_ID */
	, [VenueId]							INT				NULL	/* VENUE_ID */
	, [CanApplyAllYear]					BIT				NOT NULL	/* APPLY_THROUGHOUT_YEAR */
	-- []								BIT				NULL		/* EIS_FLAG */ -- Appears unused, we don't have this in our schema anyway
	, [RegionName]						NVARCHAR(100)	NOT NULL	/* REGION_NAME */
	, [CreatedDateTimeUtc]				DATETIME		NOT NULL	/* DATE_CREATED */
	, [ModifiedDateTimeUtc]				DATETIME		NULL		/* DATE_UPDATE */
	, [RecordStatusId]					INT				NOT NULL	/* STATUS */ -- We only export live records to CSV so this not strictly necessary to have a CASE statement here, but just in case
	, [CreatedByUserId]					NVARCHAR(40)	NOT NULL	/* CREATED_BY */
	, [ModifiedByUserId]				NVARCHAR(40)	NULL		/* UPDATED_BY */
	, [CourseInstanceSummary]			NVARCHAR(1500)	NULL		/* OPPORTUNITY_SUMMARY */
	, [ProviderRegionId]				INT				NULL		/* REGION_ID */
	, [ApplicationId]					INT				NOT NULL	/* SYS_DATA_SOURCE */
	--, []								DATETIME		NULL		/* DATE_UPDATED_COPY_OVER */
	--, []								DATETIME		NULL		/* DATE_CREATED_COPY_OVER */
	, [OfferedByOrganisationId]			INT				NULL		/* OFFERED_BY */
	, [OfferedByProviderId]				INT				NULL		/* OFFERED_BY */
	, [StartDate]						DATETIME		NULL        /* Course earliest start date*/
    , [A10Codes]						NVARCHAR(50)	NULL
	, [DfEFunded]						BIT				NULL 
	, [VenueLocationId]					INT				NULL
	-- Index is not supported on nullable columns
	--, INDEX [IX_CourseInstance_StartDate] NONCLUSTERED HASH ([StartDate]) WITH (BUCKET_COUNT = 262144)
) WITH (MEMORY_OPTIMIZED = ON)