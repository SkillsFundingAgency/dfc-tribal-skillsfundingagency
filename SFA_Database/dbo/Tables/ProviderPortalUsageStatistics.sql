CREATE TABLE [dbo].[ProviderPortalUsageStatistics] (
    [ProviderId]        INT  NOT NULL,
    [DayDate]           DATE NOT NULL,
    [StatisticTypeId]   INT  NOT NULL,
    [StatisticActionId] INT  NOT NULL,
    [Count]             INT  CONSTRAINT [DF_ProviderPortalUsageStatistics_Count] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProviderPortalUsageStatistics] PRIMARY KEY CLUSTERED ([ProviderId] ASC, [DayDate] ASC, [StatisticTypeId] ASC, [StatisticActionId] ASC),
    CONSTRAINT [FK_ProviderPortalUsageStatistics_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId]),
    CONSTRAINT [FK_ProviderPortalUsageStatistics_StatisticAction] FOREIGN KEY ([StatisticActionId]) REFERENCES [dbo].[StatisticAction] ([StatisticActionId]),
    CONSTRAINT [FK_ProviderPortalUsageStatistics_StatisticType] FOREIGN KEY ([StatisticTypeId]) REFERENCES [dbo].[StatisticType] ([StatisticTypeId])
);

