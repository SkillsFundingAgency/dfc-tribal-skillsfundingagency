CREATE PROCEDURE [dbo].[usp_WriteDWHLog]
	@LogType		NVARCHAR(25),
	@Message		NVARCHAR(1000)
AS

BEGIN
	
	INSERT INTO DWH_Log ([LogType], [Message]) VALUES (@LogType, @Message);

END;