CREATE TABLE [dbo].[Import_SectorSubjectAreaTier2]
(
	[SectorSubjectAreaTier2Id] DECIMAL(5, 2) NOT NULL PRIMARY KEY, 
    [SectorSubjectAreaTier2Desc] NVARCHAR(100) NULL,
    [SectorSubjectAreaTier2Desc2] NVARCHAR(100) NULL,
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL
)
