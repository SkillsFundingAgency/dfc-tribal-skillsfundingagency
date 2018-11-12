CREATE TABLE [dbo].[ProviderUser]
(
	[UserId] NVARCHAR(128) NOT NULL, 
    [ProviderId] INT NOT NULL,
    PRIMARY KEY ([UserId], [ProviderId]), 
    CONSTRAINT [FK_ProviderUser_User] FOREIGN KEY (UserId) REFERENCES [AspNetUsers]([Id]), 
	CONSTRAINT [FK_ProviderUser_Provider] FOREIGN KEY([ProviderId]) REFERENCES [Provider] ([ProviderId])
)
GO

CREATE TRIGGER [dbo].[Trigger_ProviderUser_InsertUpdate]
    ON [dbo].[ProviderUser]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_ProviderUser (AuditOperation, UserId, ProviderId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, UserId, ProviderId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_ProviderUser_Delete]
    ON [dbo].[ProviderUser]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_ProviderUser (AuditOperation, UserId, ProviderId)
		(SELECT 'D', UserId, ProviderId FROM deleted);
    END
GO

CREATE INDEX [IX_ProviderUser_ProviderId] ON [dbo].[ProviderUser] ([ProviderId]) INCLUDE ([UserId])
GO
