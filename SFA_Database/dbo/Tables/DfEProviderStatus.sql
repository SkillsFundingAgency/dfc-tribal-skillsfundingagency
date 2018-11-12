CREATE TABLE [dbo].[DfEProviderStatus]
(
	[DfEProviderStatusId]		INT				NOT NULL
	, [DfEProviderStatusCode]	NVARCHAR(3)		NOT NULL
	, [DfEProviderStatusName]	NVARCHAR(50)	NOT NULL
    , [DfEWsProviderStatusId]	INT				NOT NULL
	, CONSTRAINT [PK_DfEProviderStatus] PRIMARY KEY CLUSTERED ([DfEProviderStatusId] ASC)
	, CONSTRAINT [FK_DfEProviderStatus_DfEWsProviderStatus] FOREIGN KEY ([DfEWsProviderStatusId]) REFERENCES [dbo].[DfEWsProviderStatus] ([DfEWsProviderStatusId])
)
