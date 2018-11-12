CREATE TABLE dbo.[Provider] (
    [ProviderId]                INT				IDENTITY (1, 1) NOT NULL,
    [ProviderName]              NVARCHAR (200)	NOT NULL,
    [ProviderNameAlias]         NVARCHAR (200)	NULL,
    [Loans24Plus]               BIT				CONSTRAINT [DF_Table_1_24PlusLoans] DEFAULT ((0)) NOT NULL,
    [Ukprn]                     INT				NOT NULL,
    [UPIN]                      INT				NULL,
    [ProviderTypeId]            INT				NOT NULL,
    [RecordStatusId]            INT				CONSTRAINT [DF_Provider_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedByUserId]           NVARCHAR(128)	NOT NULL,
    [CreatedDateTimeUtc]        DATETIME		NOT NULL DEFAULT GetUTCDate(),
    [ModifiedByUserId]          NVARCHAR(128)	NULL,
    [ModifiedDateTimeUtc]       DATETIME		NULL,
    [ProviderRegionId]          INT				NULL,
    [IsContractingBody]         BIT				CONSTRAINT [DF_Provider_IsContractingBody] DEFAULT ((0)) NOT NULL,
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
    [TradingName] NVARCHAR(255) NULL, 
    [IsTASOnly] AS [dbo].[GetProviderIsTASOnly]([ProviderId], [RoATPFFlag]), 
    [MaxLocations] INT NULL, 
    [MaxLocationsUserId] NVARCHAR(128) NULL, 
    [MaxLocationsDateTimeUtc] DATETIME NULL, 
    [TASRefreshOverride] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Provider] PRIMARY KEY CLUSTERED ([ProviderId] ASC),
    CONSTRAINT [FK_Provider_ProviderType] FOREIGN KEY ([ProviderTypeId]) REFERENCES dbo.[ProviderType] ([ProviderTypeId]),
    CONSTRAINT [FK_Provider_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES dbo.[RecordStatus] ([RecordStatusId]),
    CONSTRAINT [FK_Provider_ProviderRegion] FOREIGN KEY ([ProviderRegionId]) REFERENCES dbo.[ProviderRegion] ([ProviderRegionId]), 
    CONSTRAINT [FK_Provider_Ukrlp] FOREIGN KEY ([Ukprn]) REFERENCES [Ukrlp]([Ukprn]), 
    CONSTRAINT [FK_Provider_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([AddressId]),
	CONSTRAINT [FK_Provider_RelationshipManager] FOREIGN KEY ([RelationshipManagerUserId]) REFERENCES [AspNetUsers]([Id]),
	CONSTRAINT [FK_Provider_InformationOfficer] FOREIGN KEY ([InformationOfficerUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Provider_QualityEmailStatus] FOREIGN KEY ([QualityEmailStatusId]) REFERENCES [QualityEmailStatus]([QualityEmailStatusId]),
	CONSTRAINT [FK_Provider_DfEProviderType] FOREIGN KEY ([DfEProviderTypeId]) REFERENCES [DfEProviderType]([DfEProviderTypeId]),
	CONSTRAINT [FK_Provider_DfEProviderStatus] FOREIGN KEY ([DfEProviderStatusId]) REFERENCES [DfEProviderStatus]([DfEProviderStatusId]),
	CONSTRAINT [FK_Provider_DfELocalAuthority] FOREIGN KEY ([DfELocalAuthorityId]) REFERENCES [DfELocalAuthority]([DfELocalAuthorityId]),
	CONSTRAINT [FK_Provider_DfERegion] FOREIGN KEY ([DfERegionId]) REFERENCES [DfERegion]([DfERegionId]),
	CONSTRAINT [FK_Provider_DfEEstablishmentType] FOREIGN KEY ([DfEEstablishmentTypeId]) REFERENCES [DfEEstablishmentType]([DfEEstablishmentTypeId]),
    CONSTRAINT [FK_Provider_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_Provider_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_Provider_RoATPProviderType] FOREIGN KEY ([RoATPProviderTypeId]) REFERENCES [RoATPProviderType]([RoATPProviderTypeId]), 
    CONSTRAINT [FK_Provider_MaxLocationsUserId] FOREIGN KEY ([MaxLocationsUserId]) REFERENCES [AspNetUsers]([Id])
)
GO

CREATE TRIGGER dbo.[Trigger_Provider_InsertUpdate]
    ON dbo.[Provider]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Provider (AuditOperation, ProviderId, ProviderName, ProviderNameAlias, Loans24Plus, Ukprn, UPIN, ProviderTypeId, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, ProviderRegionId, IsContractingBody, ProviderTrackingUrl, VenueTrackingUrl, CourseTrackingUrl, BookingTrackingUrl, RelationshipManagerUserId, InformationOfficerUserId, AddressId, Email, Website, Telephone, Fax, FEChoicesLearner, FEChoicesEmployer, FEChoicesDestination, FEChoicesUpdatedDateTimeUtc, QualityEmailsPaused, QualityEmailStatusId, TrafficLightEmailDateTimeUtc, SFAFunded, DFE1619Funded, DfENumber, DfEUrn, DfEProviderTypeId, DfEProviderStatusId, DfELocalAuthorityId, DfERegionId, DfEEstablishmentTypeId, DfEEstablishmentNumber, StatutoryLowestAge, StatutoryHighestAge, AgeRange, AnnualSchoolCensusLowestAge, AnnualSchoolCensusHighestAge, CompanyRegistrationNumber, [Uid], [SecureAccessId], [BulkUploadPending], [PublishData], [MarketingInformation], [NationalApprenticeshipProvider], [ApprenticeshipContract], [PassedOverallQAChecks], [DataReadyToQA], [RoATPFFlag], [LastAllDataUpToDateTimeUtc], RoATPProviderTypeId, RoATPStartDate, MarketingInformationUpdatedDateUtc, TradingName, MaxLocations, MaxLocationsUserId, MaxLocationsDateTimeUtc)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, ProviderId, ProviderName, ProviderNameAlias, Loans24Plus, Ukprn, UPIN, ProviderTypeId, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, ProviderRegionId, IsContractingBody, ProviderTrackingUrl, VenueTrackingUrl, CourseTrackingUrl, BookingTrackingUrl, RelationshipManagerUserId, InformationOfficerUserId, AddressId, Email, Website, Telephone, Fax, FEChoicesLearner, FEChoicesEmployer, FEChoicesDestination, FEChoicesUpdatedDateTimeUtc, QualityEmailsPaused, QualityEmailStatusId, TrafficLightEmailDateTimeUtc, SFAFunded, DFE1619Funded, DfENumber, DfEUrn, DfEProviderTypeId, DfEProviderStatusId, DfELocalAuthorityId, DfERegionId, DfEEstablishmentTypeId, DfEEstablishmentNumber, StatutoryLowestAge, StatutoryHighestAge, AgeRange, AnnualSchoolCensusLowestAge, AnnualSchoolCensusHighestAge, CompanyRegistrationNumber, [Uid], [SecureAccessId], [BulkUploadPending], [PublishData], [MarketingInformation], [NationalApprenticeshipProvider], [ApprenticeshipContract], [PassedOverallQAChecks], [DataReadyToQA], [RoATPFFlag], [LastAllDataUpToDateTimeUtc], [RoATPProviderTypeId], RoATPStartDate, MarketingInformationUpdatedDateUtc, TradingName, MaxLocations, MaxLocationsUserId, MaxLocationsDateTimeUtc FROM inserted);
    END
GO

CREATE TRIGGER dbo.[Trigger_Provider_Delete]
    ON dbo.[Provider]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Provider (AuditOperation, ProviderId, ProviderName, ProviderNameAlias, Loans24Plus, Ukprn, UPIN, ProviderTypeId, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, ProviderRegionId, IsContractingBody, ProviderTrackingUrl, VenueTrackingUrl, CourseTrackingUrl, BookingTrackingUrl, RelationshipManagerUserId, InformationOfficerUserId, AddressId, Email, Website, Telephone, Fax, FEChoicesLearner, FEChoicesEmployer, FEChoicesDestination, FEChoicesUpdatedDateTimeUtc, QualityEmailsPaused, QualityEmailStatusId, TrafficLightEmailDateTimeUtc, SFAFunded, DFE1619Funded, DfENumber, DfEUrn, DfEProviderTypeId, DfEProviderStatusId, DfELocalAuthorityId, DfERegionId, DfEEstablishmentTypeId, DfEEstablishmentNumber, StatutoryLowestAge, StatutoryHighestAge, AgeRange, AnnualSchoolCensusLowestAge, AnnualSchoolCensusHighestAge, CompanyRegistrationNumber, [Uid], [SecureAccessId], [BulkUploadPending], [PublishData], [MarketingInformation], [NationalApprenticeshipProvider], [ApprenticeshipContract], [PassedOverallQAChecks], [DataReadyToQA], [RoATPFFlag], [LastAllDataUpToDateTimeUtc], RoATPProviderTypeId, RoATPStartDate, MarketingInformationUpdatedDateUtc, TradingName, MaxLocations, MaxLocationsUserId, MaxLocationsDateTimeUtc)
		(SELECT 'D', ProviderId, ProviderName, ProviderNameAlias, Loans24Plus, Ukprn, UPIN, ProviderTypeId, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, ProviderRegionId, IsContractingBody, ProviderTrackingUrl, VenueTrackingUrl, CourseTrackingUrl, BookingTrackingUrl, RelationshipManagerUserId, InformationOfficerUserId, AddressId, Email, Website, Telephone, Fax, FEChoicesLearner, FEChoicesEmployer, FEChoicesDestination, FEChoicesUpdatedDateTimeUtc, QualityEmailsPaused, QualityEmailStatusId, TrafficLightEmailDateTimeUtc, SFAFunded, DFE1619Funded, DfENumber, DfEUrn, DfEProviderTypeId, DfEProviderStatusId, DfELocalAuthorityId, DfERegionId, DfEEstablishmentTypeId, DfEEstablishmentNumber, StatutoryLowestAge, StatutoryHighestAge, AgeRange, AnnualSchoolCensusLowestAge, AnnualSchoolCensusHighestAge, CompanyRegistrationNumber, [Uid], [SecureAccessId], [BulkUploadPending], [PublishData], [MarketingInformation], [NationalApprenticeshipProvider], [ApprenticeshipContract], [PassedOverallQAChecks], [DataReadyToQA], [RoATPFFlag], [LastAllDataUpToDateTimeUtc], [RoATPProviderTypeId], RoATPStartDate, MarketingInformationUpdatedDateUtc, TradingName, MaxLocations, MaxLocationsUserId, MaxLocationsDateTimeUtc FROM deleted);
    END
GO

CREATE UNIQUE NONCLUSTERED INDEX [UQ_Provider_SecureAccessId] 
	ON dbo.[Provider](SecureAccessId) 
	WHERE [SecureAccessId] IS NOT NULL
GO

CREATE INDEX [IX_Provider_RecordStatusId] ON [dbo].[Provider] ([RecordStatusId])
GO

CREATE INDEX [IX_Provider_RelationshipManagerUserId] ON [dbo].[Provider] ([RelationshipManagerUserId])
GO

CREATE INDEX [IX_Provider_InformationOfficerUserId] ON [dbo].[Provider] ([InformationOfficerUserId])
GO

CREATE INDEX [IX_Provider_RecordStatusId_Inc] ON [dbo].[Provider] ([RecordStatusId]) INCLUDE ([ProviderId], [IsContractingBody], [DFE1619Funded], [SFAFunded])
GO

CREATE INDEX [IX_Provider_ProviderName_ProviderId] ON [dbo].[Provider] ([ProviderName],[ProviderId])
GO

CREATE INDEX [IX_Provider_Ukprn_ProviderId] ON [dbo].[Provider] ([Ukprn],[ProviderId])
GO

CREATE INDEX [IX_Provider_Ukprn_RecordStatusId] ON [dbo].[Provider] ([Ukprn], [RecordStatusId])
GO


CREATE INDEX [IX_Provider_ApprenticeshipContract] ON [dbo].[Provider] ([ApprenticeshipContract])
