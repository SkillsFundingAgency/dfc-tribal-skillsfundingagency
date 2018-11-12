CREATE TABLE [UCAS_PG].[Provider]
(
	[ProviderId] INT NOT NULL PRIMARY KEY,
	[UniqueId] NVARCHAR(50) NOT NULL, 
    [ProviderName] NVARCHAR(150) NOT NULL, 
    [Website] NVARCHAR(255) NULL, 
    [Address1] NVARCHAR(100) NULL, 
    [Address2] NVARCHAR(100) NULL, 
    [Address3] NVARCHAR(100) NULL, 
    [Address4] NVARCHAR(100) NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [ContactTitle] NVARCHAR(100) NULL, 
    [ContactEmail] NVARCHAR(255) NULL, 
    [ContactPhone] NVARCHAR(50) NULL, 
    [ContactFax] NVARCHAR(50) NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL, 
    [CreatedByUserId] NVARCHAR(128) NOT NULL,

)

GO

CREATE UNIQUE INDEX [IX_Provider_UniqueId] ON [UCAS_PG].[Provider] ([UniqueId])
