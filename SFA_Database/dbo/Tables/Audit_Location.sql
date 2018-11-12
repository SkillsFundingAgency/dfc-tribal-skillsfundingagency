CREATE TABLE [dbo].[Audit_Location]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [LocationId]   INT NOT NULL,
	[ProviderId]   INT NOT NULL,
	[ProviderOwnLocationRef] NVARCHAR (255)  NULL,
	[LocationName] NVARCHAR (255)  NULL,
	[AddressId] INT NOT NULL,
    [Telephone]     NVARCHAR(30)    NULL, 
	[Email]         NVARCHAR(255)   NULL, 
	[Website]       NVARCHAR(255)   NULL,
    [RecordStatusId] INT NULL, 
	[CreatedByUserId]     NVARCHAR(128)             NOT NULL,
    [CreatedDateTimeUtc]  DATETIME        NOT NULL,
    [ModifiedByUserId]    NVARCHAR(128)             NULL,
    [ModifiedDateTimeUtc] DATETIME        NULL,
    [BulkUploadLocationId] NVARCHAR(255) NULL
)

GO

CREATE INDEX [IX_Audit_Location_LocationId] ON [dbo].[Audit_Location] ([LocationId])
