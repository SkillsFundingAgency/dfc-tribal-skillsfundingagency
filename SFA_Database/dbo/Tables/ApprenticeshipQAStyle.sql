CREATE TABLE [dbo].[ApprenticeshipQAStyle]
(
	[ApprenticeshipQAStyleId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ApprenticeshipId] INT NOT NULL,
    [CreatedByUserId] NVARCHAR(128) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
	[TextQAd] NVARCHAR(900) NULL,
    [Passed] BIT NOT NULL DEFAULT 0,
	[DetailsOfQA] NVARCHAR(1000)
    CONSTRAINT [FK_ApprenticeshipQAStyle_AspNetUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ApprenticeshipQAStyle_Apprenticeship] FOREIGN KEY ([ApprenticeshipId]) REFERENCES [Apprenticeship]([ApprenticeshipId])
)
