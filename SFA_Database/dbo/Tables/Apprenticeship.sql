CREATE TABLE [dbo].[Apprenticeship] (
    [ApprenticeshipId]     INT             IDENTITY (1, 1) NOT NULL,
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
    [MarketingInformation] NVARCHAR(900)  NULL, 
    [Url]                  NVARCHAR(255)   NULL, 
    [ContactTelephone]     NVARCHAR(30)    NULL, 
	[ContactEmail]         NVARCHAR(255)   NULL, 
	[ContactWebsite]       NVARCHAR(255)   NULL, 
    CONSTRAINT [PK_Apprenticeship] PRIMARY KEY CLUSTERED ([ApprenticeshipId] ASC),
    CONSTRAINT [FK_Apprenticeship_Standard] FOREIGN KEY ([StandardCode],[Version]) REFERENCES [dbo].[Standard] ([StandardCode], [Version]),
    CONSTRAINT [FK_Apprenticeship_Framework] FOREIGN KEY ([FrameworkCode], [ProgType], [PathwayCode]) REFERENCES [dbo].[Framework] ([FrameworkCode], [ProgType], [PathwayCode]),
    CONSTRAINT [FK_Apprenticeship_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId]),
    CONSTRAINT [FK_Apprenticeship_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]), 
    CONSTRAINT [FK_Apprenticeship_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_Apprenticeship_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Apprenticeship_AddedByApplicationId] FOREIGN KEY ([AddedByApplicationId]) REFERENCES [Application] ([ApplicationId])
)
GO

CREATE TRIGGER [dbo].[Trigger_Apprenticeship_Audit_Delete]
    ON [dbo].[Apprenticeship]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Apprenticeship (AuditOperation, ApprenticeshipId, ProviderId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, StandardCode, [Version], FrameworkCode, ProgType, PathwayCode, MarketingInformation, Url, ContactTelephone, ContactEmail, ContactWebsite)
		(SELECT 'D', ApprenticeshipId, ProviderId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, StandardCode, [Version], FrameworkCode, ProgType, PathwayCode, MarketingInformation, Url, ContactTelephone, ContactEmail, ContactWebsite FROM deleted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_Apprenticeship_Audit_InsertUpdate]
    ON [dbo].[Apprenticeship]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Apprenticeship (AuditOperation, ApprenticeshipId, ProviderId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, StandardCode, [Version], FrameworkCode, ProgType, PathwayCode, MarketingInformation, Url, ContactTelephone, ContactEmail, ContactWebsite)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, ApprenticeshipId, ProviderId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddedByApplicationId, RecordStatusId, StandardCode, [Version], FrameworkCode, ProgType, PathwayCode, MarketingInformation, Url, ContactTelephone, ContactEmail, ContactWebsite FROM inserted);
    END
GO


CREATE INDEX [IX_Apprenticeship_ProviderId] ON [dbo].[Apprenticeship] ([ProviderId])

GO

CREATE INDEX [IX_Apprenticeship_RecordStatusId] ON [dbo].[Apprenticeship] ([RecordStatusId])

GO

CREATE INDEX [IX_Apprenticeship_Standard] ON [dbo].[Apprenticeship] ([StandardCode], [Version])

GO

CREATE INDEX [IX_Apprenticeship_Framework] ON [dbo].[Apprenticeship] ([FrameworkCode], [ProgType], [PathwayCode])
