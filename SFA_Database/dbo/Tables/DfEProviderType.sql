CREATE TABLE [dbo].[DfEProviderType]
(
	[DfEProviderTypeId]		INT				NOT NULL
	, [DfEProviderTypeCode]	NVARCHAR(3)		NOT NULL
	, [DfEProviderTypeName]	NVARCHAR(50)	NOT NULL
	, CONSTRAINT [PK_DfEProviderType] PRIMARY KEY CLUSTERED ([DfEProviderTypeId] ASC)
)
