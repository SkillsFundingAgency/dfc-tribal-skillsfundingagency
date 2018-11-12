CREATE TABLE [dbo].[Snapshot_ApprenticeshipLocation]
(
	[Period]					VARCHAR(7)		NOT NULL,
    [ApprenticeshipLocationId]	INT				NOT NULL,
    [ApprenticeshipId]			INT				NOT NULL,
    [LocationId]				INT				NOT NULL,
	[Radius]					INT				NULL,
	[RecordStatusId]			INT             NOT NULL,
    [AddedByApplicationId]      INT             NOT NULL,
    [CreatedByUserId]           NVARCHAR(128)	NOT NULL,
    [CreatedDateTimeUtc]        DATETIME		NOT NULL DEFAULT GetUTCDate(),
    [ModifiedByUserId]          NVARCHAR(128)	NULL,
    [ModifiedDateTimeUtc]       DATETIME		NULL,
    CONSTRAINT [PK_Snapshot_ApprenticeshipLocation] PRIMARY KEY CLUSTERED ([Period], [ApprenticeshipLocationId] ASC)
);
GO

CREATE INDEX [IX_Snapshot_ApprenticeshipLocation_ApprenticeshipId] ON [dbo].[Snapshot_ApprenticeshipLocation] ([Period], [ApprenticeshipId]);
GO

CREATE INDEX [IX_Snapshot_ApprenticeshipLocation_RecordStatusId] ON [dbo].[Snapshot_ApprenticeshipLocation] ([Period], [RecordStatusId]);
GO
