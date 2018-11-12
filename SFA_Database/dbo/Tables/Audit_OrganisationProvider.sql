CREATE TABLE [dbo].[Audit_OrganisationProvider]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [OrganisationId] INT NOT NULL, 
    [ProviderId] INT NOT NULL, 
    [IsRejected] BIT NOT NULL, 
    [IsAccepted] BIT NOT NULL, 
    [Reason] NVARCHAR(200) NULL, 
    [RespondedByUserId] NVARCHAR(128) NULL, 
    [RespondedByDateTimeUtc] DATETIME NULL, 
    [CanOrganisationEditProvider] BIT NOT NULL
)

GO

CREATE INDEX [IX_Audit_OrganisationProvider_OrganisationId_ProviderId] ON [dbo].[Audit_OrganisationProvider] ([OrganisationId], [ProviderId])
