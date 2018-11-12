CREATE TABLE [UCAS].[Starts]
(
	[StartId] INT NOT NULL PRIMARY KEY, 
    [CourseId] INT NOT NULL, 
    [StartDate] DATE NULL, 
    [StartDescription] NVARCHAR(150) NULL
)

GO

CREATE INDEX [IX_Starts_CourseId] ON [UCAS].[Starts] ([CourseId])
