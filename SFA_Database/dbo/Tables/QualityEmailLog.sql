CREATE TABLE [dbo].[QualityEmailLog]
(
	[QualityEmailLogId]				INT			NOT NULL IDENTITY
	, [ProviderId]					INT			NOT NULL
	, [ModifiedDateTimeUtc]			DATETIME	NULL
	, [TrafficLightStatusId]		INT			NOT NULL DEFAULT(0)
    , [SFAFunded]					BIT			NOT NULL
    , [DFE1619Funded]				BIT			NOT NULL
	, [QualityEmailsPaused]			BIT			NOT NULL
	, [HasValidRecipients]			BIT			NOT NULL
	, [EmailTemplateId]				INT			NULL
	, [EmailDateTimeUtc]			DATETIME	NULL
	, [NextEmailTemplateId]			INT			NULL
    , [NextEmailDateTimeUtc]		DATETIME	NULL
	, [CreatedDateTimeUtc]			DATETIME	NULL
	, CONSTRAINT [PK_QualityEmailLog] PRIMARY KEY CLUSTERED ([QualityEmailLogId] ASC)
    , CONSTRAINT [FK_QualityEmailLog_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId])
    , CONSTRAINT [FK_QualityEmailLog_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [dbo].[EmailTemplate] ([EmailTemplateId])
    , CONSTRAINT [FK_QualityEmailLog_NextEmailTemplate] FOREIGN KEY ([NextEmailTemplateId]) REFERENCES [dbo].[EmailTemplate] ([EmailTemplateId])
)
GO

CREATE INDEX [IX_QualityEmailLog_ProviderEmailDateTimeUtc] ON [dbo].[QualityEmailLog] ([ProviderId], [EmailDateTimeUtc])
GO
