CREATE TABLE [dbo].[Location] (
    [LocationId]   INT IDENTITY (1, 1) NOT NULL,
	[ProviderId]   INT NOT NULL,
	[ProviderOwnLocationRef] NVARCHAR (255)  NULL,
	[LocationName] NVARCHAR (255)  NULL,
	[AddressId] INT NOT NULL,
    [Telephone]     NVARCHAR(30)    NULL, 
	[Email]         NVARCHAR(255)   NULL, 
	[Website]       NVARCHAR(255)   NULL,
    [RecordStatusId] INT NOT NULL, 
	[CreatedByUserId]     NVARCHAR(128)             NOT NULL,
    [CreatedDateTimeUtc]  DATETIME        NOT NULL,
    [ModifiedByUserId]    NVARCHAR(128)             NULL,
    [ModifiedDateTimeUtc] DATETIME        NULL,
    [BulkUploadLocationId] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_LocationId] PRIMARY KEY CLUSTERED ([LocationId] ASC), 
	CONSTRAINT [FK_Location_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([ProviderId]),	
    CONSTRAINT [FK_Location_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([AddressId]),	
    CONSTRAINT [FK_Location_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus] ([RecordStatusId]),
    CONSTRAINT [FK_Location_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_Location_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id])
);


GO

CREATE TRIGGER [dbo].[Trigger_Location_InsertUpdate]
    ON [dbo].[Location]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Location (AuditOperation, LocationId, ProviderId, ProviderOwnLocationRef, LocationName, AddressId, Telephone, Email, Website, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, BulkUploadLocationId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, LocationId, ProviderId, ProviderOwnLocationRef, LocationName, AddressId, Telephone, Email, Website, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, BulkUploadLocationId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_Location_Delete]
    ON [dbo].[Location]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Location (AuditOperation, LocationId, ProviderId, ProviderOwnLocationRef, LocationName, AddressId, Telephone, Email, Website, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, BulkUploadLocationId)
		(SELECT 'D', LocationId, ProviderId, ProviderOwnLocationRef, LocationName, AddressId, Telephone, Email, Website, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, BulkUploadLocationId FROM deleted);
    END
GO


CREATE INDEX [IX_Location_ProviderId] ON [dbo].[Location] ([ProviderId])

GO

CREATE INDEX [IX_Location_RecordStatusId] ON [dbo].[Location] ([RecordStatusId])
