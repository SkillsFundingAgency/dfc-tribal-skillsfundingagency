CREATE TABLE [search].[DAS_Apprenticeship](
	[ApprenticeshipId] [int] NOT NULL,
	[ProviderId] [int] NOT NULL,
	[StandardCode] [int] NULL,
	[FrameworkCode] [int] NULL,
	[ProgType] [int] NULL,
	[PathwayCode] [int] NULL,
	[MarketingInformation] [nvarchar](900) NULL,
	[Url] [nvarchar](255) NULL,
	[ContactTelephone] [nvarchar](30) NULL,
	[ContactEmail] [nvarchar](255) NULL,
	[ContactWebsite] [nvarchar](255) NULL,

    PRIMARY KEY NONCLUSTERED ([ApprenticeshipId] ASC)
);