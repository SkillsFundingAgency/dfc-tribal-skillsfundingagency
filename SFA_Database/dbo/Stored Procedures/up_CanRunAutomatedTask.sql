CREATE PROCEDURE [dbo].[up_CanRunAutomatedTask]
	@TaskName nvarchar(50)
AS

BEGIN

	IF (@TaskName IS NULL) 
	BEGIN
		RETURN 0;
	END;

	-- If a record for this task doesn't exist then create it
	IF NOT EXISTS (SELECT * FROM dbo.AutomatedTask WHERE TaskName = @TaskName)
	BEGIN
		BEGIN TRY
			INSERT INTO dbo.AutomatedTask (TaskName, InProgress) VALUES (@TaskName, 0);
		END TRY
		BEGIN CATCH
		END CATCH;
	END;

	-- If we can set InProgress to 1 where InProgress is currently 0 then nothing else is running this task
	-- If the flag is still set more than 20 hours later then assume there's an issue and reset
	UPDATE dbo.AutomatedTask SET InProgress = 1, DateTimeUtc = GetUtcDate() WHERE TaskName = @TaskName AND (InProgress = 0 OR DATEADD(hh, 20, DateTimeUtc) < GetUtcDate());

	IF (@@ROWCOUNT = 1)
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
