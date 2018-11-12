CREATE TABLE [dbo].[Snapshot_Venue]
(
	[Period]			  VARCHAR(7)	  NOT NULL,
    [VenueId]             INT             NOT NULL,
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
    CONSTRAINT [PK_Snapshot_Venue] PRIMARY KEY CLUSTERED ([Period], [VenueId] ASC)
);
GO

CREATE INDEX [IX_Snapshot_Venue_ProviderId] ON [dbo].[Snapshot_Venue] ([Period], [ProviderId]);
GO

CREATE INDEX [IX_Snapshot_Venue_RecordStatusId] ON [dbo].[Snapshot_Venue] ([Period], [RecordStatusId]);
