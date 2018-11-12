CREATE TABLE [dbo].[Organisation] (
    [OrganisationId]      INT            NOT NULL IDENTITY,
    [UKPRN]               INT            NULL,
    [OrganisationName]    VARCHAR (100)  NOT NULL,
    [OrganisationAlias]   VARCHAR (100)  NULL,
    [UPIN]                INT            NULL,
	[Loans24Plus]         BIT            CONSTRAINT [DF_Organisation_24PlusLoans] DEFAULT ((0)) NOT NULL,
    [OrganisationTypeId]  INT            NOT NULL,
    [Email]               NVARCHAR (255) NULL,
    [Website]             NVARCHAR (255) NULL,
    [Phone]               NVARCHAR (35)  NULL,
    [Fax]                 NVARCHAR (35)  NULL,
    [IsContractingBody]   BIT            CONSTRAINT [DF_Organisation_IsContractingBody] DEFAULT ((0)) NOT NULL,
    [CreatedByUserId]     NVARCHAR(128)            NOT NULL,
    [CreatedDateTimeUtc]  DATETIME       NOT NULL,
    [ModifiedByUserId]    NVARCHAR(128)            NULL,
    [ModifiedDateTimeUtc] DATETIME       NULL,
    [RecordStatusId]      INT            NOT NULL,
    [RelationshipManagerUserId] NVARCHAR (128)  NULL,
    [InformationOfficerUserId]  NVARCHAR (128)  NULL,
    [AddressId] INT NOT NULL, 
	[QualityEmailsPaused] BIT NOT NULL DEFAULT 0, 
    [QualityEmailStatusId] INT NULL, 
	[BulkUploadPending]	BIT				NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Organisation] PRIMARY KEY CLUSTERED ([OrganisationId] ASC),
    CONSTRAINT [FK_Organisation_OrganisationType] FOREIGN KEY ([OrganisationTypeId]) REFERENCES [dbo].[OrganisationType] ([OrganisationTypeId]),
    CONSTRAINT [FK_Organisation_Ukrlp] FOREIGN KEY ([Ukprn]) REFERENCES [Ukrlp] ([Ukprn]), 
    CONSTRAINT [FK_Organisation_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]), 
    CONSTRAINT [FK_Organisation_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([AddressId]),
	CONSTRAINT [FK_Organisation_RelationshipManager] FOREIGN KEY ([RelationshipManagerUserId]) REFERENCES [AspNetUsers]([Id]),
	CONSTRAINT [FK_Organisation_InformationOfficer] FOREIGN KEY ([InformationOfficerUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Organisation_QualityEmailStatus] FOREIGN KEY ([QualityEmailStatusId]) REFERENCES [QualityEmailStatus]([QualityEmailStatusId]),
    CONSTRAINT [FK_Organisation_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_Organisation_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id])
)
GO

CREATE TRIGGER [dbo].[Trigger_Organisation_InsertUpdate]
    ON [dbo].[Organisation]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Organisation (AuditOperation, OrganisationId, UKPRN, OrganisationName, OrganisationAlias, UPIN, Loans24Plus, OrganisationTypeId, Email, Website, Phone, Fax, IsContractingBody, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId, RelationshipManagerUserId, InformationOfficerUserId, AddressId, QualityEmailsPaused, QualityEmailStatusId, [BulkUploadPending])
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, OrganisationId, UKPRN, OrganisationName, OrganisationAlias, UPIN, Loans24Plus, OrganisationTypeId, Email, Website, Phone, Fax, IsContractingBody, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId, RelationshipManagerUserId, InformationOfficerUserId, AddressId, QualityEmailsPaused, QualityEmailStatusId, [BulkUploadPending] FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_Organisation_Delete]
    ON [dbo].[Organisation]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Organisation (AuditOperation, OrganisationId, UKPRN, OrganisationName, OrganisationAlias, UPIN, Loans24Plus, OrganisationTypeId, Email, Website, Phone, Fax, IsContractingBody, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId, RelationshipManagerUserId, InformationOfficerUserId, AddressId, QualityEmailsPaused, QualityEmailStatusId, [BulkUploadPending])
		(SELECT 'D', OrganisationId, UKPRN, OrganisationName, OrganisationAlias, UPIN, Loans24Plus, OrganisationTypeId, Email, Website, Phone, Fax, IsContractingBody, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId, RelationshipManagerUserId, InformationOfficerUserId, AddressId, QualityEmailsPaused, QualityEmailStatusId, [BulkUploadPending] FROM deleted);
    END
GO

CREATE INDEX [IX_Organisation_RecordStatusId] ON [dbo].[Organisation] ([RecordStatusId])
GO

CREATE INDEX [IX_Organisation_RelationshipManagerUserId] ON [dbo].[Organisation] ([RelationshipManagerUserId])
GO

CREATE INDEX [IX_Organisation_InformationOfficerUserId] ON [dbo].[Organisation] ([InformationOfficerUserId])
GO

CREATE INDEX [IX_Organisation_RecordStatusId_Inc] ON [dbo].[Organisation] ([RecordStatusId]) INCLUDE ([OrganisationId], [IsContractingBody])
GO

CREATE INDEX [IX_Organisation_OrganisationName_OrganisationId] ON [dbo].[Organisation] ([OrganisationName],[OrganisationId])
GO

CREATE INDEX [IX_Organisation_Ukprn_OrganisationId] ON [dbo].[Organisation] ([Ukprn], [OrganisationId])
GO

CREATE INDEX [IX_Organisation_Ukprn_RecordStatusId] ON [dbo].[Organisation] ([Ukprn], [RecordStatusId])
GO
