CREATE PROCEDURE [dbo].[up_BulkUploadProviderClearData] (
	@ProviderId INT,
	@ClearCourseData BIT,
	@ClearApprenticeshipData BIT
)
AS
BEGIN

	IF (@ClearCourseData = 1)
	BEGIN
		DELETE [a10]
		  FROM [dbo].[CourseInstanceA10FundingCode] [a10]
		  JOIN [dbo].[CourseInstance] [ci]
			ON [ci].[CourseInstanceId] = [a10].[CourseInstanceId]
		  JOIN [dbo].[Course] [c]
			ON [ci].[CourseId] = [c].[CourseId]
		 WHERE [c].[ProviderId] = @ProviderId

		DELETE [cisd]
		  FROM [dbo].[CourseInstanceStartDate] [cisd]
		  JOIN [dbo].[CourseInstance] [ci]
			ON [cisd].[CourseInstanceId] = [ci].[CourseInstanceId]
		  JOIN [dbo].[Course] [c]
			ON [ci].[CourseId] = [c].[CourseId]
		 WHERE [c].[ProviderId] = @ProviderId

		DELETE [civ]
		  FROM [dbo].[CourseInstanceVenue] [civ]
		  JOIN [dbo].[CourseInstance] [ci]
			ON [civ].[CourseInstanceId] = [ci].[CourseInstanceId]
		  JOIN [dbo].[Course] [c]
			ON [ci].[CourseId] = [c].[CourseId]
		 WHERE [c].[ProviderId] = @ProviderId

		DELETE [ci] 
		  FROM [dbo].[Course] [c]
		  JOIN [dbo].[CourseInstance] [ci]
			ON [c].[CourseId] = [ci].[CourseId]
		 WHERE [c].[ProviderId] = @ProviderId

		DELETE [cldc] 
		  FROM [dbo].[Course] [c]
		  JOIN [dbo].[CourseLearnDirectClassification] [cldc]
			ON [c].[CourseId] = [cldc].[CourseId]
		 WHERE [c].[ProviderId] = @ProviderId

		DELETE 
		  FROM [dbo].[Course]
		 WHERE [ProviderId] = @ProviderId


		CREATE TABLE #VenueAddresses (AddressId int) 

		INSERT INTO #VenueAddresses
		SELECT addressID FROM [dbo].Venue WHERE ProviderId = @ProviderId

		DELETE
		  FROM [dbo].[Venue]
		 WHERE [ProviderId] = @ProviderId

		DELETE [a]
		FROM [dbo].[Address] [a]
		JOIN #VenueAddresses [va] on [va].AddressId = [a].AddressId

	   DROP TABLE #VenueAddresses
	END

	IF (@ClearApprenticeshipData = 1)
	BEGIN
		 DELETE [aqacfr]
		 FROM ApprenticeshipQAComplianceFailureReason [aqacfr]
			INNER JOIN [dbo].ApprenticeshipQACompliance [aqac] ON [aqac].ApprenticeshipQAComplianceId = [aqacfr].ApprenticeshipQAComplianceId
			INNER JOIN [dbo].Apprenticeship [a] on [a].ApprenticeshipId = [aqac].ApprenticeshipId
		 WHERE [a].ProviderId = @ProviderId;

		 DELETE [aqac]
		   FROM [dbo].ApprenticeshipQACompliance [aqac]
		   JOIN [dbo].Apprenticeship [a] on [a].ApprenticeshipId = [aqac].ApprenticeshipId
		  WHERE [a].ProviderId = @ProviderId;

		 DELETE [aqasfr]
		 FROM ApprenticeshipQAStyleFailureReason [aqasfr]
			INNER JOIN [dbo].ApprenticeshipQAStyle [aqas] ON [aqas].ApprenticeshipQAStyleId = [aqasfr].ApprenticeshipQAStyleId
			INNER JOIN [dbo].Apprenticeship [a] on [a].ApprenticeshipId = [aqas].ApprenticeshipId
		 WHERE [a].ProviderId = @ProviderId;

		 DELETE [aqas]
		   FROM [dbo].ApprenticeshipQAStyle [aqas]
		   JOIN [dbo].Apprenticeship [a] on [a].ApprenticeshipId = [aqas].ApprenticeshipId
		  WHERE [a].ProviderId = @ProviderId;

		 DELETE [aldm]
		   FROM [dbo].ApprenticeshipLocationDeliveryMode [aldm]
		   JOIN [dbo].ApprenticeshipLocation [al] on [al].ApprenticeshipLocationId = [aldm].ApprenticeshipLocationId
		   JOIN [dbo].Apprenticeship [a] on [a].ApprenticeshipId = [al].ApprenticeshipId
		  WHERE [a].ProviderId = @ProviderId

		 DELETE [al]
		   FROM ApprenticeshipLocation [al]
		   JOIN [dbo].Apprenticeship [a] on [a].ApprenticeshipId = [al].ApprenticeshipId
		  WHERE [a].ProviderId = @ProviderId

		 DELETE [a]
		   FROM [dbo].Apprenticeship [a]
		  WHERE [a].ProviderId = @ProviderId

		 CREATE TABLE #LocationAddresses (AddressId int) 

		 INSERT INTO #LocationAddresses
		 SELECT addressID FROM [dbo].Location WHERE ProviderId = @ProviderId

		 DELETE
		   FROM [dbo].[Location]
		  WHERE [ProviderId] = @ProviderId

		 DELETE [a]
		   FROM [dbo].[Address] [a]
		   JOIN #LocationAddresses [la] on [la].AddressId = [a].AddressId

		 DROP TABLE #LocationAddresses
	END

END