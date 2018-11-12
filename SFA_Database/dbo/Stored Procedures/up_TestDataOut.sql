CREATE PROCEDURE up_TestDataOut
AS


-- Example shows calculating distance from long/lat to the venue which has an address with Long lat

DECLARE @RangeDivisor FLOAT = 1609.344
DECLARE @Latitude FLOAT, @Longitude FLOAT
SET @Latitude = 53.373458    -- The Tribal Sheffield office
SET @Longitude = -1.470429   -- The Tribal Sheffield office


SELECT *,
A.Geography.STDistance(geography::Point(@Latitude, @Longitude, 4326))/@RangeDivisor [Distance]
FROM Course C
INNER JOIN CourseInstance CI ON C.CourseId = CI.CourseId
INNER JOIN CourseInstanceVenue CIV ON CIV.CourseInstanceId = CI.CourseId
INNER JOIN Venue V ON CIV.VenueId = V.VenueId
INNER JOIN [Address] A  ON V.AddressId = A.AddressId
INNER JOIN Provider P ON C.ProviderId = P.ProviderId
LEFT OUTER JOIN CourseInstanceStartDate CSD ON CI.CourseInstanceId = CSD.CourseInstanceId
LEFT OUTER JOIN CourseInstanceA10FundingCode CAFC ON CI.CourseInstanceId = CAFC.CourseInstanceId
LEFT OUTER JOIN A10FundingCode A10FC ON CAFC.A10FundingCode = A10FC.A10FundingCodeId
LEFT OUTER JOIN StudyMode SM ON CI.StudyModeId = SM.StudyModeId
LEFT OUTER JOIN AttendancePattern AP ON CI.AttendancePatternId = AP.AttendancePatternId
WHERE A.Postcode not in (SELECT PrisonPostcode from Prison) -- Where clause should exclude courses run at prison locations

IF(@@ERROR <> 0) RETURN 1 ELSE RETURN 0