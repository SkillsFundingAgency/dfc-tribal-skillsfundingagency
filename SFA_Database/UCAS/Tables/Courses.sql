CREATE TABLE [UCAS].[Courses]
(
	[CourseId] INT NOT NULL PRIMARY KEY, 
    [OrgId] INT NOT NULL, 
    [CourseTitle] NVARCHAR(150) NOT NULL, 
    [Summary] NVARCHAR(MAX) NULL, 
    [Modules] NVARCHAR(MAX) NULL, 
    [AssessmentMethods] NVARCHAR(MAX) NULL, 
    [AdditionalEntry] NVARCHAR(MAX) NULL, 
    [NoOfPlaces] NVARCHAR(4) NULL, 
    [HESA1] NVARCHAR(10) NULL, 
    [HESA2] NVARCHAR(10) NULL, 
    [HESA3] NVARCHAR(10) NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CreatedByUserId] NVARCHAR(128) NOT NULL 
)

GO

CREATE INDEX [IX_Courses_OrgId] ON [UCAS].[Courses] ([OrgId])
