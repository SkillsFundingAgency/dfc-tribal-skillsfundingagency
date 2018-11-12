CREATE TABLE [dbo].[DfEEstablishmentType]
(
	[DfEEstablishmentTypeId]		INT				NOT NULL
	, [DfEEstablishmentTypeCode]	NVARCHAR(3)		NOT NULL
	, [DfEEstablishmentTypeName]	NVARCHAR(100)	NOT NULL
	, CONSTRAINT [PK_DfEEstablishmentType] PRIMARY KEY CLUSTERED ([DfEEstablishmentTypeId] ASC)
)
