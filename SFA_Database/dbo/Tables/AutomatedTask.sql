CREATE TABLE [dbo].[AutomatedTask]
(
    [TaskName] VARCHAR(50) NOT NULL, 
    [InProgress] BIT NOT NULL DEFAULT 0, 
    [DateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    CONSTRAINT [PK_AutomatedTask] PRIMARY KEY ([TaskName])
)

GO
