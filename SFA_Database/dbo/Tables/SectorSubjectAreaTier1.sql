CREATE TABLE [dbo].[SectorSubjectAreaTier1]
(
	[SectorSubjectAreaTier1Id] DECIMAL(5, 2) NOT NULL PRIMARY KEY, 
    [SectorSubjectAreaTier1Desc] NVARCHAR(100) NULL,
    [SectorSubjectAreaTier1Desc2] NVARCHAR(100) NULL,
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(),
    [ModifiedDateTimeUtc] DATETIME NULL
)
