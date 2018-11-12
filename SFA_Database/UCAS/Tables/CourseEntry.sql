CREATE TABLE [UCAS].[CourseEntry]
(
	[CourseEntryId] INT NOT NULL PRIMARY KEY, 
    [CourseId] INT NOT NULL, 
    [EntryId] INT NOT NULL, 
    [MinPoints] NVARCHAR(20) NULL, 
    [MaxPoints] NVARCHAR(20) NULL, 
    [Subjects] NVARCHAR(500) NULL 
)

GO

CREATE INDEX [IX_CourseEntry_CourseId] ON [UCAS].[CourseEntry] ([CourseId])

GO

CREATE INDEX [IX_CourseEntry_EntryId] ON [UCAS].[CourseEntry] ([EntryId])
