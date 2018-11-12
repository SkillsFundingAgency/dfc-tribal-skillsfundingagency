/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[CourseText]
(
	[CourseTextId]						INT				NOT NULL	PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 524288)
	, [CourseId]						INT				NOT NULL	/* COURSE_ID */
	, [CourseInstanceId]				INT				NOT NULL	/* OPPORTUNITY_ID */
	, [StudyModeBulkUploadRef]			NVARCHAR(10)	NOT NULL	/* STUDY_MODE */
	, [AttendanceModeBulkUploadRef]		NVARCHAR(10)	NOT NULL	/* ATTENDANCE_MODE */
	, [AttendancePatternBulkUploadRef]	NVARCHAR(10)	NOT NULL	/* ATTENDANCE_PATTERN */
	, [QualificationTypeRef]			NVARCHAR(10)	NULL		/* QUAL_TYPE_CODE */
	, [EastingMin]						INT				NULL		/* XMIN */
	, [EastingMax]						INT				NULL		/* XMAX */
	, [NorthingMin]						INT				NULL		/* YMIN */
	, [NorthingMax]						INT				NULL		/* YMAX */
	, [Easting]							FLOAT			NULL		/* X_COORD */
	, [Northing]						FLOAT			NULL		/* Y_COORD */
	-- 255 + 7 + 50 + 7 + 10 + 7 + 10 + 7 + 10 + 7 + 10 + 9 + 10 + 9 + 100 + 9 + 100 + 9 + 10 + 7 + 50
	, [SearchText]						NVARCHAR(700)	NOT NULL	/* SEARCH_TEXT */
	, [NumberOfCourseInstances]			INT				NOT NULL	/* NO_OF_OPPS */
	, [ModifiedDateTimeUtc]				DATETIME		NOT NULL	/* LAST_UPDATE_DATE */
	, [ProviderId]						INT				NOT NULL	/* PROVIDER_ID */
	, [LdcsCodes]						NVARCHAR(50)	NOT NULL	/* SEARCH_LDCS */
	, [ProviderName]					NVARCHAR(100)	NOT NULL	/* PROVIDER_NAME */
	--, []								???				NULL		/* IES_FLAG */ -- TODO Not sure what this is, update, flag no longer used so outputting nothing should be okay
	--, [Loans24Plus]						BIT				NOT NULL	/* TTG_FLAG */
	--, []								???				NULL		/* TQS_FLAG */ -- TODO Not sure what this is, update flag no longer used so outputting nothing should be okay
	--, []								???				NULL		/* SFL_FLAG */ -- TODO Not sure what this is, update flag no longer used so outputting nothing should be okay
	, [ApplyUntilDate]					DATE			NULL		/* APP_DEADLINE */  -- Always seems to be null but assume it should be apply until date
	, [ApplicationFundingStatus]		NVARCHAR(16)	NULL		/* APP_STATUS */
	--, [Loans24PlusFundingStatus]		NVARCHAR(16)	NULL		/* TTG_STATUS */
	--, [AdultLearnerFundingStatus]		NVARCHAR(16)	NULL		/* ADULT_LEARNER */
	--, [OtherFundingStatus]				NVARCHAR(16)	NULL		/* OTHER_FUNDING */
	--, [IndependentLivingSkills]			BIT				NULL		/* ILS_FLAG */
	, [CanApplyAllYear]					BIT				NOT NULL	/* FLEXIBLE_START_FLAG */
	, [SearchRegion]					NVARCHAR(30)	NULL		/* SEARCH_REGION */
	, [SearchTown]						NVARCHAR(100)	NOT NULL	/* SEARCH_TOWN */
	, [Postcode]						NVARCHAR(30)	NOT NULL	/* SEARCH_POSTCODE */
) WITH (MEMORY_OPTIMIZED = ON)