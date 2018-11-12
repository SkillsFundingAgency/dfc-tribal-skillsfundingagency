CREATE TABLE [dbo].[Snapshot_ProviderUser]
(
	[Period]		VARCHAR(7)		NOT NULL,
	[UserId]		NVARCHAR(128)	NOT NULL, 
    [ProviderId]	INT				NOT NULL,
    PRIMARY KEY ([Period], [UserId], [ProviderId])
);
GO

CREATE INDEX [IX_Snapshot_ProviderUser_ProviderId] ON [dbo].[Snapshot_ProviderUser] ([Period], [ProviderId]) INCLUDE ([UserId])
GO
