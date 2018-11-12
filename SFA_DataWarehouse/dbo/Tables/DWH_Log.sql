﻿CREATE TABLE [dbo].[DWH_Log]
(
	[Id] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY, 
    [DateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [LogType] NVARCHAR(25) NOT NULL, 
    [Message] NVARCHAR(1000) NOT NULL
);

