﻿CREATE PROCEDURE [remote].[CopyOverLearningAim]
AS
BEGIN

SET IDENTITY_INSERT [search].[LearningAim] ON

TRUNCATE TABLE [search].[LearningAim]
INSERT INTO [search].[LearningAim] (
	[LearningAimId]
	,  [LaraReleaseVersion]
	,  [LaraDownloadDate]
	,  [LearningAimRef]
	,  [LearningAimTitle]
	,  [LearningAimType_Desc]
	,  [AwardingBodyName]
	,  [EntrySubLevelDesc]
	,  [NotionalLevelV2Code]
	,  [NotionalLevelV2Desc]
	,  [CreditBasedTypeDesc]
	,  [QcaGlh]
	,  [SectorLeadBodyDesc]
	,  [Level2EntitlementCatDesc]
	,  [Level3EntitlementCatDesc]
	,  [SkillsForLife]
	,  [SkillsForLifeType_Desc]
	,  [SsaTier1Code]
	,  [SsaTier1Desc]
	,  [SsaTier2Code]
	,  [SsaTier2Desc]
	,  [LdcsCodeCode]
	,  [AccreditationStartDate]
	,  [AccreditationEndDate]
	,  [CertificationEndDate]
	,  [FfaCredit]
	,  [IndepLivingSkills]
	,  [ErAppStatus]
	,  [ErTtgStatus]
	,  [AdultlrStatus]
	,  [OtherfundingNonFundedStatus]
	,  [LearningAimType]
	,  [QualReferenceAuthority]
	,  [QualificationReference]
	,  [QualificationTitle]
	,  [QualificationLevel]
	,  [QualificationType]
	,  [DateUpdated]
	,  [QualificationTypeCode]
	,  [Status]
	,  [QualificationLevelCode]
	,  [DateCreated]
	,  [SourceSystemReference]
	,  [Section96ApprvlStatusCode]
	,  [Section96ApprvlStatusDesc]
	,  [SkllsDundngApprvStatCode]
	,  [SkllsFundngApprvStatDesc]
)
SELECT
	[LearningAimId]
	,  [LaraReleaseVersion]
	,  [LaraDownloadDate]
	,  [LearningAimRef]
	,  [LearningAimTitle]
	,  [LearningAimType_Desc]
	,  [AwardingBodyName]
	,  [EntrySubLevelDesc]
	,  [NotionalLevelV2Code]
	,  [NotionalLevelV2Desc]
	,  [CreditBasedTypeDesc]
	,  [QcaGlh]
	,  [SectorLeadBodyDesc]
	,  [Level2EntitlementCatDesc]
	,  [Level3EntitlementCatDesc]
	,  [SkillsForLife]
	,  [SkillsForLifeType_Desc]
	,  [SsaTier1Code]
	,  [SsaTier1Desc]
	,  [SsaTier2Code]
	,  [SsaTier2Desc]
	,  [LdcsCodeCode]
	,  [AccreditationStartDate]
	,  [AccreditationEndDate]
	,  [CertificationEndDate]
	,  [FfaCredit]
	,  [IndepLivingSkills]
	,  [ErAppStatus]
	,  [ErTtgStatus]
	,  [AdultlrStatus]
	,  [OtherfundingNonFundedStatus]
	,  [LearningAimType]
	,  [QualReferenceAuthority]
	,  [QualificationReference]
	,  [QualificationTitle]
	,  [QualificationLevel]
	,  [QualificationType]
	,  [DateUpdated]
	,  [QualificationTypeCode]
	,  [Status]
	,  [QualificationLevelCode]
	,  [DateCreated]
	,  [SourceSystemReference]
	,  [Section96ApprvlStatusCode]
	,  [Section96ApprvlStatusDesc]
	,  [SkllsDundngApprvStatCode]
	,  [SkllsFundngApprvStatDesc]
FROM [remote].[LearningAim]

SET IDENTITY_INSERT [search].[LearningAim] OFF

RETURN 0
END