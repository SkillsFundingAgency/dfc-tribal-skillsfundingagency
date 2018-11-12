CREATE PROCEDURE [UCAS_PG].[up_UCAS_PrepareForImport]
AS

BEGIN

	DELETE FROM [UCAS_PG].[Course];
	DELETE FROM [UCAS_PG].[Provider];
	DELETE FROM [UCAS_PG].[Location];
	DELETE FROM [UCAS_PG].[CourseOption];
	DELETE FROM [UCAS_PG].[CourseOptionFee];

	RETURN 0;

END;
