CREATE TABLE [dbo].[DFEReport_Usage]
(
	[Period] VARCHAR(7) NOT NULL, 
    [NumberOfProvidersWithValidSuperUser] INT NOT NULL, 
    [NumberOfProviders] INT NOT NULL, 
    [NumberOfProvidersUpdatedOpportunityInPeriod] INT NOT NULL, 
    [TotalBulkUploadOpportunities] INT NOT NULL, 
    [NumberOfBulkUploadOpportunitiesInPeriod] INT NOT NULL, 
	[TotalManuallyUpdatedOpportunities] INT NOT NULL,
    [NumberOfManuallyUpdatedOpportunitiesInPeriod] INT NOT NULL, 
    [NumberOfProvidersNotUpdatedOpportunityInPastYear] INT NOT NULL, 
    [NumberOfProvidersUpdatedDuringPeriod] INT NOT NULL, 
    [NumberOfProvidersUpdated1to2PeriodsAgo] INT NOT NULL, 
    [NumberOfProvidersUpdated2to3PeriodsAgo] INT NOT NULL, 
    [NumberOfProvidersUpdatedMoreThan3PeriodsAgo] INT NOT NULL, 
    CONSTRAINT [PK_DFEReport_Usage] PRIMARY KEY ([Period])
);
