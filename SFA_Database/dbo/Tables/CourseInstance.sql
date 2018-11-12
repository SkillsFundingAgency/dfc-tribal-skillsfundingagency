CREATE TABLE [dbo].[CourseInstance] (
    [CourseInstanceId]             INT             IDENTITY (1, 1) NOT NULL,
    [CourseId]                     INT             NOT NULL,
    [RecordStatusId]               INT             NOT NULL,
    [ProviderOwnCourseInstanceRef] VARCHAR (255)    NULL,
    [OfferedByProviderId]          INT             NULL,
    [DisplayProviderId]            INT             NULL,
    [StudyModeId]                  INT             NULL,
    [AttendanceTypeId]             INT             NULL,
    [AttendancePatternId]          INT             NULL,
    [DurationUnit]                 INT             NULL,
    [DurationUnitId]           INT             NULL,
    [DurationAsText]               NVARCHAR (150)  NULL,
    [StartDateDescription]         NVARCHAR (150)  NULL,
    [EndDate]                      DATE            NULL,
    [TimeTable]                    NVARCHAR (200)  NULL,
    [Price]                        DECIMAL (10, 2) NULL,
    [PriceAsText]                  NVARCHAR (1000)  NULL,
    [AddedByApplicationId]         INT             NOT NULL,
    [LanguageOfInstruction]        NVARCHAR (100)  NULL,
    [LanguageOfAssessment]         NVARCHAR (100)  NULL,
    [ApplyFromDate]                DATE            NULL,
    [ApplyUntilDate]               DATE            NULL,
    [ApplyUntilText]                    NVARCHAR (100)  NULL,
    [EnquiryTo]                    NVARCHAR (255)  NULL,
    [ApplyTo]                      NVARCHAR (255)  NULL,
    [Url]                          NVARCHAR (255)  NULL,
    [CanApplyAllYear]              BIT             NOT NULL,
    [CreatedByUserId]              NVARCHAR(128)             NOT NULL,
    [CreatedDateTimeUtc]           DATETIME        NOT NULL,
    [ModifiedByUserId]             NVARCHAR(128)             NULL,
    [ModifiedDateTimeUtc]          DATETIME        NULL,
    [PlacesAvailable] INT NULL, 
    [BothOfferedByDisplayBySearched] BIT NOT NULL DEFAULT 0, 
    [VenueLocationId] INT NULL, 
    [OfferedByOrganisationId] INT NULL , 
    [DisplayedByOrganisationId] INT NULL , 
    CONSTRAINT [PK_CourseInstance] PRIMARY KEY CLUSTERED ([CourseInstanceId] ASC),
    CONSTRAINT [FK_CourseInstance_Application] FOREIGN KEY ([AddedByApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_CourseInstance_AttendancePattern] FOREIGN KEY ([AttendancePatternId]) REFERENCES [dbo].[AttendancePattern] ([AttendancePatternId]),
    CONSTRAINT [FK_CourseInstance_AttendanceType] FOREIGN KEY ([AttendanceTypeId]) REFERENCES [dbo].[AttendanceType] ([AttendanceTypeId]),
    CONSTRAINT [FK_CourseInstance_Course] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course] ([CourseId]),    
    CONSTRAINT [FK_CourseInstance_StudyType] FOREIGN KEY ([StudyModeId]) REFERENCES [dbo].[StudyMode] ([StudyModeId]), 
    CONSTRAINT [FK_CourseInstance_DurationUnit] FOREIGN KEY ([DurationUnitId]) REFERENCES [dbo].[DurationUnit] ([DurationUnitId]), 
    CONSTRAINT [FK_CourseInstance_VenueLocation] FOREIGN KEY ([VenueLocationId]) REFERENCES [VenueLocation]([VenueLocationId]), 
    CONSTRAINT [FK_CourseInstance_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId]), 
    CONSTRAINT [FK_CourseInstance_Provider_Offered] FOREIGN KEY ([OfferedByProviderId]) REFERENCES [Provider]([ProviderId]),  
	CONSTRAINT [FK_CourseInstance_Provider_Display] FOREIGN KEY ([DisplayProviderId]) REFERENCES [Provider]([ProviderId]), 
    CONSTRAINT [FK_CourseInstance_OfferedByOrganisation] FOREIGN KEY ([OfferedByOrganisationId]) REFERENCES [Organisation]([OrganisationId]),
	CONSTRAINT [FK_CourseInstance_DisplayByOrganisation] FOREIGN KEY ([DisplayedByOrganisationId]) REFERENCES [Organisation]([OrganisationId]),
    CONSTRAINT [FK_CourseInstance_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_CourseInstance_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id])
)
GO

