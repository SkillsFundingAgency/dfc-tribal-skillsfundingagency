﻿CREATE TABLE [Search].[Course] (
    [CourseId]                   INT             NOT NULL,
    [ProviderId]                 INT             NOT NULL,
    [LearningAimRef]             NVARCHAR (10)   NULL,
    [CourseTitle]                NVARCHAR (255)  NOT NULL,
    [CourseSummary]              NVARCHAR (2000) NULL,
    [ProviderOwnCourseRef]       NVARCHAR (255)  NULL,
    [Url]                        NVARCHAR (511)  NULL,
    [BookingUrl]                 NVARCHAR (511)  NULL,
	[EntryRequirements]			 NVARCHAR (4000) NOT NULL,
	[AssessmentMethod]			 NVARCHAR (4000) NULL,
	[EquipmentRequired]			 NVARCHAR (4000) NULL,
    [QualificationTitle]         NVARCHAR (255)  NULL,
    [QualificationBulkUploadRef] NVARCHAR (10)   NULL,
    [QualificationLevelName]	 NVARCHAR (50)   NULL,
    [LDCS1]                      NVARCHAR (12)   NULL,
    [LDCS2]                      NVARCHAR (12)   NULL,
    [LDCS3]                      NVARCHAR (12)   NULL,
    [LDCS4]                      NVARCHAR (12)   NULL,
    [LDCS5]                      NVARCHAR (12)   NULL,
    [ApplicationId]              INT             NOT NULL,
    [UcasTariffPoints]           INT             NULL,
    [QualificationRefAuthority]  NVARCHAR (10)   NULL,
    [QualificationRef]           NVARCHAR (10)   NULL,
    [CourseTypeId]               INT             NULL,
    [CreatedDateTimeUtc]         DATETIME        NOT NULL,
    [ModifiedDateTimeUtc]        DATETIME        NULL,
    [RecordStatusId]             INT             NOT NULL,
    [AwardingOrganisationName]   NVARCHAR (150)  NULL,
    [CreatedByUserId]            NVARCHAR (128)  NOT NULL,
    [ModifiedByUserId]           NVARCHAR (128)  NULL,
    [QualificationTypeRef]       NVARCHAR (10)   NULL,
    [QualificationTypeName]      NVARCHAR (100)   NULL,
    [QualificationDataType]      NVARCHAR (15)   NOT NULL,
    [PrimaryApplicationId]       INT             NOT NULL,
	[ErAppStatus]                 NVARCHAR (50)  NULL,
	[SkillsForLife]               NVARCHAR (5)   NULL,
	[Loans24Plus]                 BIT            NULL,
	[AdultLearnerFundingStatus]   NVARCHAR (16)  NULL,
	[OtherFundingStatus]          NVARCHAR (16)  NULL,
	[IndependentLivingSkills]     BIT            NULL,	

    PRIMARY KEY NONCLUSTERED ([CourseId] ASC)
);


