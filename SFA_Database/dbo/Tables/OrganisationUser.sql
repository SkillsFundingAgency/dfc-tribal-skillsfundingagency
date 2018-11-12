CREATE TABLE [dbo].[OrganisationUser]
(
	[UserId] NVARCHAR(128) NOT NULL, 
    [OrganisationId] INT NOT NULL,
    PRIMARY KEY ([UserId], [OrganisationId]), 
    CONSTRAINT [FK_OrganisationUser_User] FOREIGN KEY (UserId) REFERENCES [AspNetUsers]([Id]), 
	CONSTRAINT [FK_OrganisationUser_Organisation] FOREIGN KEY([OrganisationId]) REFERENCES [Organisation] ([OrganisationId])    
)


GO

CREATE TRIGGER [dbo].[Trigger_OrganisationUser_InsertUpdate]
    ON [dbo].[OrganisationUser]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_OrganisationUser (AuditOperation, UserId, OrganisationId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, UserId, OrganisationId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_OrganisationUser_Delete]
    ON [dbo].[OrganisationUser]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_OrganisationUser (AuditOperation, UserId, OrganisationId)
		(SELECT 'D', UserId, OrganisationId FROM deleted);
    END