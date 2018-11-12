CREATE TABLE [UCAS].[Deletions]
(
	[TableName] NVARCHAR(25) NOT NULL , 
    [RecordId] BIGINT NOT NULL, 
    PRIMARY KEY ([TableName], [RecordId])
)
