CREATE TABLE [dbo].[Audit_CourseInstanceVenue]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CourseInstanceId] INT NOT NULL, 
    [VenueId] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_CourseInstanceVenue_CourseInstanceId_VenueId] ON [dbo].[Audit_CourseInstanceVenue] ([CourseInstanceId], [VenueId])
