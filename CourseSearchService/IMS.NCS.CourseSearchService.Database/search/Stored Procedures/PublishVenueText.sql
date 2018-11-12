CREATE PROCEDURE [search].[PublishVenueText]
AS
BEGIN
	DELETE FROM [dbo].[VenueText];

	INSERT INTO [dbo].[VenueText] (
		[VenueTextId],
		[VenueId],
		[ProviderId],
		[SearchText]
	)
	SELECT [VenueTextId],
		[VenueId],
		[ProviderId],
		[SearchText]
	FROM [search].[VenueText];

	RETURN 0;

END;
