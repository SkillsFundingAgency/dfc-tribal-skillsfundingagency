CREATE TABLE [UCAS].[CoursesIndex]
(
	[CourseIndexId] INT NOT NULL PRIMARY KEY, 
    [CourseId] INT NOT NULL, 
    [MinDuration] FLOAT NULL, 
    [MaxDuration] FLOAT NULL, 
    [DurationId] INT NULL, 
    [QualificationId] INT NOT NULL, 
    [StudyModeId] INT NULL 
)

GO

CREATE INDEX [IX_CoursesIndex_DurationId] ON [UCAS].[CoursesIndex] ([DurationId])
