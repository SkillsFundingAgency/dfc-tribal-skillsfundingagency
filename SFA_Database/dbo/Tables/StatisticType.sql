CREATE TABLE [dbo].[StatisticType] (
    [StatisticTypeId]   INT           NOT NULL,
    [StatisticTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StatisticType] PRIMARY KEY CLUSTERED ([StatisticTypeId] ASC)
);

