CREATE TABLE [dbo].[Snapshot_Apprenticeship]
(
	[Period]			   VARCHAR(7)	   NOT NULL,
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
    [MarketingInformation] NVARCHAR(2000)  NULL, 
    [Url]                  NVARCHAR(255)   NULL, 
    [ContactTelephone]     NVARCHAR(30)    NULL, 
	[ContactEmail]         NVARCHAR(255)   NULL, 
	[ContactWebsite]       NVARCHAR(255)   NULL, 
    CONSTRAINT [PK_Snapshot_Apprenticeship] PRIMARY KEY CLUSTERED ([Period], [ApprenticeshipId] ASC)
);
GO

CREATE INDEX [IX_Snapshot_Apprenticeship_ProviderId] ON [dbo].[Snapshot_Apprenticeship] ([Period], [ProviderId]);
GO

CREATE INDEX [IX_Snapshot_Apprenticeship_RecordStatusId] ON [dbo].[Snapshot_Apprenticeship] ([Period], [RecordStatusId]);
GO

CREATE INDEX [IX_Snapshot_Apprenticeship_Standard] ON [dbo].[Snapshot_Apprenticeship] ([Period], [StandardCode], [Version]);
GO

CREATE INDEX [IX_Snapshot_Apprenticeship_Framework] ON [dbo].[Snapshot_Apprenticeship] ([Period], [FrameworkCode], [ProgType], [PathwayCode]);
