CREATE TABLE [dbo].[Course] (
    [CourseId]             INT             IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([CourseId] ASC),
    CONSTRAINT [FK_Course_Application] FOREIGN KEY ([AddedByApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_Course_LearningAim] FOREIGN KEY ([LearningAimRefId]) REFERENCES [dbo].[LearningAim] ([LearningAimRefId]),
    CONSTRAINT [FK_Course_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId]),
    CONSTRAINT [FK_Course_QualificationLevel] FOREIGN KEY ([QualificationLevelId]) REFERENCES [dbo].[QualificationLevel] ([QualificationLevelId]),
    CONSTRAINT [FK_Course_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]), 
    CONSTRAINT [FK_Course_QualificationType] FOREIGN KEY ([WhenNoLarQualificationTypeId]) REFERENCES [QualificationType]([QualificationTypeId]), 
    CONSTRAINT [FK_Course_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Course_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers]([Id])
);
GO

CREATE TRIGGER [dbo].[Trigger_Course_Audit_Delete]
    ON [dbo].[Course]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Course (AuditOperation, CourseId, ProviderId, CourseTitle, CourseSummary, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, LearningAimRefId, QualificationLevelId, EntryRequirements, ProviderOwnCourseRef, Url, BookingUrl, AssessmentMethod, EquipmentRequired, WhenNoLarQualificationTypeId, WhenNoLarQualificationTitle, AwardingOrganisationName, UcasTariffPoints)
		(SELECT 'D', CourseId, ProviderId, CourseTitle, CourseSummary, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, LearningAimRefId, QualificationLevelId, EntryRequirements, ProviderOwnCourseRef, Url, BookingUrl, AssessmentMethod, EquipmentRequired, WhenNoLarQualificationTypeId, WhenNoLarQualificationTitle, AwardingOrganisationName, UcasTariffPoints FROM deleted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_Course_Audit_InsertUpdate]
    ON [dbo].[Course]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Course (AuditOperation, CourseId, ProviderId, CourseTitle, CourseSummary, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, LearningAimRefId, QualificationLevelId, EntryRequirements, ProviderOwnCourseRef, Url, BookingUrl, AssessmentMethod, EquipmentRequired, WhenNoLarQualificationTypeId, WhenNoLarQualificationTitle, AwardingOrganisationName, UcasTariffPoints)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, CourseId, ProviderId, CourseTitle, CourseSummary, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, LearningAimRefId, QualificationLevelId, EntryRequirements, ProviderOwnCourseRef, Url, BookingUrl, AssessmentMethod, EquipmentRequired, WhenNoLarQualificationTypeId, WhenNoLarQualificationTitle, AwardingOrganisationName, UcasTariffPoints FROM inserted);
    END
GO

CREATE INDEX [IX_Course_ProviderId] ON [dbo].[Course] ([ProviderId])
GO

CREATE INDEX [IX_Course_RecordStatusId] ON [dbo].[Course] ([RecordStatusId])
GO

CREATE INDEX [IX_Course_WhenNoLarQualificationTypeId] ON [dbo].[Course] ([WhenNoLarQualificationTypeId])
GO

CREATE INDEX [IX_Course_LearningAimRefId] ON [dbo].[Course] ([LearningAimRefId])
GO

CREATE INDEX [IX_Course_RecordStatusId_Inc1] ON [dbo].[Course] ([RecordStatusId]) INCLUDE ([CourseId], [ProviderId], [CourseTitle], [LearningAimRefId], [QualificationLevelId], [WhenNoLarQualificationTypeId])
GO

CREATE INDEX [IX_Course_RecordStatusId_Inc2] ON [dbo].[Course] ([RecordStatusId]) INCLUDE ([ProviderId], [AddedByApplicationId], [LearningAimRefId])
GO
