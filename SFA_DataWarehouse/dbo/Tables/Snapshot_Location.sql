CREATE TABLE [dbo].[Snapshot_Location]
(
	[Period]		VARCHAR(7) NOT NULL,
	[LocationId]   INT NOT NULL,
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
    CONSTRAINT [PK_Snapshot_LocationId] PRIMARY KEY CLUSTERED ([Period], [LocationId] ASC)
);
GO

CREATE INDEX [IX_Snapshot_Location_ProviderId] ON [dbo].[Snapshot_Location] ([Period], [ProviderId]);
GO

CREATE INDEX [IX_Snapshot_Location_RecordStatusId] ON [dbo].[Snapshot_Location] ([Period], [RecordStatusId]);
