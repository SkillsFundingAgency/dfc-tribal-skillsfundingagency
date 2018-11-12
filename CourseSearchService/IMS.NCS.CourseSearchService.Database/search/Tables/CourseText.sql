CREATE TABLE [Search].[CourseText] (
    [CourseTextId]                   INT            IDENTITY (1, 1) NOT NULL,
    [CourseId]                       INT            NOT NULL,
    [CourseInstanceId]               INT            NOT NULL,
    [StudyModeBulkUploadRef]         NVARCHAR (10)  NOT NULL,
    [AttendanceModeBulkUploadRef]    NVARCHAR (10)  NOT NULL,
    [AttendancePatternBulkUploadRef] NVARCHAR (10)  NOT NULL,
    [QualificationTypeRef]           NVARCHAR (10)  NULL,
    [EastingMin]                     INT            NULL,
    [EastingMax]                     INT            NULL,
    [NorthingMin]                    INT            NULL,
    [NorthingMax]                    INT            NULL,
    [Easting]                        FLOAT (53)     NULL,
    [Northing]                       FLOAT (53)     NULL,
    [SearchText]                     NVARCHAR (700) NOT NULL,
    [NumberOfCourseInstances]        INT            NOT NULL,
    [ModifiedDateTimeUtc]            DATETIME       NOT NULL,
    [ProviderId]                     INT            NOT NULL,
    [LdcsCodes]                      NVARCHAR (50)  NOT NULL,
    [ProviderName]                   NVARCHAR (100) NOT NULL,
    [ApplyUntilDate]                 DATE           NULL,
    [ApplicationFundingStatus]       NVARCHAR (16)  NULL,
    [CanApplyAllYear]                BIT            NOT NULL,
    [SearchRegion]                   NVARCHAR (30)  NULL,
    [SearchTown]                     NVARCHAR (100) NOT NULL,
    [Postcode]                       NVARCHAR (30)  NOT NULL,
    CONSTRAINT [PK__CourseTe__00096723F1BFDBBA] PRIMARY KEY CLUSTERED ([CourseTextId] ASC)
);



