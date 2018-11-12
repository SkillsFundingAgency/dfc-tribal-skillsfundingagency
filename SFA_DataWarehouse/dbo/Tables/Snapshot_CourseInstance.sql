CREATE TABLE [dbo].[Snapshot_CourseInstance]
(
	[Period]					   VARCHAR(7)	   NOT NULL,
    [CourseInstanceId]             INT             NOT NULL,
    [CourseId]                     INT             NOT NULL,
    [RecordStatusId]               INT             NOT NULL,
    [ProviderOwnCourseInstanceRef] VARCHAR (255)   NULL,
    [OfferedByProviderId]          INT             NULL,
    [DisplayProviderId]            INT             NULL,
    [StudyModeId]                  INT             NULL,
    [AttendanceTypeId]             INT             NULL,
    [AttendancePatternId]          INT             NULL,
    [DurationUnit]                 INT             NULL,
    [DurationUnitId]			   INT             NULL,
    [DurationAsText]               NVARCHAR (150)  NULL,
    [StartDateDescription]         NVARCHAR (150)  NULL,
    [EndDate]                      DATE            NULL,
    [TimeTable]                    NVARCHAR (200)  NULL,
    [Price]                        DECIMAL (10, 2) NULL,
    [PriceAsText]                  NVARCHAR (1000) NULL,
    [AddedByApplicationId]         INT             NOT NULL,
    [LanguageOfInstruction]        NVARCHAR (100)  NULL,
    [LanguageOfAssessment]         NVARCHAR (100)  NULL,
    [ApplyFromDate]                DATE            NULL,
    [ApplyUntilDate]               DATE            NULL,
    [ApplyUntilText]               NVARCHAR (100)  NULL,
    [EnquiryTo]                    NVARCHAR (255)  NULL,
    [ApplyTo]                      NVARCHAR (255)  NULL,
    [Url]                          NVARCHAR (255)  NULL,
    [CanApplyAllYear]              BIT             NOT NULL,
    [CreatedByUserId]              NVARCHAR(128)   NOT NULL,
    [CreatedDateTimeUtc]           DATETIME        NOT NULL,
    [ModifiedByUserId]             NVARCHAR(128)   NULL,
    [ModifiedDateTimeUtc]          DATETIME        NULL,
    [PlacesAvailable]			   INT             NULL, 
    [BothOfferedByDisplayBySearched] BIT           NOT NULL DEFAULT 0, 
    [VenueLocationId]			   INT			   NULL, 
    [OfferedByOrganisationId]	   INT			   NULL, 
    [DisplayedByOrganisationId]    INT			   NULL, 
    [IsDfE] BIT NOT NULL DEFAULT 0, 
    [IsSFA] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Snapshot_CourseInstance] PRIMARY KEY CLUSTERED ([Period], [CourseInstanceId])
)
GO


CREATE INDEX [IX_CourseInstance_CourseId] ON [dbo].[Snapshot_CourseInstance] ([Period], [CourseId])
GO

CREATE INDEX [IX_CourseInstance_RecordStatusId] ON [dbo].[Snapshot_CourseInstance] ([Period], [RecordStatusId])
GO

CREATE INDEX [IX_CourseInstance_RecordStatusId_OfferedByOrganisationId] ON [dbo].[Snapshot_CourseInstance] ([Period], [RecordStatusId], [OfferedByOrganisationId]) INCLUDE ([CourseId])
GO

CREATE INDEX [IX_CourseInstance_RecordStatusId_DisplayedByOrganisationId] ON [dbo].[Snapshot_CourseInstance] ([Period], [RecordStatusId], [DisplayedByOrganisationId]) INCLUDE ([CourseId])
GO

