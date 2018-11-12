CREATE TRIGGER [TR_AspNetUsers_Update]
ON [AspNetUsers] AFTER UPDATE
AS
	UPDATE [AspNetUsers]
	SET [ModifiedDateTimeUtc] = GetUtcDate()
	WHERE [ID] IN (SELECT DISTINCT Id FROM Inserted)
GO
