CREATE TABLE [UCAS].[StartsIndex]
(
	[StartIndexId] INT NOT NULL PRIMARY KEY, 
    [StartId] INT NOT NULL, 
    [CourseIndexId] INT NULL, 
    [PlaceOfStudyId] INT NULL
)

GO

CREATE INDEX [IX_StartsIndex_StartId] ON [UCAS].[StartsIndex] ([StartId])

GO

CREATE INDEX [IX_StartsIndex_CourseIndexId] ON [UCAS].[StartsIndex] ([CourseIndexId])

GO

CREATE INDEX [IX_StartsIndex_PlaceOfStudyId] ON [UCAS].[StartsIndex] ([PlaceOfStudyId])
