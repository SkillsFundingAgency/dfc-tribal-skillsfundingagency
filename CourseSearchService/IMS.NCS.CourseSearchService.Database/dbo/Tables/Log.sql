CREATE TABLE [dbo].[Log]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [MachineName] NVARCHAR(50) NOT NULL, 
    [Method] NVARCHAR(50) NOT NULL, 
    [Details] TEXT NOT NULL
)

GO

CREATE INDEX [IX_Log_DateTimeUtc] ON [dbo].[Log] ([DateTimeUtc])