CREATE TABLE [dbo].[Audit_PublicAPIUser]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
	[PublicAPIUserId] UNIQUEIDENTIFIER NOT NULL, 
    [CompanyName] NVARCHAR(100) NOT NULL, 
    [Telephone] NVARCHAR(100) NULL, 
    [Email] NVARCHAR(100) NULL, 
    [ContactFirstName] NVARCHAR(100) NULL, 
    [ContactLastName] NVARCHAR(100) NULL, 
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [ModifiedByUserId] NVARCHAR(128) NULL, 
    [ModifiedDateTimeUtc] DATETIME NULL,
    [RecordStatusId] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_PublicAPIUser_PublicAPIUserId] ON [dbo].[Audit_PublicAPIUser] ([PublicAPIUserId])
