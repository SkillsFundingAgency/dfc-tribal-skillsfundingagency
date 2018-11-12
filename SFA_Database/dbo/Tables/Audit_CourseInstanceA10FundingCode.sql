CREATE TABLE [dbo].[Audit_CourseInstanceA10FundingCode]
(
	[AuditSeq] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation] NVARCHAR NOT NULL, 
    [AuditDateUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [CourseInstanceId] INT NOT NULL, 
    [A10FundingCode] INT NOT NULL
)

GO

CREATE INDEX [IX_Audit_CourseInstanceA10FundingCode_CourseInstanceId_A10FundingCode] ON [dbo].[Audit_CourseInstanceA10FundingCode] ([CourseInstanceId], [A10FundingCode])
