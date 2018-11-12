CREATE TABLE [dbo].[ProviderQACompliance]
(
	[ProviderQAComplianceId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProviderId] INT NOT NULL,
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
	[TextQAd] NVARCHAR(900) NULL,
    [DetailsOfUnverifiableClaim] NVARCHAR(2000) NULL, 
    [DetailsOfComplianceFailure] NVARCHAR(2000) NULL, 
    [Passed] BIT NOT NULL DEFAULT 0
    CONSTRAINT [FK_ProviderQACompliance_AspNetUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ProviderQACompliance_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([ProviderId])
)
