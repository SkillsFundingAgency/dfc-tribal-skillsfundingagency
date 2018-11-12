CREATE PROCEDURE [UCAS].[up_UCAS_PrepareForImport]
	@Incremental	BIT
AS

BEGIN

	DELETE FROM [UCAS].[Deletions];

	IF @Incremental = 0 /* Full */
		BEGIN
			DELETE FROM [UCAS].[CourseEntry];
			DELETE FROM [UCAS].[Courses];
			DELETE FROM [UCAS].[CoursesIndex];
			DELETE FROM [UCAS].[Currencies];
			DELETE FROM [UCAS].[Fees];
			DELETE FROM [UCAS].[FeeYears];
			DELETE FROM [UCAS].[Orgs];
			DELETE FROM [UCAS].[PlacesOfStudy];
			DELETE FROM [UCAS].[Qualifications];
			DELETE FROM [UCAS].[Starts];
			DELETE FROM [UCAS].[StartsIndex];
			DELETE FROM [UCAS].[Towns];
		END;

	RETURN 0;

END;
