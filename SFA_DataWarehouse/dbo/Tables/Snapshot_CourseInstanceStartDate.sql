CREATE TABLE [dbo].[Snapshot_CourseInstanceStartDate]
(
	[Period]					VARCHAR(7) NOT NULL,
    [CourseInstanceStartDateId] INT  NOT NULL,
    [CourseInstanceId]          INT  NOT NULL,
    [StartDate]                 DATE NOT NULL,
    [IsMonthOnlyStartDate]      BIT  CONSTRAINT [DF_Snapshot_CourseInstanceStartDates_IsMonthOnlyStartDate] DEFAULT (0) NOT NULL,
    [PlacesAvailable] INT NULL, 
    CONSTRAINT [PK_Snapshot_CourseInstanceStartDates] PRIMARY KEY CLUSTERED ([Period], [CourseInstanceStartDateId] ASC),
);
GO

CREATE INDEX [IX_Snapshot_CourseInstanceStartDate_CourseInstanceId] ON [dbo].[Snapshot_CourseInstanceStartDate] ([Period], [CourseInstanceId]);
GO

