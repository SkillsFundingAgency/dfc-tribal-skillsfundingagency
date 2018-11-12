CREATE PROCEDURE [remote].[CopyOverVenueText]
AS
BEGIN

	TRUNCATE TABLE [search].[VenueText];

	DBCC CHECKIDENT([Search.VenueText], RESEED, 1);  -- Reseed the identity column because we are adding approx 22 million rows per night and therefore the int column runs out of space in around 100 days.

	INSERT INTO [search].[VenueText] (
		[VenueId]
		, [ProviderId]
		, [SearchText]
	)
	SELECT DISTINCT
		[VenueId]
		, [ProviderId]
		, Upper([r].[Data])
	FROM [remote].[VenueText]
		CROSS APPLY Split([SearchText], '|') r;

	RETURN 0;

END;
