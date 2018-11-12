CREATE TABLE [dbo].[CourseInstanceStartDate] (
    [CourseInstanceStartDateId] INT  IDENTITY (1, 1) NOT NULL,
    [CourseInstanceId]          INT  NOT NULL,
    [StartDate]                 DATE NOT NULL,
    [IsMonthOnlyStartDate]      BIT  CONSTRAINT [DF_CourseInstanceStartDates_IsMonthOnlyStartDate] DEFAULT ((0)) NOT NULL,
    [PlacesAvailable] INT NULL, 
    CONSTRAINT [PK_CourseInstanceStartDates] PRIMARY KEY CLUSTERED ([CourseInstanceStartDateId] ASC),
    CONSTRAINT [FK_CourseInstanceStartDate_CourseInstance] FOREIGN KEY ([CourseInstanceId]) REFERENCES [dbo].[CourseInstance] ([CourseInstanceId])
);


GO

CREATE TRIGGER [dbo].[Trigger_CourseInstanceStartDate_InsertUpdate]
    ON [dbo].[CourseInstanceStartDate]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstanceStartDate (AuditOperation, CourseInstanceStartDateId, CourseInstanceId, StartDate, IsMonthOnlyStartDate, PlacesAvailable)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, CourseInstanceStartDateId, CourseInstanceId, StartDate, IsMonthOnlyStartDate, PlacesAvailable FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_CourseInstanceStartDate_Delete]
    ON [dbo].[CourseInstanceStartDate]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstanceStartDate (AuditOperation, CourseInstanceStartDateId, CourseInstanceId, StartDate, IsMonthOnlyStartDate, PlacesAvailable)
		(SELECT 'D', CourseInstanceStartDateId, CourseInstanceId, StartDate, IsMonthOnlyStartDate, PlacesAvailable FROM deleted);
    END
GO

CREATE INDEX [IX_CourseInstanceStartDate_CourseInstanceId] ON [dbo].[CourseInstanceStartDate] ([CourseInstanceId])
