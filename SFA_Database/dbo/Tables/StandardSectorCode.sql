CREATE TABLE [dbo].[StandardSectorCode]
(
	[StandardSectorCodeId] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [StandardSectorCodeDesc] NVARCHAR(120) NULL, 
    [StandardSectorCodeDesc2] NVARCHAR(60) NULL, 
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL,
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(),
    [ModifiedDateTimeUtc] DATETIME NULL
)
