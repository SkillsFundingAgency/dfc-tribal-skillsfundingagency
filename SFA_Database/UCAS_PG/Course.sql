CREATE TABLE [UCAS_PG].[Course]
(
	[CourseId] INT NOT NULL PRIMARY KEY, 
    [UniqueId] NVARCHAR(50) NOT NULL, 
	[ProviderId] INT NOT NULL,
    [CourseTitle] NVARCHAR(150) NOT NULL, 
    [CourseSummary] NVARCHAR(MAX) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL, 
    [CreatedByUserId] NVARCHAR(128) NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_Course_UniqueId] ON [UCAS_PG].[Course] ([UniqueId])

GO

CREATE INDEX [IX_Course_ProviderId] ON [UCAS_PG].[Course] ([ProviderId])
