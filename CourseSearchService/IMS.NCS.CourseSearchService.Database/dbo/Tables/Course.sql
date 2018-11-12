/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[Course]
(
	[CourseId]	INT					NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 524288) /* COURSE_ID */
	, [ProviderId]					INT				NOT NULL	/* PROVIDER_ID */
	, [LearningAimRef]				NVARCHAR(10)	NULL		/* LAD_ID */
	, [CourseTitle]					NVARCHAR(255)	NOT NULL	/* PROVIDER_COURSE_TITLE */
	, [CourseSummary]				NVARCHAR(2000)	NULL		/* COURSE_SUMMARY */
	, [ProviderOwnCourseRef]		NVARCHAR(255)	NULL		/* PROVIDER_COURSE_ID */
	-- Next Table --, [Url]							NVARCHAR(511)	NULL		/* COURSE_URL */
	-- Next Table --, [BookingUrl]					NVARCHAR(511)	NULL		/* BOOKING_URL */
	-- Next Table --, [EntryRequirements]			NVARCHAR(4000)	NOT NULL	/* ENTRY_REQUIREMENTS */
	-- Next Table --, [AssessmentMethod]			NVARCHAR(4000)	NOT NULL	/* ASSESSMENT_METHOD */
	-- Next Table --, [EquipmentRequired]			NVARCHAR(4000)	NOT NULL	/* EQUIPMENT_REQUIRED */
	--, [QualificationType]			???				???			/* QUALIFICATION_TYPE */ -- Appears to never be populated
	, [QualificationTitle]			NVARCHAR(255)	NULL		/* QUALIFICATION_TITLE */
	, [QualificationBulkUploadRef]	NVARCHAR(10)	NULL		/* QUALIFICATION_LEVEL */
    , [QualificationLevelName]		NVARCHAR (50)   NULL
	, [LDCS1]						NVARCHAR(12)	NULL		/* LDCS1 */
	, [LDCS2]						NVARCHAR(12)	NULL		/* LDCS2 */
	, [LDCS3]						NVARCHAR(12)	NULL		/* LDCS3 */
	, [LDCS4]						NVARCHAR(12)	NULL		/* LDCS4 */
	, [LDCS5]						NVARCHAR(12)	NULL		/* LDCS5 */
	, [ApplicationId]				INT				NOT NULL	/* SYS_DATA_SOURCE */
	, [UcasTariffPoints]			INT				NULL		/* UCAS_TARIFF */
	, [QualificationRefAuthority]   NVARCHAR(10)	NULL		/* QUAL_REF_AUTHORITY */ -- Appears to just default to Ofqual when this has a learning aim ref
	, [QualificationRef]			NVARCHAR(10)	NULL		/* QUAL_REFERENCE */
	, [CourseTypeId]				INT				NULL		/* COURSE_TYPE_ID */ -- TODO Check course type is correct, we have a few 2's and higher numbers in the example CSV output, update Graeme has suggested these other numbers are UCAS related, so only outputting 1 and 3 here currently
	, [CreatedDateTimeUtc]			DATETIME		NOT NULL	/* DATE_CREATED */
	, [ModifiedDateTimeUtc]			DATETIME		NULL		/* DATE_UPDATE */
	, [RecordStatusId]				INT				NOT NULL	/* STATUS */ -- We only export live records to CSV so this not strictly necessary to have a CASE statement here, but just in case
	, [AwardingOrganisationName]	NVARCHAR(150)	NULL		/* AWARDING_ORG_NAME */
	, [CreatedByUserId]				NVARCHAR(50)	NOT NULL	/* CREATED_BY */
	, [ModifiedByUserId]			NVARCHAR(50)	NULL		/* UPDATED_BY */
	, [QualificationTypeRef]		NVARCHAR(10)	NULL		/* QUALIFICATION_TYPE_CODE */
    , [QualificationTypeName]       NVARCHAR (100)  NULL
	, [QualificationDataType]		NVARCHAR(15)	NOT NULL	/* DATA_TYPE */
	, [PrimaryApplicationId]			INT				NOT NULL	/* SYS_DATA */
	--, []								DATETIME		NULL		/* DATE_UPDATED_COPY_OVER */
	--, []								DATETIME		NULL		/* DATE_CREATED_COPY_OVER */
	,[ErAppStatus]                 NVARCHAR (50)  NULL
	,[SkillsForLife]               NVARCHAR (5)   NULL 
	,[Loans24Plus]                 BIT            NULL 
	,[AdultLearnerFundingStatus]   NVARCHAR (16)  NULL 
	,[OtherFundingStatus]          NVARCHAR (16)  NULL 
	,[IndependentLivingSkills]     BIT            NULL 

) WITH (MEMORY_OPTIMIZED = ON)
