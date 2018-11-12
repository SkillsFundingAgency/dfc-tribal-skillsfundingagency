CREATE PROCEDURE [search].[PublishAllData]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

	EXEC [search].[PublishCategoryCode];
	EXEC [search].[PublishCourse];
	EXEC [search].[PublishCourseInstance];
	EXEC [search].[PublishCourseInstanceA10FundingCode];
	EXEC [search].[PublishCourseInstanceStartDate];
	EXEC [search].[PublishCourseText];
	EXEC [search].[PublishLearningAim];
	EXEC [search].[PublishProvider];
	EXEC [search].[PublishProviderText];
	EXEC [search].[PublishVenue];
	EXEC [search].[PublishVenueText];
	EXEC [search].[PublishCountyAlias];
	EXEC [search].[PublishVenueLocation];

	RETURN 0;

END;