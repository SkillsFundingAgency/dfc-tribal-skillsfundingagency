CREATE TABLE [dbo].[DfERegion]
(
	[DfERegionId]		INT				NOT NULL
	, [DfERegionCode]	NVARCHAR(3)		NOT NULL
	, [DfERegionName]	NVARCHAR(50)	NOT NULL
	, CONSTRAINT [PK_DfERegion] PRIMARY KEY CLUSTERED ([DfERegionId] ASC)
)
