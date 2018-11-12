CREATE TABLE [dbo].[Audit_ApprenticeshipLocation]
(
	[AuditSeq]					INT				NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation]			NVARCHAR		NOT NULL, 
    [AuditDateUtc]				DATETIME		NOT NULL DEFAULT GetUtcDate(), 
	[ApprenticeshipLocationId]	INT				NOT NULL, 
    [ApprenticeshipId]			INT				NOT NULL, 
    [LocationId]				INT				NOT NULL,
	[Radius]					INT				NULL,
	[RecordStatusId]			INT             NOT NULL,
	[AddedByApplicationId]		INT				NOT NULL,
    [CreatedByUserId]           NVARCHAR(128)	NOT NULL,
    [CreatedDateTimeUtc]        DATETIME		NOT NULL DEFAULT GetUTCDate(),
    [ModifiedByUserId]          NVARCHAR(128)	NULL,
    [ModifiedDateTimeUtc]       DATETIME		NULL
)

GO

CREATE INDEX [IX_Audit_ApprenticeshipLocation_ApprenticeshipLocationId] ON [dbo].[Audit_ApprenticeshipLocation] ([ApprenticeshipLocationId])

