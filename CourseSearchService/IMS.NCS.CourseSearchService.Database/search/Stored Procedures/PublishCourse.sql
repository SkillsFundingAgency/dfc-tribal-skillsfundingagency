CREATE PROCEDURE [search].[PublishCourse]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

	DELETE FROM [dbo].[Course];
	INSERT INTO [dbo].[Course] (
		[CourseId]
		,  [ProviderId]
		,  [LearningAimRef]
		,  [CourseTitle]
		,  [CourseSummary]
		,  [ProviderOwnCourseRef]
		,  [QualificationTitle]
		,  [QualificationBulkUploadRef]
		,  [QualificationLevelName]
		,  [LDCS1]
		,  [LDCS2]
		,  [LDCS3]
		,  [LDCS4]
		,  [LDCS5]
		,  [ApplicationId]
		,  [UcasTariffPoints]
		,  [QualificationRefAuthority]
		,  [QualificationRef]
		,  [CourseTypeId]
		,  [CreatedDateTimeUtc]
		,  [ModifiedDateTimeUtc]
		,  [RecordStatusId]
		,  [AwardingOrganisationName]
		,  [CreatedByUserId]
		,  [ModifiedByUserId]
		,  [QualificationTypeRef]
		,  [QualificationTypeName]
		,  [QualificationDataType]
		,  [PrimaryApplicationId]
		,  [ErAppStatus]               
		,  [SkillsForLife]             
		,  [Loans24Plus]               
		,  [AdultLearnerFundingStatus] 
		,  [OtherFundingStatus]        
		,  [IndependentLivingSkills]   
	)
	SELECT
		[CourseId]
		,  [ProviderId]
		,  [LearningAimRef]
		,  [CourseTitle]
		,  [CourseSummary]
		,  [ProviderOwnCourseRef]
		,  [QualificationTitle]
		,  [QualificationBulkUploadRef]
		,  [QualificationLevelName]
		,  [LDCS1]
		,  [LDCS2]
		,  [LDCS3]
		,  [LDCS4]
		,  [LDCS5]
		,  [ApplicationId]
		,  [UcasTariffPoints]
		,  [QualificationRefAuthority]
		,  [QualificationRef]
		,  [CourseTypeId]
		,  [CreatedDateTimeUtc]
		,  [ModifiedDateTimeUtc]
		,  [RecordStatusId]
		,  [AwardingOrganisationName]
		,  [CreatedByUserId]
		,  [ModifiedByUserId]
		,  [QualificationTypeRef]
		,  [QualificationTypeName]
		,  [QualificationDataType]
		,  [PrimaryApplicationId]
		,  [ErAppStatus]               
		,  [SkillsForLife]             
		,  [Loans24Plus]               
		,  [AdultLearnerFundingStatus] 
		,  [OtherFundingStatus]        
		,  [IndependentLivingSkills]   

	FROM [search].[Course];


	DELETE FROM [dbo].[Course1];
	INSERT INTO [dbo].[Course1] (
		[CourseId]
		,  [EntryRequirements]
	)
	SELECT
		[CourseId]
		,  [EntryRequirements]
	FROM [search].[Course];


	DELETE FROM [dbo].[Course2];
	INSERT INTO [dbo].[Course2] (
		[CourseId]
		,  [AssessmentMethod]
	)
	SELECT
		[CourseId]
		,  [AssessmentMethod]
	FROM [search].[Course];


	DELETE FROM [dbo].[Course3];
	INSERT INTO [dbo].[Course3] (
		[CourseId]
		,  [EquipmentRequired])
	SELECT
		[CourseId]
		,  [EquipmentRequired]FROM [search].[Course];


	DELETE FROM [dbo].[Course4];
	INSERT INTO [dbo].[Course4] (
		[CourseId]
		,  [Url]
		,  [BookingUrl])
	SELECT
		[CourseId]
		,  [Url]
		,  [BookingUrl]
	FROM [search].[Course];


	DELETE FROM [dbo].[CourseFreeText];
	INSERT INTO [dbo].[CourseFreeText]
	(
		CourseId,
		CourseTitle,
		CourseSummary,
		QualificationTitle
	)
	SELECT CourseId,
		CourseTitle,
		CourseSummary,
		QualificationTitle
	FROM [dbo].[Course];

	RETURN 0;

END;
