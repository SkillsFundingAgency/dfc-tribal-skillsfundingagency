CREATE TABLE [dbo].[Snapshot_CourseInstanceVenue]
(
	[Period]			VARCHAR(7) NOT NULL,
    [CourseInstanceId] INT NOT NULL,
    [VenueId]          INT NOT NULL,
    CONSTRAINT [PK_Snapshot_CourseInstanceVenue] PRIMARY KEY CLUSTERED ([Period], [CourseInstanceId], [VenueId])
);

GO

CREATE INDEX [IX_Snapshot_CourseInstanceVenue_VenueId] ON [dbo].[Snapshot_CourseInstanceVenue] ([Period], [VenueId])
GO
