CREATE TABLE [dbo].[Audit_CourseInstanceStartDate]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CourseInstanceStartDateId] INT NOT NULL, 
    [CourseInstanceId] INT NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [IsMonthOnlyStartDate] BIT NOT NULL, 
    [PlacesAvailable] INT NULL
)

GO

CREATE INDEX [IX_Audit_CourseInstanceStartDate_CourseInstanceStartDateId] ON [dbo].[Audit_CourseInstanceStartDate] ([CourseInstanceStartDateId])
