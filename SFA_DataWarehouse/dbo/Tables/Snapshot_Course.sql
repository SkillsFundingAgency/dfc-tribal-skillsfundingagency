CREATE TABLE [dbo].[Snapshot_Course] (
	[Period]			   VARCHAR(7)	   NOT NULL,
    [CourseId]             INT             NOT NULL,
    [ProviderId]           INT             NOT NULL,
    [CourseTitle]          NVARCHAR (255)  NOT NULL,
    [CourseSummary]        NVARCHAR (2000) NULL,
    [CreatedByUserId]      NVARCHAR(128)             NOT NULL,
    [CreatedDateTimeUtc]   DATETIME        NOT NULL,
    [ModifiedByUserId]     NVARCHAR(128)             NULL,
    [ModifiedDateTimeUtc]  DATETIME        NULL,
    [AddedByApplicationId] INT             NOT NULL,
    [RecordStatusId]       INT             NOT NULL,
    [LearningAimRefId]     VARCHAR (10)    NULL,
    [QualificationLevelId] INT             NULL,
    [EntryRequirements]    NVARCHAR (4000) NOT NULL,
    [ProviderOwnCourseRef] NVARCHAR(255) NULL, 
    [Url] NVARCHAR(255) NULL, 
    [BookingUrl] NVARCHAR(255) NULL, 
    [AssessmentMethod] NVARCHAR(4000) NULL, 
    [EquipmentRequired] NVARCHAR(4000) NULL, 
    [WhenNoLarQualificationTypeId] INT NULL, 
    [WhenNoLarQualificationTitle] NVARCHAR(255) NULL, 
    [AwardingOrganisationName] NVARCHAR(150) NULL, 
    [UcasTariffPoints] INT NULL, 
    [IsDfE] BIT NOT NULL DEFAULT 0, 
    [IsSFA] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Snapshot_Course] PRIMARY KEY CLUSTERED ([Period], [CourseId])
);
GO

CREATE INDEX [IX_Snapshot_Course_ProviderId] ON [dbo].[Snapshot_Course] ([Period], [ProviderId])
GO

CREATE INDEX [IX_Course_RecordStatusId] ON [dbo].[Snapshot_Course] ([Period], [RecordStatusId])
GO

CREATE INDEX [IX_Course_WhenNoLarQualificationTypeId] ON [dbo].[Snapshot_Course] ([Period], [WhenNoLarQualificationTypeId])
GO

CREATE INDEX [IX_Course_LearningAimRefId] ON [dbo].[Snapshot_Course] ([Period], [LearningAimRefId])
GO

CREATE INDEX [IX_Course_RecordStatusId_Inc1] ON [dbo].[Snapshot_Course] ([Period], [RecordStatusId]) INCLUDE ([CourseId], [ProviderId], [CourseTitle], [LearningAimRefId], [QualificationLevelId], [WhenNoLarQualificationTypeId])
GO

CREATE INDEX [IX_Course_RecordStatusId_Inc2] ON [dbo].[Snapshot_Course] ([Period], [RecordStatusId]) INCLUDE ([ProviderId], [AddedByApplicationId], [LearningAimRefId])
GO
