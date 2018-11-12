CREATE TABLE [dbo].[Snapshot_Address]
(
	[Period]			VARCHAR(7) NOT NULL,
	[AddressId] [int]	NOT NULL,
	[AddressLine1] [nvarchar](110) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[Town] [nvarchar](75) NULL,
	[County] [nvarchar](75) NULL,
	[Postcode] [nvarchar](30) NOT NULL,
	[ProviderRegionId] int NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Geography] as geography::STGeomFromText('Point(' + CAST([Longitude] AS VARCHAR(32)) + ' ' + CAST([Latitude] AS VARCHAR(32)) + ')',4326),
	CONSTRAINT [PK_Snapshot_Address] PRIMARY KEY CLUSTERED ( [Period], [AddressId] ASC )  
);
GO

CREATE INDEX [IX_Snapshot_Address_Postcode] ON [dbo].[Snapshot_Address] ([Period], [Postcode])
GO
