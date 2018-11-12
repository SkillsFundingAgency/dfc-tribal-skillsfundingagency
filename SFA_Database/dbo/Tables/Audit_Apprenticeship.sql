CREATE TABLE [dbo].[Audit_Apprenticeship]
(
	[AuditSeq]             INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation]       NVARCHAR NOT NULL, 
    [AuditDateUtc]         DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [ApprenticeshipId]     INT             NOT NULL,
    [ProviderId]           INT             NOT NULL,
    [CreatedByUserId]      NVARCHAR(128)   NOT NULL,
    [CreatedDateTimeUtc]   DATETIME        NOT NULL,
    [ModifiedByUserId]     NVARCHAR(128)   NULL,
    [ModifiedDateTimeUtc]  DATETIME        NULL,
    [AddedByApplicationId] INT             NOT NULL,
    [RecordStatusId]       INT             NOT NULL,
    [StandardCode]         INT             NULL,
	[Version]              INT             NULL,
    [FrameworkCode]        INT             NULL,
	[ProgType]             INT             NULL, 
    [PathwayCode]          INT             NULL, 
    [MarketingInformation] NVARCHAR(2000)   NULL, 
    [Url]                  NVARCHAR(255)   NULL, 
    [ContactTelephone]     NVARCHAR(30)    NULL, 
	[ContactEmail]         NVARCHAR(255)   NULL, 
	[ContactWebsite]       NVARCHAR(255)   NULL
)

GO

CREATE INDEX [IX_Audit_Apprenticeship_ApprenticeshipId] ON [dbo].[Audit_Apprenticeship] ([ApprenticeshipId])

GO


CREATE INDEX [IX_Audit_Apprenticeship_ProviderId] ON [dbo].[Audit_Apprenticeship] ([ProviderId])
