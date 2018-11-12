CREATE TABLE [dbo].[Audit_ApprenticeshipLocationDeliveryMode]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [ApprenticeshipLocationId] INT NOT NULL, 
    [DeliveryModeId] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_ApprenticeshipLocationDeliveryMode_ApprenticeshipLocationId_DeliveryModeId] ON [dbo].[Audit_ApprenticeshipLocationDeliveryMode] ([ApprenticeshipLocationId], [DeliveryModeId])

