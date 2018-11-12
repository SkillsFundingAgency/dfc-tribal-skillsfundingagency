CREATE TABLE [dbo].[Import_StandardSectorCode]
(
	[StandardSectorCodeId] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [StandardSectorCodeDesc] NVARCHAR(120) NULL, 
    [StandardSectorCodeDesc2] NVARCHAR(60) NULL, 
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL
)
