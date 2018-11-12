CREATE TABLE [dbo].[Audit_Address]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [AddressId] INT NOT NULL, 
    [AddressLine1] NVARCHAR(110) NULL, 
    [AddressLine2] NVARCHAR(100) NULL, 
    [Town] NVARCHAR(75) NULL, 
    [County] NVARCHAR(75) NULL, 
    [Postcode] NVARCHAR(30) NOT NULL, 
    [ProviderRegionId] INT NULL, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL, 
	[Geography] as geography::STGeomFromText('Point(' + CAST([Longitude] AS VARCHAR(32)) + ' ' + CAST([Latitude] AS VARCHAR(32)) + ')',4326)
)

GO

CREATE INDEX [IX_Audit_Address_AddressId] ON [dbo].[Audit_Address] ([AddressId])
