CREATE TABLE [search].[DAS_ApprenticeshipLocationDeliveryMode](
	[ApprenticeshipLocationId] [int] NOT NULL,
	[DeliveryModeId] [int] NOT NULL,
    PRIMARY KEY NONCLUSTERED ([ApprenticeshipLocationId], [DeliveryModeId] ASC)
);