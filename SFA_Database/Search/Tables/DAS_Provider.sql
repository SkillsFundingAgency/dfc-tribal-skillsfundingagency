CREATE TABLE [Search].[DAS_Provider] (
    [ProviderId]          INT            NOT NULL,
    [ProviderName]        NVARCHAR (200) NOT NULL,
    [Ukprn]               INT            NOT NULL,
    [Email]               NVARCHAR (255) NULL,
    [Website]             NVARCHAR (511) NULL,
    [Telephone]           NVARCHAR (30)  NULL,
    [MarketingInformation] NVARCHAR (900)  NULL,
    [LearnerSatisfaction] FLOAT NULL, 
    [EmployerSatisfaction] FLOAT NULL, 
    [National] BIT NOT NULL DEFAULT 0, 
    [TradingName] VARCHAR(255) NULL, 
    PRIMARY KEY NONCLUSTERED ([ProviderId] ASC)
);
