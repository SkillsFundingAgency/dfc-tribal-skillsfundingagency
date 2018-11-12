CREATE PROCEDURE [dbo].[up_ReportProviderQualityEmailHistory]
		@ProviderId int
		, @StartDate datetime
		, @EndDate datetime
AS

/*
*	Name:		[up_ReportProviderQualityEmailHistory]
*	System: 	Stored procedure interface module
*	Description:	Quality email history
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2015
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	SELECT 
		p.Ukprn
		, l.ProviderId
		, p.ProviderName
		, pt.ProviderTypeName
		, l.ModifiedDateTimeUtc
		, l.TrafficLightStatusId
		, l.SFAFunded
		, l.DFE1619Funded
		, l.QualityEmailsPaused
		, l.HasValidRecipients
		, l.EmailDateTimeUtc
		, t1.Name EmailTemplateName
		, l.NextEmailDateTimeUtc
		, t2.Name NextEmailTemplateName
	FROM QualityEmailLog l
		LEFT JOIN Provider p ON p.ProviderId = l.ProviderId
		LEFT JOIN ProviderType pt on pt.ProviderTypeId = p.ProviderTypeId
		LEFT JOIN EmailTemplate t1 on t1.EmailTemplateId = l.EmailTemplateId
		LEFT JOIN EmailTemplate t2 on t2.EmailTemplateId = l.NextEmailTemplateId
	WHERE
		(@ProviderId IS NULL OR p.ProviderId = @ProviderId)
		AND (@StartDate IS NULL OR l.CreatedDateTimeUtc >= @StartDate)
		AND (@EndDate IS NULL OR l.CreatedDateTimeUtc <= @EndDate)
		AND l.CreatedDateTimeUtc IS NOT NULL -- Earlier values may be inaccurate
	ORDER BY l.QualityEmailLogId DESC

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1
	END
	
	RETURN 0

END