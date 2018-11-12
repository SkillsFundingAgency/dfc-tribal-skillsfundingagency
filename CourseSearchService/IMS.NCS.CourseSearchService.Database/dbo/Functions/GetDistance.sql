CREATE  Function DBO.GetDistance(@from varchar(10),@to varchar(10))
Returns Float
As
BEGIN

DECLARE @pointA geography;
DECLARE @pointB geography;

Declare @pointA_Lat nvarchar(100);
Declare @pointA_Lon nvarchar(100);

Declare @pointB_Lat nvarchar(100);
Declare @pointB_Lon nvarchar(100);

select @pointA_Lat=Lat, @pointA_Lon=lng from GeoLocation where Postcode=@from
select @pointB_Lat=Lat, @pointB_Lon=lng from GeoLocation where Postcode=@to

SET @pointA = geography::STGeomFromText('POINT(' + @pointA_Lon +' ' + @pointA_Lat +')', 4326);
SET @pointB = geography::STGeomFromText('POINT(' + @pointB_Lon +' ' + @pointB_Lat +')', 4326); 

Return  @pointA.STDistance(@pointB)/1609.344;

END