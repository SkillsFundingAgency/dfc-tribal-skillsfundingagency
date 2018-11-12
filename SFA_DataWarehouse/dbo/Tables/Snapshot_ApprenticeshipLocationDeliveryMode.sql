CREATE TABLE [dbo].[Snapshot_ApprenticeshipLocationDeliveryMode]
(
	[Period]					VARCHAR(7)	NOT NULL,
    [ApprenticeshipLocationId]	INT			NOT NULL,
    [DeliveryModeId]			INT			NOT NULL,
    CONSTRAINT [PK_Snapshot_ApprenticeshipLocationDeliveryMode] PRIMARY KEY CLUSTERED ([Period], [ApprenticeshipLocationId] ASC, [DeliveryModeId] ASC)
);
