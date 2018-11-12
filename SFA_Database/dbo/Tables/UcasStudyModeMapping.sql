CREATE TABLE [dbo].[UcasStudyModeMapping]
(
	[UcasStudyModeId] INT NOT NULL PRIMARY KEY, 
    [UcasStudyMode] NVARCHAR(100) NOT NULL, 
    [MapsToStudyModeId] INT NULL, 
    [MapsToAttendanceTypeId] INT NULL, 
    [MapsToAttendancePattern] INT NULL, 
    CONSTRAINT [FK_UcasStudyModeMapping_StudyMode] FOREIGN KEY ([MapsToStudyModeId]) REFERENCES [StudyMode]([StudyModeId]),
	CONSTRAINT [FK_UcasStudyModeMapping_AttendanceType] FOREIGN KEY ([MapsToAttendanceTypeId]) REFERENCES [AttendanceType] ([AttendanceTypeId]),
	CONSTRAINT [FK_UcasStudyModeMapping_AttendancePattern] FOREIGN KEY ([MapsToAttendancePattern]) REFERENCES [AttendancePattern] ([AttendancePatternId])
)
GO

CREATE INDEX [IX_UcasStudyModeMapping] ON [dbo].[UcasStudyModeMapping] ([UcasStudyMode])
GO