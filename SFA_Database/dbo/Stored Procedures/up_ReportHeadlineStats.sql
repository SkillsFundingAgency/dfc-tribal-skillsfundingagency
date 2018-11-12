CREATE PROCEDURE [dbo].[up_ReportHeadlineStats]
AS

/*
*	Name:		[up_ReportHeadlineStats]
*	System: 	Stored procedure interface module
*	Description:	Headline Statistics Report
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2017
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	DECLARE @LiveStatusId INT = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	SELECT Sum(QS.Courses) AS NumberOfCourses,
		Sum(QS.CourseInstances) AS NumberOfOpportunities,
		Sum(CASE WHEN QS.AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END) AS NumberOfPoorProviders,
		Sum(CASE WHEN QS.AutoAggregateQualityRating >= 51 AND AutoAggregateQualityRating < 71 THEN 1 ELSE 0 END) AS NumberOfAverageProviders,
		Sum(CASE WHEN QS.AutoAggregateQualityRating >= 71 AND AutoAggregateQualityRating < 91 THEN 1 ELSE 0 END) AS NumberOfGoodProviders,
		Sum(CASE WHEN QS.AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END) AS NumberOfVeryGoodProviders,
		Avg(QS.AutoAggregateQualityRating) AS AverageQualityScorePercent,
		CASE WHEN Avg(QS.AutoAggregateQualityRating) < 51 THEN 'Poor'
			 WHEN Avg(QS.AutoAggregateQualityRating) < 71 THEN 'Average'
			 WHEN Avg(QS.AutoAggregateQualityRating) < 91 THEN 'Good'
			 ELSE 'Very Good' END AS AverageQualityScoreText,
		Sum(CASE WHEN QS.Courses = 0 AND P.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS ZeroCourses
	FROM QualityScore QS
		INNER JOIN Provider P ON P.ProviderId = QS.ProviderId
	WHERE P.IsTASOnly = 0
		AND P.SFAFunded = 1
		AND P.RecordStatusId = @LiveStatusId;
		
	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;