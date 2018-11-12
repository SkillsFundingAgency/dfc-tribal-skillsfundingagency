CREATE FUNCTION [dbo].[GetPointForPostcode](@Postcode VARCHAR(10))
RETURNS GEOGRAPHY

AS

BEGIN

	DECLARE @pointA GEOGRAPHY;

	DECLARE @pointA_Lat NVARCHAR(20);
	DECLARE @pointA_Lon NVARCHAR(20);

	SELECT @pointA_Lat=Lat, @pointA_Lon=lng FROM GeoLocation WHERE Postcode = @Postcode

	IF @pointA_Lat IS NULL
		RETURN NULL;

	SET @pointA = geography::STGeomFromText('POINT(' + @pointA_Lon +' ' + @pointA_Lat +')', 4326); 

	RETURN @pointA;

END;