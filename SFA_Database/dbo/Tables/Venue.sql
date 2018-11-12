CREATE TABLE [dbo].[Venue] (
    [VenueId]             INT             IDENTITY (1, 1) NOT NULL,
    [ProviderId]          INT             NOT NULL,
    [ProviderOwnVenueRef] NVARCHAR (255)  NULL,
    [VenueName]           NVARCHAR (255)  NOT NULL,
    [Email]               NVARCHAR (255)  NULL,
    [Website]             NVARCHAR (255)  NULL,
    [Fax]                 NVARCHAR (35)   NULL,
    [Facilities]          NVARCHAR (2000) NULL,
    [RecordStatusId]      INT             NOT NULL,
    [CreatedByUserId]     NVARCHAR(128)             NOT NULL,
    [CreatedDateTimeUtc]  DATETIME        NOT NULL,
    [ModifiedByUserId]    NVARCHAR(128)             NULL,
    [ModifiedDateTimeUtc] DATETIME        NULL,
    [AddressId] INT NOT NULL, 
    [Telephone] NVARCHAR(30) NULL, 
    [BulkUploadVenueId] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_Venue] PRIMARY KEY CLUSTERED ([VenueId] ASC),
    CONSTRAINT [FK_Venue_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]), 
    CONSTRAINT [FK_Venue_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([AddressId]), 
    CONSTRAINT [FK_Venue_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([ProviderId]),
    CONSTRAINT [FK_Venue_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_venue_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id])
)
GO

CREATE TRIGGER [dbo].[Trigger_Venue_InsertUpdate]
    ON [dbo].[Venue]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Venue (AuditOperation, VenueId, ProviderId, ProviderOwnVenueRef, VenueName, Email, Website, Fax, Facilities, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddressId, Telephone, BulkUploadVenueId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, VenueId, ProviderId, ProviderOwnVenueRef, VenueName, Email, Website, Fax, Facilities, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddressId, Telephone, BulkUploadVenueId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_Venue_Delete]
    ON [dbo].[Venue]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_Venue (AuditOperation, VenueId, ProviderId, ProviderOwnVenueRef, VenueName, Email, Website, Fax, Facilities, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddressId, Telephone, BulkUploadVenueId)
		(SELECT 'D', VenueId, ProviderId, ProviderOwnVenueRef, VenueName, Email, Website, Fax, Facilities, RecordStatusId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, AddressId, Telephone, BulkUploadVenueId FROM deleted);
    END
GO


CREATE INDEX [IX_Venue_ProviderId] ON [dbo].[Venue] ([ProviderId])

GO

CREATE INDEX [IX_Venue_RecordStatusId] ON [dbo].[Venue] ([RecordStatusId])
