CREATE TABLE [dbo].[ProgressMessage]
(
	[MessageArea] NVARCHAR(10) NOT NULL PRIMARY KEY, 
    [IsComplete] BIT NOT NULL, 
    [MessageText] NVARCHAR(Max) NOT NULL
)
