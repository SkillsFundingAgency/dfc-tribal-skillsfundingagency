CREATE TABLE [dbo].[LocationAlias]
(
	[LocationAliasId]		INT NOT NULL PRIMARY KEY,
	[ParentLocationAliasId] INT NULL,
	[LocationAliasTypeId]	INT NOT NULL,
	[LocationAliasName]		NVARCHAR(32) NOT NULL,
	CONSTRAINT [FK_LocationAlias_LocationAliasType] FOREIGN KEY ([LocationAliasTypeId]) REFERENCES [dbo].[LocationAliasType] ([LocationAliasTypeId]),
)
