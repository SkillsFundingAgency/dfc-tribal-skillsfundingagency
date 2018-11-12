CREATE TABLE [dbo].[Audit_OrganisationUser]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [UserId] NVARCHAR(128) NOT NULL, 
    [OrganisationId] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_OrganisationUser_UserId_OrganisationId] ON [dbo].[Audit_OrganisationUser] ([UserId], [OrganisationId])
