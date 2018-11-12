﻿CREATE PROCEDURE [remote].[CopyOverCourseText]
AS
BEGIN

SET IDENTITY_INSERT [search].[CourseText] ON

TRUNCATE TABLE [search].[CourseText]
INSERT INTO [search].[CourseText] (
	[CourseTextId]
	, [CourseId]
	, [CourseInstanceId]
	, [StudyModeBulkUploadRef]
	, [AttendanceModeBulkUploadRef]
	, [AttendancePatternBulkUploadRef]
	, [QualificationTypeRef]
	, [EastingMin]
	, [EastingMax]
	, [NorthingMin]
	, [NorthingMax]
	, [Easting]
	, [Northing]
	, [SearchText]
	, [NumberOfCourseInstances]
	, [ModifiedDateTimeUtc]
	, [ProviderId]
	, [LdcsCodes]
	, [ProviderName]
	--, [Loans24Plus]
	, [ApplyUntilDate]
	, [ApplicationFundingStatus]
	--, [Loans24PlusFundingStatus]
	--, [AdultLearnerFundingStatus]
	--, [OtherFundingStatus]
	--, [IndependentLivingSkills]
	, [CanApplyAllYear]
	, [SearchRegion]
	, [SearchTown]
	, [Postcode]
)
SELECT
	[CourseTextId]
	, [CourseId]
	, [CourseInstanceId]
	, [StudyModeBulkUploadRef]
	, [AttendanceModeBulkUploadRef]
	, [AttendancePatternBulkUploadRef]
	, [QualificationTypeRef]
	, [EastingMin]
	, [EastingMax]
	, [NorthingMin]
	, [NorthingMax]
	, [Easting]
	, [Northing]
	, [SearchText]
	, [NumberOfCourseInstances]
	, [ModifiedDateTimeUtc]
	, [ProviderId]
	, [LdcsCodes]
	, [ProviderName]
	--, [Loans24Plus]
	, [ApplyUntilDate]
	, [ApplicationFundingStatus]
	--, [Loans24PlusFundingStatus]
	--, [AdultLearnerFundingStatus]
	--, [OtherFundingStatus]
	--, [IndependentLivingSkills]
	, [CanApplyAllYear]
	, [SearchRegion]
	, [SearchTown]
	, [Postcode]
FROM [remote].[CourseText]

SET IDENTITY_INSERT [search].[CourseText] OFF

RETURN 0
END