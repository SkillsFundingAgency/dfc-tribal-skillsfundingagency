﻿/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[LearningAim] (
    [LearningAimId]               INT            NOT NULL,
    [LaraReleaseVersion]          NVARCHAR (20)  NULL,
    [LaraDownloadDate]            DATETIME2 (7)  NULL,
    [LearningAimRef]              NVARCHAR (10)  NOT NULL,
    [LearningAimTitle]            NVARCHAR (255) NULL,
    [LearningAimType_Desc]        NVARCHAR (255) NULL,
    [AwardingBodyName]            NVARCHAR (150) NULL,
    [EntrySubLevelDesc]           NVARCHAR (255) NULL,
    [NotionalLevelV2Code]         NVARCHAR (10)  NULL,
    [NotionalLevelV2Desc]         NVARCHAR (50)  NULL,
    [CreditBasedTypeDesc]         NVARCHAR (255) NULL,
    [QcaGlh]                      NVARCHAR (100) NULL,
    [SectorLeadBodyDesc]          NVARCHAR (100) NULL,
    [Level2EntitlementCatDesc]    NVARCHAR (100) NULL,
    [Level3EntitlementCatDesc]    NVARCHAR (100) NULL,
    [SkillsForLife]               NVARCHAR (5)   NULL,
    [SkillsForLifeType_Desc]      NVARCHAR (255) NULL,
    [SsaTier1Code]                NVARCHAR (10)  NULL,
    [SsaTier1Desc]                NVARCHAR (255) NULL,
    [SsaTier2Code]                NVARCHAR (10)  NULL,
    [SsaTier2Desc]                NVARCHAR (255) NULL,
    [LdcsCodeCode]                NVARCHAR (10)  NULL,
    [AccreditationStartDate]      DATETIME2 (7)  NULL,
    [AccreditationEndDate]        DATETIME2 (7)  NULL,
    [CertificationEndDate]        DATETIME2 (7)  NULL,
    [FfaCredit]                   NVARCHAR (10)  NULL,
    [IndepLivingSkills]           NVARCHAR (10)  NULL,
    [ErAppStatus]                 NVARCHAR (50)  NULL,
    [ErTtgStatus]                 NVARCHAR (50)  NULL,
    [AdultlrStatus]               NVARCHAR (50)  NULL,
    [OtherfundingNonFundedStatus] NVARCHAR (50)  NULL,
    [LearningAimType]             SMALLINT       NULL,
    [QualReferenceAuthority]      NVARCHAR (50)  NULL,
    [QualificationReference]      NVARCHAR (20)  NULL,
    [QualificationTitle]          NVARCHAR (255) NULL,
    [QualificationLevel]          NVARCHAR (50)  NULL,
    [QualificationType]           NVARCHAR (100) NULL,
    [DateUpdated]                 DATETIME2 (7)  NULL,
    [QualificationTypeCode]       NVARCHAR (4)   NULL,
    [Status]                      NVARCHAR (10)  NULL,
    [QualificationLevelCode]      NVARCHAR (10)  NULL,
    [DateCreated]                 DATETIME2 (7)  NULL,
    [SourceSystemReference]       NVARCHAR (20)  NULL,
    [Section96ApprvlStatusCode]   NVARCHAR (10)  NULL,
    [Section96ApprvlStatusDesc]   NVARCHAR (100) NULL,
    [SkllsDundngApprvStatCode]    NVARCHAR (10)  NULL,
    [SkllsFundngApprvStatDesc]    NVARCHAR (100) NULL,
    PRIMARY KEY NONCLUSTERED HASH ([LearningAimId]) WITH (BUCKET_COUNT = 262144)
)
WITH (MEMORY_OPTIMIZED = ON);

