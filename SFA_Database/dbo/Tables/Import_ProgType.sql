CREATE TABLE [dbo].[Import_ProgType]
(
	[ProgTypeId] INT NOT NULL PRIMARY KEY, 
    [ProgTypeDesc] NVARCHAR(70) NULL, 
    [ProgTypeDesc2] NVARCHAR(25) NULL, 
    [EffectiveFrom] DATETIME NULL, 
    [EffectiveTo] DATETIME NULL
)
