CREATE TABLE [dbo].[Standard]
(
	[StandardCode] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [StandardName] NVARCHAR(255) NOT NULL, 
	[StandardSectorCode] NVARCHAR(50) NULL,
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL, 
	[URLLink] NVARCHAR(1000) NULL,
	[SectorSubjectAreaTier1] DECIMAL(5,2) NULL,
	[SectorSubjectAreaTier2] DECIMAL(5,2) NULL,
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(),
    [ModifiedDateTimeUtc] DATETIME NULL, 
	[RecordStatusId] INT NOT NULL DEFAULT 2,
    [NotionalEndLevel] NVARCHAR(5) NULL, 
    [OtherBodyApprovalRequired] NVARCHAR(10) NULL, 
    PRIMARY KEY ([StandardCode], [Version]), 
    CONSTRAINT [FK_Standard_SectorSubjectAreaTier1] FOREIGN KEY ([SectorSubjectAreaTier1]) REFERENCES [SectorSubjectAreaTier1]([SectorSubjectAreaTier1Id]),
    CONSTRAINT [FK_Standard_SectorSubjectAreaTier2] FOREIGN KEY ([SectorSubjectAreaTier2]) REFERENCES [SectorSubjectAreaTier2]([SectorSubjectAreaTier2Id]), 
    CONSTRAINT [FK_Standard_StandardSectorCode] FOREIGN KEY ([StandardSectorCode]) REFERENCES [StandardSectorCode]([StandardSectorCodeId]),
	CONSTRAINT [FK_Standard_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]) 
)
