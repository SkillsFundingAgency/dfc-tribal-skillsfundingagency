CREATE FUNCTION [dbo].[IsValidAPIKey]
(
	@PublicAPI				INT,
	@APIKey					NVARCHAR(50)
)
RETURNS BIT
AS 
BEGIN
	-- If this is the public API then ensure that we have a valid API Key
	IF (@PublicAPI = 1)
	BEGIN
		IF NOT EXISTS (SELECT PublicAPIUserId FROM [remote].[PublicAPIUser] WHERE PublicAPIUserId = @APIKey AND RecordStatusId = 2)
			RETURN 0;
	END;

	RETURN 1;
END;
