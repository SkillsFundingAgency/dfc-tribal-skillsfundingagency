CREATE TABLE [Search].[DataExportConfiguration] (
    [DataExportConfigurationId] INT      IDENTITY (1, 1) NOT NULL,
    [ThresholdPercent]          INT      CONSTRAINT [DF_DataExportConfiguration1_ThresholdPercent] DEFAULT ((0)) NULL,
    [OverrideThreshold]         BIT      CONSTRAINT [DF_DataExportConfiguration1_OverrideThreshold] DEFAULT ((0)) NULL,
    [IsEnabled]                 BIT      CONSTRAINT [DF_DataExportConfiguration1_IsEnabled] DEFAULT ((0)) NULL,
    [CreatedOn]                 DATETIME CONSTRAINT [DF_DataExportConfiguration1_CreatedOn] DEFAULT (getutcdate()) NULL,
    [IncludeUCASData] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_DataExportConfiguration1] PRIMARY KEY CLUSTERED ([DataExportConfigurationId] ASC)
);

