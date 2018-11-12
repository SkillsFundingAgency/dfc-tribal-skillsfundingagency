CREATE TABLE [dbo].[Address] (
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[AddressLine1] [nvarchar](110) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[Town] [nvarchar](75) NULL,
	[County] [nvarchar](75) NULL,
	[Postcode] [nvarchar](30) NOT NULL,
	[ProviderRegionId] int NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Geography] as geography::STGeomFromText('Point(' + CAST([Longitude] AS VARCHAR(32)) + ' ' + CAST([Latitude] AS VARCHAR(32)) + ')',4326),
	CONSTRAINT [FK_Address_ProviderRegion] FOREIGN KEY ([ProviderRegionId]) REFERENCES [dbo].[ProviderRegion] ([ProviderRegionId]),
	CONSTRAINT [PK_Address_computed] PRIMARY KEY CLUSTERED ( [AddressId] ASC )  
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TRIGGER [dbo].[Trigger_Address_InsertUpdate]
    ON [dbo].[Address]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Address (AuditOperation, AddressId, AddressLine1, AddressLine2, Town, County, Postcode, ProviderRegionId, Latitude, Longitude)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, AddressId, AddressLine1, AddressLine2, Town, County, Postcode, ProviderRegionId, Latitude, Longitude FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_Address_Delete]
    ON [dbo].[Address]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Address (AuditOperation, AddressId, AddressLine1, AddressLine2, Town, County, Postcode, ProviderRegionId, Latitude, Longitude)
		(SELECT 'D', AddressId, AddressLine1, AddressLine2, Town, County, Postcode, ProviderRegionId, Latitude, Longitude FROM deleted);
    END
GO

CREATE INDEX [IX_Address_Postcode] ON [dbo].[Address] ([Postcode])
GO
