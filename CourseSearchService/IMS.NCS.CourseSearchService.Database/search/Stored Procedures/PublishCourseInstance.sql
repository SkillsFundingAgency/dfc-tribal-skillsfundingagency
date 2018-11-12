CREATE PROCEDURE [search].[PublishCourseInstance]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

	DELETE FROM [dbo].[CourseInstance];
	INSERT INTO [dbo].[CourseInstance] (
		[CourseInstanceId]
		, [ProviderOwnCourseInstanceRef]
		, [Price]
		, [DurationUnitId]
		, [DurationUnitBulkUploadRef]
		, [DurationUnitName]
		, [DurationAsText]
		, [StartDateDescription]
		, [EndDate]
		, [StudyModeBulkUploadRef]
		, [StudyModeName]
		, [AttendanceModeBulkUploadRef]
		, [AttendanceModeName]
		, [AttendancePatternBulkUploadRef]
		, [AttendancePatternName]
		, [LanguageOfInstruction]
		, [LanguageOfAssessment]
		, [PlacesAvailable]
		, [EnquiryTo]
		, [ApplyTo]
		, [ApplyFromDate]
		, [ApplyUntilDate]
		, [ApplyUntilText]
		, [TimeTable]
		, [CourseId]
		, [VenueId]
		, [CanApplyAllYear]
		, [RegionName]
		, [CreatedDateTimeUtc]
		, [ModifiedDateTimeUtc]
		, [RecordStatusId]
		, [CreatedByUserId]
		, [ModifiedByUserId]
		, [CourseInstanceSummary]
		, [ProviderRegionId]
		, [ApplicationId]
		, [OfferedByOrganisationId]
		, [OfferedByProviderId]
		, [StartDate]
		, [A10Codes]
		, [DfEFunded]
		, [VenueLocationId]
	)
	SELECT
		[CourseInstanceId]
		, [ProviderOwnCourseInstanceRef]
		, [Price]
		, [DurationUnitId]
		, [DurationUnitBulkUploadRef]
		, [DurationUnitName]
		, [DurationAsText]
		, [StartDateDescription]
		, [EndDate]
		, [StudyModeBulkUploadRef]
		, [StudyModeName]
		, [AttendanceModeBulkUploadRef]
		, [AttendanceModeName]
		, [AttendancePatternBulkUploadRef]
		, [AttendancePatternName]
		, [LanguageOfInstruction]
		, [LanguageOfAssessment]
		, [PlacesAvailable]
		, [EnquiryTo]
		, [ApplyTo]
		, [ApplyFromDate]
		, [ApplyUntilDate]
		, [ApplyUntilText]
		, [TimeTable]
		, [CourseId]
		, [VenueId]
		, [CanApplyAllYear]
		, [RegionName]
		, [CreatedDateTimeUtc]
		, [ModifiedDateTimeUtc]
		, [RecordStatusId]
		, [CreatedByUserId]
		, [ModifiedByUserId]
		, [CourseInstanceSummary]
		, [ProviderRegionId]
		, [ApplicationId]
		, [OfferedByOrganisationId]
		, [OfferedByProviderId]
		, [StartDate]
		, [A10Codes]
		, [DfEFunded]
		, [VenueLocationId]
	FROM [search].[CourseInstance];

	DELETE FROM [dbo].[CourseInstance1];
	INSERT INTO [dbo].[CourseInstance1] (
		[CourseInstanceId]
		, [Url])
	SELECT [CourseInstanceId]
		, [Url]
	FROM [search].[CourseInstance];

	DELETE FROM [dbo].[CourseInstancePriceDescription];
	INSERT INTO [dbo].[CourseInstancePriceDescription] (
		CourseInstanceId,
		PriceAsText
	)
	SELECT [CourseInstanceId],
		[PriceAsText]
	FROM [search].[CourseInstance]
	WHERE [PriceAsText] IS NOT NULL;
	  
	RETURN 0;

END;