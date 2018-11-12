CREATE PROCEDURE [remote].[CopyOverAllData]
AS
BEGIN

	BEGIN TRANSACTION;
	  
	EXEC [remote].[CopyOverCategoryCode];
	EXEC [remote].[CopyOverCourse];
	EXEC [remote].[CopyOverCourseInstance];
	EXEC [remote].[CopyOverCourseInstanceA10FundingCode];
	EXEC [remote].[CopyOverCourseInstanceStartDate];
	EXEC [remote].[CopyOverCourseText];
	EXEC [remote].[CopyOverLearningAim];
	EXEC [remote].[CopyOverProvider];
	EXEC [remote].[CopyOverProviderText];
	EXEC [remote].[CopyOverVenue];
	EXEC [remote].[CopyOverVenueLocation];
	EXEC [remote].[CopyOverCountyAlias];
	EXEC [remote].[CopyOverVenueText];
		
	COMMIT TRANSACTION;
	  
	RETURN 0;

END;