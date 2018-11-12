CREATE TABLE [dbo].[OpenDataDownload]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [IPAddress] NVARCHAR(25) NOT NULL, 
    [Filename] NVARCHAR(50) NOT NULL
)

GO

CREATE INDEX [IX_OpenDataDownload_DateTimeUtc] ON [dbo].[OpenDataDownload] ([DateTimeUtc])
