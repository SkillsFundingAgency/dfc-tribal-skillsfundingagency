CREATE PROCEDURE [dbo].[API_Venue_GetById]
(	 
	 @VenueId	INT,
	 @PublicAPI	INT = 1
)
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 

AS BEGIN --ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = 'English')

	SELECT 
		/*Venue*/
		[V].[VenueName],
		[V].[AddressLine1],
		[V].[AddressLine2],
		[V].[Town],
		[V].[County],
		[V].[Postcode],
		[V].[Latitude],
		[V].[Longitude],
		[V].[Telephone],
		[V].[Email],
		[V1].[Website],
		[V].[Fax],
		[V].[Facilities]
	  FROM 
		[dbo].[Venue] V
		INNER JOIN [dbo].[Venue1] [V1] on [V].[VenueId] = [V1].[VenueId]
	  WHERE
		V.VenueId=@VenueId
		AND (@PublicAPI = 0 OR V.ApplicationId != 3);

 END;
