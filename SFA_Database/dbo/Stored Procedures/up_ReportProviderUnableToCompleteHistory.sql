CREATE PROCEDURE [dbo].[up_ReportProviderUnableToCompleteHistory]
		@ShowAllProviders bit
		, @ProviderId int
AS

/*
*	Name:		[up_ReportProviderQAHistory]
*	System: 	Stored procedure interface module
*	Description:	List provider QA history
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2016
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

		SELECT P.ProviderId,
			p.Ukprn Ukprn
			,p.ProviderName ProviderName
			, [dbo].[GetUnableToCompleteFailureReasonsCommaDelimited](qac.ProviderUnableToCompleteId) UnableToCompleteReasonChecks
			, qac.TextUnableToComplete UnableToCompleteText
			, qac.CreatedDateTimeUtc UnableToCompleteDateCreated
			, u.Name UnableToCompleteUsername
		FROM ProviderUnableToComplete qac
			JOIN Provider p on p.ProviderId = qac.ProviderId
			JOIN AspNetUsers u on u.Id = qac.CreatedByUserId
		WHERE (@ShowAllProviders = 1 OR p.ProviderId = @ProviderId) and qac.Active = 1	
		ORDER BY qac.CreatedDateTimeUtc DESC;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;