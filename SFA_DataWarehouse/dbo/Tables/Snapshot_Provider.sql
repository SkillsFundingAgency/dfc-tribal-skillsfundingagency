CREATE TABLE [dbo].[Snapshot_Provider]
(
	[Period]					VARCHAR(7)		NOT NULL,
    [ProviderId]                INT				NOT NULL,
    [ProviderName]              NVARCHAR (200)	NOT NULL,
    [ProviderNameAlias]         NVARCHAR (200)	NULL,
    [Loans24Plus]               BIT				CONSTRAINT [DF_Snapshot_Provider_24PlusLoans] DEFAULT (0) NOT NULL,
    [Ukprn]                     INT				NOT NULL,
    [UPIN]                      INT				NULL,
    [ProviderTypeId]            INT				NOT NULL,
    [RecordStatusId]            INT				CONSTRAINT [DF_Snapshot_Provider_IsDeleted] DEFAULT (0) NOT NULL,
    [CreatedByUserId]           NVARCHAR(128)	NOT NULL,
    [CreatedDateTimeUtc]        DATETIME		NOT NULL DEFAULT GetUTCDate(),
    [ModifiedByUserId]          NVARCHAR(128)	NULL,
    [ModifiedDateTimeUtc]       DATETIME		NULL,
    [ProviderRegionId]          INT				NULL,
    [IsContractingBody]         BIT				CONSTRAINT [DF_Snapshot_Provider_IsContractingBody] DEFAULT (0) NOT NULL,
    [ProviderTrackingUrl]       NVARCHAR (255)	NULL,
    [VenueTrackingUrl]          NVARCHAR (255)	NULL,
    [CourseTrackingUrl]         NVARCHAR (255)	NULL,
    [BookingTrackingUrl]        NVARCHAR (255)	NULL,
    [RelationshipManagerUserId] NVARCHAR (128)	NULL,
    [InformationOfficerUserId]  NVARCHAR (128)	NULL,
    [AddressId]					INT				NULL, 
    [Email]						NVARCHAR(255)	NULL, 
    [Website]					NVARCHAR(255)	NULL, 
    [Telephone]					NVARCHAR(30)	NULL, 
    [Fax]						NVARCHAR(30)	NULL, 
    [FeChoicesLearner]			DECIMAL(3, 1)	NULL, 
    [FeChoicesEmployer]			DECIMAL(3, 1)	NULL, 
    [FeChoicesDestination]		INT				NULL, 
    [FeChoicesUpdatedDateTimeUtc] DATETIME		NULL, 
    [QualityEmailsPaused]		BIT				NOT NULL DEFAULT 0, 
    [QualityEmailStatusId]		INT				NULL, 
	[TrafficLightEmailDateTimeUtc]	DATE		NULL,
    [DFE1619Funded]				BIT				NOT NULL DEFAULT 0,
	[SFAFunded]					BIT				NOT NULL DEFAULT 0,
	[DfENumber]					INT				NULL,
	[DfEUrn]					INT				NULL,
	[DfEProviderTypeId]			INT				NULL,
	[DfEProviderStatusId]		INT				NULL,
	[DfELocalAuthorityId]		INT				NULL,
	[DfERegionId]				INT				NULL,
	[DfEEstablishmentTypeId]	INT				NULL,
	[DfEEstablishmentNumber]	INT				NULL,
	[StatutoryLowestAge]		INT				NULL,
	[StatutoryHighestAge]		INT				NULL,
	[AgeRange]					VARCHAR(10)		NULL,
	[AnnualSchoolCensusLowestAge]	INT			NULL,
	[AnnualSchoolCensusHighestAge]	INT			NULL,
	[CompanyRegistrationNumber]	INT				NULL,
	[Uid]						INT				NULL,
	[SecureAccessId]			INT				NULL,
	[BulkUploadPending]			BIT				NOT NULL DEFAULT 0,
	[PublishData]				BIT				NOT NULL DEFAULT 1,
	[MarketingInformation]      NVARCHAR(900)  NULL, 
    [NationalApprenticeshipProvider] BIT NOT NULL DEFAULT 0, 
    [ApprenticeshipContract] BIT NOT NULL DEFAULT 0, 
    [PassedOverallQAChecks] BIT NOT NULL DEFAULT 0, 
    [DataReadyToQA] BIT NOT NULL DEFAULT 0, 
    [RoATPFFlag] BIT NOT NULL DEFAULT 0, 
    [LastAllDataUpToDateTimeUtc] DATETIME NULL, 
    [RoATPProviderTypeId] INT NULL, 
    [RoATPStartDate] DATE NULL, 
    [MarketingInformationUpdatedDateUtc] DATETIME NULL, 
    [IsTASOnly] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Snapshot_Provider] PRIMARY KEY CLUSTERED ([Period], [ProviderId] ASC)
);
GO

CREATE INDEX [IX_Snapshot_Provider_RecordStatusId] ON [dbo].[Snapshot_Provider] ([Period], [RecordStatusId]);
GO

CREATE INDEX [IX_Snapshot_Provider_RelationshipManagerUserId] ON [dbo].[Snapshot_Provider] ([Period], [RelationshipManagerUserId]);
GO

CREATE INDEX [IX_Snapshot_Provider_InformationOfficerUserId] ON [dbo].[Snapshot_Provider] ([Period], [InformationOfficerUserId]);
GO

CREATE INDEX [IX_Snapshot_Provider_RecordStatusId_Inc] ON [dbo].[Snapshot_Provider] ([Period], [RecordStatusId]) INCLUDE ([ProviderId], [IsContractingBody], [DFE1619Funded], [SFAFunded]);
GO

CREATE INDEX [IX_Snapshot_Provider_ProviderName_ProviderId] ON [dbo].[Snapshot_Provider] ([Period], [ProviderName],[ProviderId]);
GO

CREATE INDEX [IX_Snapshot_Provider_Ukprn_ProviderId] ON [dbo].[Snapshot_Provider] ([Period], [Ukprn],[ProviderId]);
GO

CREATE INDEX [IX_Snapshot_Provider_Ukprn_RecordStatusId] ON [dbo].[Snapshot_Provider] ([Period], [Ukprn], [RecordStatusId]);
GO

CREATE INDEX [IX_Snapshot_Provider_ApprenticeshipContract] ON [dbo].[Snapshot_Provider] ([ApprenticeshipContract]);
