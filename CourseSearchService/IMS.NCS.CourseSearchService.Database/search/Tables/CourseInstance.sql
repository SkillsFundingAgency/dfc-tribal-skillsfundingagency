﻿CREATE TABLE [search].[CourseInstance] (
    [CourseInstanceId]               INT             NOT NULL,
    [ProviderOwnCourseInstanceRef]   NVARCHAR (255)  NULL,
    [Price]                          DECIMAL (10, 2) NULL,
    [PriceAsText]                    NVARCHAR (1000)  NULL,
    [DurationUnitId]                 INT             NULL,
    [DurationUnitBulkUploadRef]      NVARCHAR (10)   NULL,
    [DurationUnitName]				 NVARCHAR (50)   NULL,
    [DurationAsText]                 NVARCHAR (150)  NULL,
    [StartDateDescription]           NVARCHAR (150)  NULL,
    [EndDate]                        DATE            NULL,
    [StudyModeBulkUploadRef]         NVARCHAR (10)   NOT NULL,
    [StudyModeName]					 NVARCHAR (50)   NULL,
    [AttendanceModeBulkUploadRef]    NVARCHAR (10)   NOT NULL,
    [AttendanceModeName]			 NVARCHAR (50)   NULL,
    [AttendancePatternBulkUploadRef] NVARCHAR (10)   NOT NULL,
    [AttendancePatternName]			 NVARCHAR (50)   NULL,
    [LanguageOfInstruction]          NVARCHAR (100)  NULL,
    [LanguageOfAssessment]           NVARCHAR (100)  NULL,
    [PlacesAvailable]                INT             NULL,
    [EnquiryTo]                      NVARCHAR (255)  NULL,
    [ApplyTo]                        NVARCHAR (255)  NULL,
    [ApplyFromDate]                  DATE            NULL,
    [ApplyUntilDate]                 DATE            NULL,
    [ApplyUntilText]                 NVARCHAR (100)  NULL,
    [Url]                            NVARCHAR (511)  NULL,
    [TimeTable]                      NVARCHAR (200)  NULL,
    [CourseId]                       INT             NOT NULL,
    [VenueId]                        INT             NULL,
    [CanApplyAllYear]                BIT             NOT NULL,
    [RegionName]                     NVARCHAR (100)  NOT NULL,
    [CreatedDateTimeUtc]             DATETIME        NOT NULL,
    [ModifiedDateTimeUtc]            DATETIME        NULL,
    [RecordStatusId]                 INT             NOT NULL,
    [CreatedByUserId]                NVARCHAR (128)  NOT NULL,
    [ModifiedByUserId]               NVARCHAR (128)  NULL,
    [CourseInstanceSummary]          NVARCHAR (1500) NULL,
    [ProviderRegionId]               INT             NULL,
    [ApplicationId]                  INT             NOT NULL,
    [OfferedByOrganisationId]        INT             NULL,
    [OfferedByProviderId]            INT             NULL,
    [StartDate]                      DATETIME        NULL,
    [A10Codes]						 NVARCHAR(95)	 NULL, 
	[DfEFunded]						 BIT			 NULL,
	[VenueLocationId]				 INT			 NULL,
    CONSTRAINT [PK_CourseInstance] PRIMARY KEY CLUSTERED ([CourseInstanceId] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_VenueId]
    ON [search].[CourseInstance]([VenueId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CourseId]
    ON [search].[CourseInstance]([CourseId] ASC);

