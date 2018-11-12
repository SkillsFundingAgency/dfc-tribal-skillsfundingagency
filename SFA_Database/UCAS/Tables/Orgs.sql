CREATE TABLE [UCAS].[Orgs]
(
	[OrgId] INT NOT NULL PRIMARY KEY, 
    [OrgName] NVARCHAR(150) NOT NULL, 
    [Address1] NVARCHAR(100) NULL, 
    [Address2] NVARCHAR(100) NULL, 
    [Address3] NVARCHAR(100) NULL, 
    [TownId] INT NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [Fax] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(100) NULL, 
    [Web] NVARCHAR(100) NULL, 
    [UKPRN] INT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CreatedByUserId] NVARCHAR(128) NOT NULL
)
