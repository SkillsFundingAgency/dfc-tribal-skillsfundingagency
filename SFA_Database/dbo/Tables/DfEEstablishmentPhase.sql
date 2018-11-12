CREATE TABLE [dbo].[DfEEstablishmentPhase]
(
	[DfEEstablishmentPhaseId]		INT				NOT NULL
	, [DfEEstablishmentPhaseCode]	NVARCHAR(3)		NOT NULL
	, [DfEEstablishmentPhaseName]	NVARCHAR(50)	NOT NULL
	, CONSTRAINT [PK_DfEEstablishmentPhase] PRIMARY KEY CLUSTERED ([DfEEstablishmentPhaseId] ASC)
)
