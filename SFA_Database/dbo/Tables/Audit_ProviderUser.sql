CREATE TABLE [dbo].[Audit_ProviderUser]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [UserId] NVARCHAR(128) NOT NULL, 
    [ProviderId] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_ProviderUser_UserId_ProviderId] ON [dbo].[Audit_ProviderUser] ([UserId], [ProviderId])
