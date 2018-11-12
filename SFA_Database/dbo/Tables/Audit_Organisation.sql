CREATE TABLE [dbo].[Audit_Organisation]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [OrganisationId] INT NOT NULL, 
    [UKPRN] INT NULL, 
    [OrganisationName] VARCHAR(100) NOT NULL, 
    [OrganisationAlias] VARCHAR(100) NULL, 
    [UPIN] INT NULL, 
    [Loans24Plus] BIT NOT NULL, 
    [OrganisationTypeId] INT NOT NULL, 
    [Email] NVARCHAR(255) NULL, 
    [Website] NVARCHAR(255) NULL, 
    [Phone] NVARCHAR(35) NULL, 
    [Fax] NVARCHAR(35) NULL, 
    [IsContractingBody] BIT NOT NULL, 
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL, 
    [ModifiedByUserId] NVARCHAR(128) NULL, 
    [ModifiedDateTimeUtc] DATETIME NULL, 
    [RecordStatusId] INT NOT NULL, 
    [RelationshipManagerUserId] NVARCHAR(128) NULL, 
    [InformationOfficerUserId] NVARCHAR(128) NULL, 
    [AddressId] INT NOT NULL, 
    [QualityEmailsPaused] BIT NOT NULL, 
    [QualityEmailStatusId] INT NULL,
	[BulkUploadPending] BIT NULL
)

GO

CREATE INDEX [IX_Audit_Organisation_OrganisationId] ON [dbo].[Audit_Organisation] ([OrganisationId])
