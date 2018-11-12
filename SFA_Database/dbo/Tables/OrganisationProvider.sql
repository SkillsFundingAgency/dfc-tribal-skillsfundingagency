CREATE TABLE [dbo].[OrganisationProvider] (
    [OrganisationId]              INT      NOT NULL,
    [ProviderId]                  INT      NOT NULL,
    [IsRejected]                  BIT      CONSTRAINT [DF_OrganisationProvider_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsAccepted]                  BIT      CONSTRAINT [DF_OrganisationProvider_IsAccepted] DEFAULT ((0)) NOT NULL,
	[Reason]                      NVARCHAR(200)      NULL,
    [RespondedByUserId]           NVARCHAR(128)      NULL,
    [RespondedByDateTimeUtc]      DATETIME NULL,
    [CanOrganisationEditProvider] BIT      CONSTRAINT [DF_OrganisationProvider_CanOrganisationEditProvider] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OrganisationProvider] PRIMARY KEY CLUSTERED ([OrganisationId] ASC, [ProviderId] ASC),
    CONSTRAINT [FK_OrganisationProvider_Organisation] FOREIGN KEY ([OrganisationId]) REFERENCES [dbo].[Organisation] ([OrganisationId]),
    CONSTRAINT [FK_OrganisationProvider_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId])
);


GO

CREATE TRIGGER [dbo].[Trigger_OrganisationProvider_InsertUpdate]
    ON [dbo].[OrganisationProvider]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_OrganisationProvider (AuditOperation, OrganisationId, ProviderId, IsRejected, IsAccepted, Reason, RespondedByUserId, RespondedByDateTimeUtc, CanOrganisationEditProvider)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, OrganisationId, ProviderId, IsRejected, IsAccepted, Reason, RespondedByUserId, RespondedByDateTimeUtc, CanOrganisationEditProvider FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_OrganisationProvider_Delete]
    ON [dbo].[OrganisationProvider]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_OrganisationProvider (AuditOperation, OrganisationId, ProviderId, IsRejected, IsAccepted, Reason, RespondedByUserId, RespondedByDateTimeUtc, CanOrganisationEditProvider)
		(SELECT 'D', OrganisationId, ProviderId, IsRejected, IsAccepted, Reason, RespondedByUserId, RespondedByDateTimeUtc, CanOrganisationEditProvider FROM deleted);
    END