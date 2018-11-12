CREATE TABLE [dbo].[ProviderUnableToComplete]
(
	[ProviderUnableToCompleteId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProviderId] INT NOT NULL,
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
	[TextUnableToComplete] NVARCHAR(900) NULL
	CONSTRAINT [FK_ProviderUnableToComplete_AspNetUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_ProviderUnableToComplete_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([ProviderId])
)
