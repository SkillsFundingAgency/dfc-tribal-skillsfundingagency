CREATE TABLE [dbo].[DfEWsProviderStatus]
(
	[DfEWsProviderStatusId]		INT				NOT NULL
	, [DfEWsProviderStatusCode]	NVARCHAR(3)		NOT NULL
	, [DfEWsProviderStatusName]	NVARCHAR(50)	NOT NULL
	, CONSTRAINT [PK_DfEWsProviderStatus] PRIMARY KEY CLUSTERED ([DfEWsProviderStatusId] ASC)
)
