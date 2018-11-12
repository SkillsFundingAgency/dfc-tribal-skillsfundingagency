CREATE TABLE [search].[DAS_ApprenticeshipLocation](
	[ApprenticeshipLocationId] [int] NOT NULL,
	[ApprenticeshipId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[Radius] [int] NULL,
    PRIMARY KEY NONCLUSTERED ([ApprenticeshipLocationId] ASC)
);