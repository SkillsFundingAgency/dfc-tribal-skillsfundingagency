CREATE TABLE [dbo].[Audit_CourseLearnDirectClassification]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CourseId] INT NOT NULL, 
    [LearnDirectClassificationRef] NVARCHAR(12) NOT NULL, 
    [ClassificationOrder] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_CourseLearnDirectClassification_CourseId_LearnDirectClassificationId] ON [dbo].[Audit_CourseLearnDirectClassification] ([CourseId], [LearnDirectClassificationRef])
