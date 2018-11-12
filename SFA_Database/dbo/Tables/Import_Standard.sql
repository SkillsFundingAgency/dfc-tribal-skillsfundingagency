CREATE TABLE [dbo].[Import_Standard]
(
	[StandardCode] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [StandardName] NVARCHAR(255) NOT NULL, 
	[StandardSectorCode] NVARCHAR(100) NULL,
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL, 
	[URLLink] NVARCHAR(1000) NULL,
	[SectorSubjectAreaTier1] DECIMAL(5,2) NULL,
	[SectorSubjectAreaTier2] DECIMAL(5,2) NULL,
    [NotionalEndLevel] NVARCHAR(5) NULL, 
    [OtherBodyApprovalRequired] NVARCHAR(10) NULL, 
    PRIMARY KEY ([StandardCode], [Version])
)

