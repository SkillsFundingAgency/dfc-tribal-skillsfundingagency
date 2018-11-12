CREATE VIEW [search].[v_VenueLocation]
	AS
	SELECT v1.VenueLocationId
		, v1.Latitude
		, v1.Longitude
		, v1.Geography
		, v1.Easting
		, v1.Northing
		, v1.LocationName LocationName1
		, v2.LocationName LocationName2
		, v3.LocationName LocationName3
		, v4.LocationName LocationName4
		, v5.LocationName LocationName5
		, v6.LocationName LocationName6
		, v7.LocationName LocationName7
		, v8.LocationName LocationName8
		, v9.LocationName LocationName9
		, v1.NearestPostcode
	FROM VenueLocation v1
		LEFT JOIN VenueLocation v2 ON v2.VenueLocationId = v1.ParentVenueLocationId
		LEFT JOIN VenueLocation v3 ON v3.VenueLocationId = v2.ParentVenueLocationId
		LEFT JOIN VenueLocation v4 ON v4.VenueLocationId = v3.ParentVenueLocationId
		LEFT JOIN VenueLocation v5 ON v5.VenueLocationId = v4.ParentVenueLocationId
		LEFT JOIN VenueLocation v6 ON v6.VenueLocationId = v5.ParentVenueLocationId
		LEFT JOIN VenueLocation v7 ON v7.VenueLocationId = v6.ParentVenueLocationId
		LEFT JOIN VenueLocation v8 ON v8.VenueLocationId = v7.ParentVenueLocationId
		LEFT JOIN VenueLocation v9 ON v9.VenueLocationId = v8.ParentVenueLocationId
