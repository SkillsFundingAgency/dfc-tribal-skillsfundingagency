CREATE TABLE [dbo].[PublicAPIUser]
(
	[PublicAPIUserId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NewId(), 
    [CompanyName] NVARCHAR(100) NOT NULL, 
    [Telephone] NVARCHAR(100) NULL, 
    [Email] NVARCHAR(100) NULL, 
    [ContactFirstName] NVARCHAR(100) NULL, 
    [ContactLastName] NVARCHAR(100) NULL, 
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [ModifiedByUserId] NVARCHAR(128) NULL, 
    [ModifiedDateTimeUtc] DATETIME NULL,
    [RecordStatusId] INT NOT NULL, 
    CONSTRAINT [FK_PublicAPIUser_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
)
GO

CREATE TRIGGER [dbo].[Trigger_PublicAPIUser_Audit_Delete]
    ON [dbo].[PublicAPIUser]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_PublicAPIUser (AuditOperation, PublicAPIUserId, CompanyName, Telephone, Email, ContactFirstName, ContactLastName, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId)
		(SELECT 'D', PublicAPIUserId, CompanyName, Telephone, Email, ContactFirstName, ContactLastName, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId FROM deleted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_PublicAPIUser_Audit_InsertUpdate]
    ON [dbo].[PublicAPIUser]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_PublicAPIUser (AuditOperation, PublicAPIUserId, CompanyName, Telephone, Email, ContactFirstName, ContactLastName, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, PublicAPIUserId, CompanyName, Telephone, Email, ContactFirstName, ContactLastName, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, RecordStatusId FROM inserted);
    END
GO
