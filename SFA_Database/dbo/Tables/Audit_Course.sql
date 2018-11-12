CREATE TABLE [dbo].[Audit_Course]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CourseId] INT NOT NULL, 
    [ProviderId] INT NOT NULL, 
    [CourseTitle] NVARCHAR(255) NOT NULL, 
    [CourseSummary] NVARCHAR(2000) NULL, 
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL, 
    [ModifiedByUserId] NVARCHAR(128) NULL, 
    [ModifiedDateTimeUtc] DATETIME NULL, 
    [AddedByApplicationId] INT NOT NULL, 
    [RecordStatusId] INT NOT NULL, 
    [LearningAimRefId] VARCHAR(10) NULL, 
    [QualificationLevelId] INT NULL, 
    [EntryRequirements] NVARCHAR(4000) NOT NULL, 
    [ProviderOwnCourseRef] NVARCHAR(255) NULL, 
    [Url] NVARCHAR(255) NULL, 
    [BookingUrl] NVARCHAR(255) NULL, 
    [AssessmentMethod] NVARCHAR(4000) NULL, 
    [EquipmentRequired] NVARCHAR(4000) NULL, 
    [WhenNoLarQualificationTypeId] INT NULL, 
    [WhenNoLarQualificationTitle] NVARCHAR(255) NULL, 
    [AwardingOrganisationName] NVARCHAR(150) NULL, 
    [UcasTariffPoints] INT NULL
)

GO

CREATE INDEX [IX_Audit_Course_CourseId] ON [dbo].[Audit_Course] ([CourseId])

GO
