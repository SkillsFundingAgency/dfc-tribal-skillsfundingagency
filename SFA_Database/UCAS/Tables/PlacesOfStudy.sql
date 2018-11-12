CREATE TABLE [UCAS].[PlacesOfStudy]
(
	[PlaceOfStudyId] INT NOT NULL PRIMARY KEY, 
    [OrgId] INT NOT NULL, 
    [PlaceOfStudy] NVARCHAR(150) NOT NULL, 
    [PlaceOfStudyDescription] NVARCHAR(2000) NULL, 
    [Address1] NVARCHAR(100) NULL, 
    [Address2] NVARCHAR(100) NULL, 
    [Address3] NVARCHAR(100) NULL, 
    [TownId] INT NULL, 
    [Postcode] NVARCHAR(10) NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CreatedByUserId] NVARCHAR(128) NOT NULL  
)

GO

CREATE INDEX [IX_PlacesOfStudy_OrgId] ON [UCAS].[PlacesOfStudy] ([OrgId])
