CREATE TABLE [dbo].[ProviderQAStyle]
(
	[ProviderQAStyleId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProviderId] INT NOT NULL,
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
	[TextQAd] NVARCHAR(900) NULL,
    [Passed] BIT NOT NULL DEFAULT 0,
	[DetailsOfQA] NVARCHAR(1000)
    CONSTRAINT [FK_ProviderQAStyle_AspNetUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ProviderQAStyle_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([ProviderId])
)
