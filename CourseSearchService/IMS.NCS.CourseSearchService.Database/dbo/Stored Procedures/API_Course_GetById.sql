CREATE PROCEDURE [dbo].[API_Course_GetById]
(	 
	@CourseId	INT = NULL,
 	@PublicAPI	INT = 1
)

AS

SELECT
	/*COURSE INFORMATION*/
	[C].[CourseId],
	[C].[CourseTitle],
	[C].[QualificationTypeName] AS [QualificationTypeRef],
	[C].[QualificationLevelName] as Qualification_Level,
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
	[CI].[DurationAsText] as Duration_Description,
	[CI].[DurationUnitId] as DurationValue,
	[CI].[DurationUnitName] as DurationUnit,
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
	[CI1].[Url] as CourseInstanceUrl,
	[CIA10].[A10FundingCode],
	[CI].[CourseInstanceId] OpportunityId
	from
	[dbo].[Course] C
		INNER JOIN [dbo].[Course1]  C1 on C1.CourseId=C.CourseId
		INNER JOIN [dbo].[Course2]  C2 on C2.CourseId=C.CourseId
		INNER JOIN [dbo].[Course3]  C3 on C3.CourseId=C.CourseId
		INNER JOIN [dbo].[Course4]  C4 on C4.CourseId=C.CourseId
		INNER JOIN [DBO].[Provider]  P on C.ProviderId=P.ProviderId
		INNER JOIN [DBO].[CourseInstance]  CI on CI.CourseId=C.CourseId
		INNER JOIN [DBO].[CourseInstance1]  CI1 on CI1.CourseInstanceId=CI.CourseInstanceId
		INNER JOIN [dbo].[CourseInstanceA10FundingCode] CIA10 on CIA10.CourseInstanceId= CI.CourseInstanceId
		LEFT OUTER JOIN [dbo].[CourseInstancePriceDescription] CIPD ON CIPD.CourseInstanceId = CI.CourseInstanceId
		OUTER APPLY (
			SELECT Min([SD].[StartDate]) StartDate
			FROM [dbo].[CourseInstanceStartDate] SD
			WHERE [SD].[CourseInstanceId] = [CI].[CourseInstanceId]
		) CISD
WHERE 
	 C.CourseId= ISNULL(@CourseId,C.CourseId) 
	 AND (@PublicAPI = 0 OR C.ApplicationId != 3)
 

