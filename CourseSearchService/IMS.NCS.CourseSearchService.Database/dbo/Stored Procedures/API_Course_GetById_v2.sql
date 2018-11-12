CREATE PROCEDURE [dbo].[API_Course_GetById_v2]
(	 
	@CourseId	INT = NULL,
 	@PublicAPI	INT = 1,
	@APIKey		NVARCHAR(50) = NULL
)

AS

BEGIN

	-- If this is the public API then ensure that we have a valid API Key
	IF ([dbo].[IsValidAPIKey](@PublicAPI, @APIKey) = 0)
	BEGIN
		RETURN 0;
	END;

	SELECT /*COURSE INFORMATION*/
		[C].[CourseId],
		[C].[CourseTitle],
		[C].[QualificationTypeName] AS [QualificationTypeRef],
		[C].[QualificationLevelName] AS Qualification_Level,
		[C].[LDCS1],
		[C].[LDCS2],
		[C].[LDCS3],
		[C].[LDCS4],
		[C].[LDCS5],
		/*Number of opportunity*/
		[C].[CourseSummary],
		[C].[AwardingOrganisationName],
		[C2].[AssessmentMethod],
		[C4].[BookingUrl],
		[C].[UcasTariffPoints],
		[C].[QualificationDataType],
		[C1].[EntryRequirements],
		[C3].[EquipmentRequired],
		[C].[LearningAimRef],
		[C].[QualificationRefAuthority],
		[C].[QualificationRef],
		[C].[QualificationTitle],
		[C4].[Url],
		/** PROVIDER INFORMATION **/
		[P].ProviderId,
		[P].[ProviderName],
		[P].AddressLine1,
		[P].AddressLine2,
		[P].[Town],
		[P].[County],
		[P].[Postcode],
		[P].[Telephone],
		[P].[Website],
		[P].[Ukprn],
		[P].[Fax],
		[P].[Email],
		[P].[UPIN],
		[P].[Loans24Plus],
		 /**OPPORTUNITY RESULT**/
		[CI].[AttendanceModeName] AS [AttendanceModeBulkUploadRef],
		[CI].[AttendancePatternName] AS [AttendancePatternBulkUploadRef],
		[CI].[DurationAsText] AS Duration_Description,
		[CI].[DurationUnitId] AS DurationValue,
		[CI].[DurationUnitName] AS DurationUnit,
		[CI].[Price],
		[CIPD].[PriceAsText],
		[CISD].[StartDate],
		[CI].[StartDateDescription],
		[CI].[StudyModeName] AS [StudyModeBulkUploadRef],
		[CI].[TimeTable],
		[CI].[RegionName],
		[CI].[VenueId],
		[CI].[CanApplyAllYear],
		[CI].[ApplyFromDate],
		[CI].[ApplyTo],
		[CI].[ApplyUntilDate],
		[CI].[ApplyUntilText],
		[CI].[EndDate],
		[CI].[EnquiryTo],
		[CI].[LanguageOfAssessment],
		[CI].[LanguageOfInstruction],
		[CI].[PlacesAvailable],
		[CI].[ProviderOwnCourseInstanceRef],
		[CI1].[Url] AS CourseInstanceUrl,
		[CIA10].[A10FundingCode],
		[CI].[CourseInstanceId] OpportunityId,
		[CI].[DfEFunded] AS CourseDfEFunded,
		[P].FEChoices_LearnerDestination,
		[P].FEChoices_LearnerSatisfaction,
		[P].FEChoices_EmployerSatisfaction
	FROM [dbo].[Course] C
		INNER JOIN [dbo].[Course1] C1 ON C1.CourseId = C.CourseId
		INNER JOIN [dbo].[Course2] C2 ON C2.CourseId = C.CourseId
		INNER JOIN [dbo].[Course3] C3 ON C3.CourseId = C.CourseId
		INNER JOIN [dbo].[Course4] C4 ON C4.CourseId = C.CourseId
		INNER JOIN [dbo].[Provider] P ON C.ProviderId = P.ProviderId
		INNER JOIN [dbo].[CourseInstance] CI ON CI.CourseId = C.CourseId
		INNER JOIN [dbo].[CourseInstance1] CI1 on CI1.CourseInstanceId=CI.CourseInstanceId
		LEFT OUTER JOIN [dbo].[CourseInstanceA10FundingCode] CIA10 ON CIA10.CourseInstanceId = CI.CourseInstanceId
		LEFT OUTER JOIN [dbo].[CourseInstancePriceDescription] CIPD ON CIPD.CourseInstanceId = CI.CourseInstanceId
		OUTER APPLY (
			SELECT Min([SD].[StartDate]) StartDate
			FROM [dbo].[CourseInstanceStartDate] SD
			WHERE SD.CourseInstanceId = CI.CourseInstanceId
		) CISD
	WHERE C.CourseId = IsNull(@CourseId, C.CourseId) 
		AND (@PublicAPI = 0 OR C.ApplicationId != 3);
 
 END;