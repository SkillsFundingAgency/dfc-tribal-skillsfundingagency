CREATE TABLE [dbo].[CourseInstanceA10FundingCode] (
    [CourseInstanceId] INT NOT NULL,
    [A10FundingCode]   INT NOT NULL,
    CONSTRAINT [PK_CourseInstanceA10FundingCode] PRIMARY KEY CLUSTERED ([CourseInstanceId] ASC, [A10FundingCode] ASC),
    CONSTRAINT [FK_CourseInstanceA10FundingCode_A10FundingCode] FOREIGN KEY ([A10FundingCode]) REFERENCES [dbo].[A10FundingCode] ([A10FundingCodeId]),
    CONSTRAINT [FK_CourseInstanceA10FundingCode_CourseInstance] FOREIGN KEY ([CourseInstanceId]) REFERENCES [dbo].[CourseInstance] ([CourseInstanceId])
);


GO

CREATE TRIGGER [dbo].[Trigger_CourseInstanceA10FundingCode_InsertUpdate]
    ON [dbo].[CourseInstanceA10FundingCode]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstanceA10FundingCode (AuditOperation, CourseInstanceId, A10FundingCode)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, CourseInstanceId, A10FundingCode FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_CourseInstanceA10FundingCode_Delete]
    ON [dbo].[CourseInstanceA10FundingCode]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstanceA10FundingCode (AuditOperation, CourseInstanceId, A10FundingCode)
		(SELECT 'D', CourseInstanceId, A10FundingCode FROM deleted);
    END