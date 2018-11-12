CREATE TABLE [dbo].[Framework]
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
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(),
    [ModifiedDateTimeUtc] DATETIME NULL, 
	[RecordStatusId] INT NOT NULL DEFAULT 2,
    PRIMARY KEY ([FrameworkCode], [ProgType], [PathwayCode]), 
    CONSTRAINT [FK_Framework_ProgType] FOREIGN KEY ([ProgType]) REFERENCES [ProgType]([ProgTypeId]),
    CONSTRAINT [FK_Framework_SectorSubjectAreaTier1] FOREIGN KEY ([SectorSubjectAreaTier1]) REFERENCES [SectorSubjectAreaTier1]([SectorSubjectAreaTier1Id]),
    CONSTRAINT [FK_Framework_SectorSubjectAreaTier2] FOREIGN KEY ([SectorSubjectAreaTier2]) REFERENCES [SectorSubjectAreaTier2]([SectorSubjectAreaTier2Id]),
	CONSTRAINT [FK_Framework_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]) 
)
