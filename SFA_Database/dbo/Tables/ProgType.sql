CREATE TABLE [dbo].[ProgType]
(
	[ProgTypeId] INT NOT NULL PRIMARY KEY, 
    [ProgTypeDesc] NVARCHAR(70) NULL, 
    [ProgTypeDesc2] NVARCHAR(25) NULL, 
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL,
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(),
    [ModifiedDateTimeUtc] DATETIME NULL
)
