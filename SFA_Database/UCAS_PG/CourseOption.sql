CREATE TABLE [UCAS_PG].[CourseOption]
(
	[CourseOptionId] INT NOT NULL , 
    [UniqueId] NVARCHAR(50) NOT NULL, 
    [CourseId] INT NOT NULL, 
    [LocationId] INT NOT NULL, 
    [Qualification] NVARCHAR(100) NOT NULL, 
    [QualificationLevel] NVARCHAR(100) NOT NULL, 
    [EntryRequirements] NVARCHAR(MAX) NULL, 
    [AssessmentMethods] NVARCHAR(MAX) NULL, 
    [StudyMode] NVARCHAR(50) NOT NULL, 
    [StartDate] DATETIME NULL, 
    [DurationValue] INT NOT NULL, 
    [DurationType] NVARCHAR(25) NOT NULL, 
    CONSTRAINT [PK_CourseOption] PRIMARY KEY ([CourseOptionId]) 
)

GO

CREATE INDEX [IX_CourseOption_CourseId] ON [UCAS_PG].[CourseOption] ([CourseId])


GO

CREATE INDEX [IX_CourseOption_LocationId] ON [UCAS_PG].[CourseOption] ([LocationId])
