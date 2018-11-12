CREATE TABLE [dbo].[CourseLearnDirectClassification]
(
	[CourseId] INT NOT NULL , 
    [LearnDirectClassificationRef] NVARCHAR(12) NOT NULL, 
    [ClassificationOrder] INT NOT NULL, 
    PRIMARY KEY ([CourseId], [LearnDirectClassificationRef]), 
    CONSTRAINT [FK_CourseLearnDirectClassification_Course] FOREIGN KEY ([CourseId]) REFERENCES [Course]([CourseId]),
	CONSTRAINT [FK_CourseLearnDirectClassification_LearnDirectClassification] FOREIGN KEY ([LearnDirectClassificationRef]) REFERENCES [LearnDirectClassification]([LearnDirectClassificationRef])
)

GO

CREATE TRIGGER [dbo].[Trigger_CourseLearnDirectClassification_InsertUpdate]
    ON [dbo].[CourseLearnDirectClassification]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseLearnDirectClassification (AuditOperation, CourseId, LearnDirectClassificationRef, ClassificationOrder)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, CourseId, LearnDirectClassificationRef, ClassificationOrder FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_CourseLearnDirectClassification_Delete]
    ON [dbo].[CourseLearnDirectClassification]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseLearnDirectClassification (AuditOperation, CourseId, LearnDirectClassificationRef, ClassificationOrder)
		(SELECT 'D', CourseId, LearnDirectClassificationRef, ClassificationOrder FROM deleted);
    END