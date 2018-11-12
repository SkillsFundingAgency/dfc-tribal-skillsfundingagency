CREATE TABLE [dbo].[Import_Framework]
(
	[FrameworkCode] INT NOT NULL , 
    [ProgType] INT NOT NULL, 
    [PathwayCode] INT NOT NULL, 
	[PathwayName] NVARCHAR(2000) NULL,
	[NasTitle] NVARCHAR(2000) NULL,
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL, 
	[SectorSubjectAreaTier1] DECIMAL(5,2) NULL,
	[SectorSubjectAreaTier2] DECIMAL(5,2) NULL,
    PRIMARY KEY ([FrameworkCode], [ProgType], [PathwayCode])
)
