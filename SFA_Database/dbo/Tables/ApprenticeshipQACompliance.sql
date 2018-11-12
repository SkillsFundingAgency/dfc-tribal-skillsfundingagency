CREATE TABLE [dbo].[ApprenticeshipQACompliance]
(
	[ApprenticeshipQAComplianceId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ApprenticeshipId] INT NOT NULL,
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
	[TextQAd] NVARCHAR(900) NULL,
    [DetailsOfUnverifiableClaim] NVARCHAR(2000) NULL, 
    [DetailsOfComplianceFailure] NVARCHAR(2000) NULL, 
    [Passed] BIT NOT NULL DEFAULT 0
    CONSTRAINT [FK_ApprenticeshipQACompliance_AspNetUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ApprenticeshipQACompliance_Apprenticeship] FOREIGN KEY ([ApprenticeshipId]) REFERENCES [Apprenticeship]([ApprenticeshipId])
)
