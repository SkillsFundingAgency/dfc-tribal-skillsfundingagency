CREATE FUNCTION [dbo].[GetApprenticeshipName]
(
	@ApprenticeshipId int
)
RETURNS NVARCHAR(4000)
AS
BEGIN
	DECLARE @Name NVARCHAR(1000)
	SELECT @Name = 
		CASE
			WHEN a.FrameworkCode IS NOT NULL AND f.PathwayName IS NULL THEN
				f.NasTitle + ' - ' + pt.ProgTypeDesc
			WHEN a.FrameworkCode IS NOT NULL AND f.PathwayName IS NOT NULL THEN
				f.NasTitle + ' - ' + pt.ProgTypeDesc + ' - '  + f.PathwayName
			WHEN a.StandardCode IS NOT NULL THEN
				ssc.StandardSectorCodeDesc + ' - ' + s.StandardName
			ELSE 'No framework or standard specified'
		END
	FROM Apprenticeship a
		LEFT JOIN Framework f ON f.FrameworkCode = a.FrameworkCode
								 AND f.ProgType = a.ProgType
								 AND f.PathwayCode = a.PathwayCode
		LEFT JOIN ProgType pt on pt.ProgTypeId = a.ProgType
		LEFT JOIN Standard s ON s.StandardCode = a.StandardCode
								AND s.Version = a.Version
		LEFT JOIN StandardSectorCode ssc ON ssc.StandardSectorCodeId = s.StandardSectorCode
	WHERE a.ApprenticeshipId = @ApprenticeshipId

	RETURN ISNULL(@Name, '')
END