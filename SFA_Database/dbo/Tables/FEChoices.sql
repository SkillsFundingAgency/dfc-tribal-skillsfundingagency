CREATE TABLE [dbo].[FEChoices]
(
	[UPIN] INT NOT NULL PRIMARY KEY, 
    [LearnerDestination] FLOAT NULL, 
    [LearnerSatisfaction] FLOAT NULL, 
    [EmployerSatisfaction] FLOAT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate() 
)
