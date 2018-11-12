CREATE TABLE [dbo].[CourseInstanceVenue] (
    [CourseInstanceId] INT NOT NULL,
    [VenueId]          INT NOT NULL,
    CONSTRAINT [PK_CourseInstanceVenue] PRIMARY KEY CLUSTERED ([CourseInstanceId] ASC, [VenueId] ASC),
    CONSTRAINT [FK_CourseInstanceVenue_CourseInstance] FOREIGN KEY ([CourseInstanceId]) REFERENCES [dbo].[CourseInstance] ([CourseInstanceId]),
    CONSTRAINT [FK_CourseInstanceVenue_Venue] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venue] ([VenueId])
);


GO

CREATE TRIGGER [dbo].[Trigger_CourseInstanceVenue_InsertUpdate]
    ON [dbo].[CourseInstanceVenue]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstanceVenue (AuditOperation, CourseInstanceId, VenueId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, CourseInstanceId, VenueId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_CourseInstanceVenue_Delete]
    ON [dbo].[CourseInstanceVenue]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_CourseInstanceVenue (AuditOperation, CourseInstanceId, VenueId)
		(SELECT 'D', CourseInstanceId, VenueId FROM deleted);
    END
GO

CREATE INDEX [IX_CourseInstanceVenue_VenueId] ON [dbo].[CourseInstanceVenue] ([VenueId])
GO
