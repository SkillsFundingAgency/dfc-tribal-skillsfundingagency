CREATE TABLE [dbo].[ApprenticeshipLocationDeliveryMode] (
    [ApprenticeshipLocationId] INT NOT NULL,
    [DeliveryModeId]   INT NOT NULL,
    CONSTRAINT [PK_ApprenticeshipLocationDeliveryMode] PRIMARY KEY CLUSTERED ([ApprenticeshipLocationId] ASC, [DeliveryModeId] ASC),
    CONSTRAINT [FK_ApprenticeshipLocationDeliveryMode_ApprenticeshipLocationId] FOREIGN KEY ([ApprenticeshipLocationId]) REFERENCES [dbo].[ApprenticeshipLocation] ([ApprenticeshipLocationId]),
    CONSTRAINT [FK_ApprenticeshipLocationDeliveryMode_DeliveryModeId] FOREIGN KEY ([DeliveryModeId]) REFERENCES [dbo].[DeliveryMode] ([DeliveryModeId])
);


GO

CREATE TRIGGER [dbo].[Trigger_ApprenticeshipLocationDeliveryMode_InsertUpdate]
    ON [dbo].[ApprenticeshipLocationDeliveryMode]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_ApprenticeshipLocationDeliveryMode (AuditOperation, ApprenticeshipLocationId, DeliveryModeId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, ApprenticeshipLocationId, DeliveryModeId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_ApprenticeshipLocationDeliveryMode_Delete]
    ON [dbo].[ApprenticeshipLocationDeliveryMode]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_ApprenticeshipLocationDeliveryMode (AuditOperation, ApprenticeshipLocationId, DeliveryModeId)
		(SELECT 'D', ApprenticeshipLocationId, DeliveryModeId FROM deleted);
    END

