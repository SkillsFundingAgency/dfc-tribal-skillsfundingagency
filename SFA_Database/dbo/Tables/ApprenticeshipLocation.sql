CREATE TABLE [dbo].[ApprenticeshipLocation] (
    [ApprenticeshipLocationId]	INT IDENTITY(1,1) NOT NULL,
    [ApprenticeshipId]			INT				NOT NULL,
    [LocationId]				INT				NOT NULL,
	[Radius]					INT				NULL,
	[RecordStatusId]			INT             NOT NULL,
    [AddedByApplicationId]      INT             NOT NULL,
    [CreatedByUserId]           NVARCHAR(128)	NOT NULL,
    [CreatedDateTimeUtc]        DATETIME		NOT NULL DEFAULT GetUTCDate(),
    [ModifiedByUserId]          NVARCHAR(128)	NULL,
    [ModifiedDateTimeUtc]       DATETIME		NULL,
    CONSTRAINT [PK_ApprenticeshipLocation] PRIMARY KEY CLUSTERED ([ApprenticeshipLocationId] ASC),
    CONSTRAINT [FK_ApprenticeshipLocation_Application] FOREIGN KEY ([AddedByApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_ApprenticeshipLocation_ApprenticeshipId] FOREIGN KEY ([ApprenticeshipId]) REFERENCES [dbo].[Apprenticeship] ([ApprenticeshipId]),
    CONSTRAINT [FK_ApprenticeshipLocation_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_ApprenticeshipLocation_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]),
    CONSTRAINT [FK_ApprenticeshipLocation_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]), 
    CONSTRAINT [FK_ApprenticeshipLocation_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
GO

CREATE TRIGGER [dbo].[Trigger_ApprenticeshipLocation_InsertUpdate]
    ON [dbo].[ApprenticeshipLocation]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_ApprenticeshipLocation (AuditOperation, ApprenticeshipLocationId, ApprenticeshipId, LocationId, Radius, RecordStatusId, AddedByApplicationId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, ApprenticeshipLocationId, ApprenticeshipId, LocationId, Radius, RecordStatusId, AddedByApplicationId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_ApprenticeshipLocation_Delete]
    ON [dbo].[ApprenticeshipLocation]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_ApprenticeshipLocation (AuditOperation, ApprenticeshipLocationId, ApprenticeshipId, LocationId, Radius, RecordStatusId, AddedByApplicationId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc)
		(SELECT 'D', ApprenticeshipLocationId, ApprenticeshipId, LocationId, Radius, RecordStatusId, AddedByApplicationId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc FROM deleted);
    END
GO

CREATE INDEX [IX_ApprenticeshipLocation_ApprenticeshipId] ON [dbo].[ApprenticeshipLocation] ([ApprenticeshipId])
GO

CREATE INDEX [IX_ApprenticeshipLocation_RecordStatusId] ON [dbo].[ApprenticeshipLocation] ([RecordStatusId])
GO
