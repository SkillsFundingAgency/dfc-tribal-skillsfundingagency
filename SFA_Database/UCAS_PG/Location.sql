CREATE TABLE [UCAS_PG].[Location]
(
	[LocationId] INT NOT NULL PRIMARY KEY, 
    [ProviderId] INT NOT NULL, 
    [LocationName] NVARCHAR(150) NOT NULL, 
    [Address1] NVARCHAR(100) NULL,
    [Address2] NVARCHAR(100) NULL,
    [Address3] NVARCHAR(100) NULL,
    [Address4] NVARCHAR(100) NULL,
    [Postcode] NVARCHAR(10) NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL, 
    [CreatedByUserid] NVARCHAR(128) NOT NULL
)

GO

CREATE INDEX [IX_Location_Provider] ON [UCAS_PG].[Location] ([ProviderId])
