﻿CREATE PROCEDURE [search].[PublishLearningAim]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

DELETE FROM [dbo].[LearningAim]
INSERT INTO [dbo].[LearningAim] (
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
FROM [search].[LearningAim]
	  
RETURN 0
END