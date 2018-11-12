CREATE TABLE [dbo].[DfEEstablishmentStatus]
(
	[DfEEstablishmentStatusId]		INT				NOT NULL
	, [DfEEstablishmentStatusCode]	NVARCHAR(3)		NOT NULL
	, [DfEEstablishmentStatusName]	NVARCHAR(50)	NOT NULL
	, [DfEProviderStatusId]			INT				NULL
	, CONSTRAINT [PK_DfEEstablishmentStatus] PRIMARY KEY CLUSTERED ([DfEEstablishmentStatusId] ASC)
	, CONSTRAINT [FK_DfEEstablishmentStatus_DfEProviderStatus] FOREIGN KEY ([DfEProviderStatusId]) REFERENCES [dbo].[DfEProviderStatus] ([DfEProviderStatusId])
)