CREATE TRIGGER [dbo].[Trigger_CourseInstance_Audit_InsertUpdate]
    ON [dbo].[CourseInstance]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstance (AuditOperation, CourseInstanceId, CourseId, RecordStatusId, ProviderOwnCourseInstanceRef, OfferedByProviderId, DisplayProviderId, StudyModeId, AttendanceTypeId, AttendancePatternId, DurationUnit, DurationUnitId, DurationAsText, StartDateDescription, EndDate, TimeTable, Price, PriceAsText, AddedByApplicationId, LanguageOfInstruction, LanguageOfAssessment, ApplyFromDate, ApplyUntilDate, ApplyUntilText, EnquiryTo, ApplyTo, Url, CanApplyAllYear, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, PlacesAvailable, BothOfferedByDisplayBySearched, VenueLocationId, OfferedByOrganisationId, DisplayedByOrganisationId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, CourseInstanceId, CourseId, RecordStatusId, ProviderOwnCourseInstanceRef, OfferedByProviderId, DisplayProviderId, StudyModeId, AttendanceTypeId, AttendancePatternId, DurationUnit, DurationUnitId, DurationAsText, StartDateDescription, EndDate, TimeTable, Price, PriceAsText, AddedByApplicationId, LanguageOfInstruction, LanguageOfAssessment, ApplyFromDate, ApplyUntilDate, ApplyUntilText, EnquiryTo, ApplyTo, Url, CanApplyAllYear, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, PlacesAvailable, BothOfferedByDisplayBySearched, VenueLocationId, OfferedByOrganisationId, DisplayedByOrganisationId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_CourseInstance_Audit_Delete]
    ON [dbo].[CourseInstance]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstance (AuditOperation, CourseInstanceId, CourseId, RecordStatusId, ProviderOwnCourseInstanceRef, OfferedByProviderId, DisplayProviderId, StudyModeId, AttendanceTypeId, AttendancePatternId, DurationUnit, DurationUnitId, DurationAsText, StartDateDescription, EndDate, TimeTable, Price, PriceAsText, AddedByApplicationId, LanguageOfInstruction, LanguageOfAssessment, ApplyFromDate, ApplyUntilDate, ApplyUntilText, EnquiryTo, ApplyTo, Url, CanApplyAllYear, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, PlacesAvailable, BothOfferedByDisplayBySearched, VenueLocationId, OfferedByOrganisationId, DisplayedByOrganisationId)
		(SELECT 'D', CourseInstanceId, CourseId, RecordStatusId, ProviderOwnCourseInstanceRef, OfferedByProviderId, DisplayProviderId, StudyModeId, AttendanceTypeId, AttendancePatternId, DurationUnit, DurationUnitId, DurationAsText, StartDateDescription, EndDate, TimeTable, Price, PriceAsText, AddedByApplicationId, LanguageOfInstruction, LanguageOfAssessment, ApplyFromDate, ApplyUntilDate, ApplyUntilText, EnquiryTo, ApplyTo, Url, CanApplyAllYear, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, PlacesAvailable, BothOfferedByDisplayBySearched, VenueLocationId, OfferedByOrganisationId, DisplayedByOrganisationId FROM deleted);
    END
GO

CREATE INDEX [IX_CourseInstance_CourseId] ON [dbo].[CourseInstance] ([CourseId])
GO

CREATE INDEX [IX_CourseInstance_RecordStatusId] ON [dbo].[CourseInstance] ([RecordStatusId])
GO

CREATE INDEX [IX_CourseInstance_RecordStatusId_OfferedByOrganisationId] ON [dbo].[CourseInstance] ([RecordStatusId], [OfferedByOrganisationId]) INCLUDE ([CourseId])
GO

CREATE INDEX [IX_CourseInstance_RecordStatusId_DisplayedByOrganisationId] ON [dbo].[CourseInstance] ([RecordStatusId], [DisplayedByOrganisationId]) INCLUDE ([CourseId])
GO

