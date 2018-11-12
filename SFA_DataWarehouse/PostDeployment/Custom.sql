/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------

All custom refactor scripts should run only once, the best way to do this is to use
one of the following templates. You should replace the X's with a GUID, print a sensible
description and add your SQL where indicated.

1. General changes not related to the language text fields

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
BEGIN
	PRINT '[Updating Widget Catalogue Information]'
	SET NOCOUNT ON

	-- SQL Code goes here

	INSERT INTO __RefactorLog (OperationKey) VALUES ('XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
END
GO

2. Changes to language text fields

To get the fully qualified field names:
	a. Go to the Portal Admin > Languages and download the current language or,
	b. Run the following SQL: EXEC [dbo].[up_LanguageTextListByLanguageId] 1, null
	                      or: EXEC [dbo].[up_LanguageTextListByKeyGroupId] 1, null, null, null, 1

Ensure you make corresponding changes to the call to AppGlobal.Language.GetText().

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
BEGIN
	PRINT '[Updating Widget Catalogue Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'The new text for the field'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Fully_Qualified_FieldName1', @NewText, @NewText

	SET @NewText = 'The new text for the field'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Fully_Qualified_FieldName2', @NewText, @NewText

	-- etc

	INSERT INTO __RefactorLog (OperationKey) VALUES ('XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
END
GO
*/

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '998C71E4-D690-427B-B7B0-4D2516217102')
BEGIN
	PRINT '[Running Month Ends]';
	SET NOCOUNT ON;

	DECLARE @DateToRun DATE;
	DECLARE @ReRun INT = 0;
	DECLARE @PeriodType VARCHAR(1) = 'M';
	DECLARE @PeriodName NVARCHAR(50);

	SELECT @DateToRun = DATEADD(MONTH, 1, NextPeriodStartDate) FROM DWH_Period_Latest WHERE PeriodType = @PeriodType;
	
	WHILE (@DateToRun <= GetUtcDate())
	BEGIN
		SELECT @PeriodName = PeriodName FROM DWH_Period_Latest WHERE PeriodType = @PeriodType;
		PRINT '[Running Month End for ' + @PeriodName + ']';
		EXEC [usp_PeriodEnd] @DateToRun = @DateToRun, @ReRun = @ReRun, @PeriodType = @PeriodType;
		SET @DateToRun = DATEADD(MONTH, 1, @DateToRun);
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('998C71E4-D690-427B-B7B0-4D2516217102');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4F904C53-72BD-4AA8-8A87-6368987331CA')
BEGIN
	PRINT '[Fixing Data]';
	SET NOCOUNT ON;

	DECLARE @PeriodToRun NVARCHAR(7)
	DECLARE @CurrentPeriod NVARCHAR(7);
	DECLARE @LiveStatusId INT = 2;

	SELECT @PeriodToRun = MIN(Period) FROM DWH_Period WHERE PeriodType = 'M';
	SELECT @CurrentPeriod = Period FROM DWH_Period_Latest WHERE PeriodType = 'M';

	WHILE @PeriodToRun <= @CurrentPeriod
	BEGIN

		DELETE FROM [DFEReport_Provision] WHERE Period = @PeriodToRun;

		INSERT [DFEReport_Provision]
		(
			[Period], 
			[NumberOfCourses],
			[NumberOfLiveCourses],
			[NumberOfOpportunities],
			[NumberOfLiveOpportunities],
			[ProvidersWithNoCourses]
			
		)
		SELECT @PeriodToRun,
			COALESCE(C.NumberOfCourses, 0) AS NumberOfCourses,
			COALESCE(C.NumberOfLiveCourses, 0) AS NumberOfLiveCourses,
			COALESCE(C.NumberOfOpportunities, 0) AS NumberOfOpportunities,
			COALESCE(C.NumberOfLiveOpportunities, 0) AS NumberOfLiveOpportunities,
			COALESCE(P.NumberOfProvidersWithNoCourses, 0) AS NumberOfProvidersWithNoCourses			
		FROM (
				SELECT Count(*) AS NumberOfCourses,
					Sum(CASE WHEN SC.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveCourses,
					Sum(COALESCE(NumOpportunities, 0)) AS NumberOfOpportunities,
					Sum(COALESCE(NumLiveOpportunities, 0)) AS NumberOfLiveOpportunities
				FROM Snapshot_Course SC
					LEFT JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.Period = SC.Period
					LEFT OUTER JOIN (
										SELECT SCI.CourseId, 
											CASE WHEN Sum(CASE WHEN A10.CourseInstanceId IS NOT NULL THEN 1 ELSE 0 END) = 0 THEN 0 ELSE 1 END AS IsDFE, 
											Count(*) AS NumOpportunities, 
											Sum(CASE WHEN SCI.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumLiveOpportunities 
										FROM Snapshot_CourseInstance SCI 
											LEFT OUTER JOIN (
																SELECT CourseInstanceId 
																FROM Snapshot_CourseInstanceA10FundingCode 
																WHERE Period = @PeriodToRun
																	AND A10FundingCode = 25 															
																GROUP BY CourseInstanceId
															) A10 ON A10.CourseInstanceId = SCI.CourseInstanceId
										WHERE SCI.[Period] = @PeriodToRun 
										GROUP BY SCI.CourseId
									) SCI ON SC.CourseId = SCI.CourseId
				WHERE SC.[Period] = @PeriodToRun
					AND SP.RecordStatusId = @LiveStatusId
					AND SP.DFE1619Funded = 1
					AND (SP.SFAFunded = 0 OR SCI.IsDFE = 1)
			) C,
			(
				SELECT Count(*) AS NumberOfProvidersWithNoCourses
				FROM Snapshot_Provider SP
				WHERE SP.[Period] = @PeriodToRun
					AND SP.ProviderId NOT IN (SELECT DISTINCT ProviderId FROM Snapshot_Course WHERE [Period] = @PeriodToRun AND RecordStatusId = @LiveStatusId)
					AND SP.DFE1619Funded = 1
					AND SP.RecordStatusId = @LiveStatusId
			) P;

		SELECT @PeriodToRun = Min(Period) FROM DWH_Period WHERE Period > @PeriodToRun AND PeriodType = 'M';

	END;


	INSERT INTO __RefactorLog (OperationKey) VALUES ('4F904C53-72BD-4AA8-8A87-6368987331CA');
END;
GO


--Fix Monthly Report Provision Data. Should only be pulling in data for SFA Providers
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4D97114D-78F1-4b7f-9556-7431D09475D2')
BEGIN
	PRINT '[Fixing Data]';
	SET NOCOUNT ON;

	DECLARE @PeriodToRun NVARCHAR(7)
	DECLARE @CurrentPeriod NVARCHAR(7);
	DECLARE @LiveStatusId INT = 2;

	SELECT @PeriodToRun = MIN(Period) FROM DWH_Period WHERE PeriodType = 'M';
	SELECT @CurrentPeriod = Period FROM DWH_Period_Latest WHERE PeriodType = 'M';

	WHILE @PeriodToRun <= @CurrentPeriod
	BEGIN

		DELETE FROM [MonthlyReport_Provision] WHERE Period = @PeriodToRun;

		INSERT [MonthlyReport_Provision]
		(
			[Period], 
			[NumberOfCourses],
			[NumberOfLiveCourses],
			[NumberOfOpportunities],
			[NumberOfLiveOpportunities],
			[ProvidersWithNoCourses],
			[SM_Flexible],
			[SM_FullTime],
			[SM_NotKnown],
			[SM_PartOfFullTimeProgramme],
			[SM_PartTime],
			[AM_DistanceWithAttendance],
			[AM_DistanceWithoutAttendance],
			[AM_FaceToFace],
			[AM_LocationCampus],
			[AM_MixedMode],
			[AM_NotKnown],
			[AM_OnlineWithAttendance],
			[AM_OnlineWithoutAttendance],
			[AM_WorkBased],
			[AP_Customised],
			[AP_DayBlockRelease],
			[AP_Daytime],
			[AP_Evening],
			[AP_NotApplicable],
			[AP_NotKnown],
			[AP_Twilight],
			[AP_Weekend],
			[DU_1WeekOrLess],
			[DU_1To4Weeks],
			[DU_1To3Months],
			[DU_3To6Months],
			[DU_6To12Months],
			[DU_1To2Years],
			[DU_NotKnown],
			[QT_14To19Diploma],
			[QT_AccessToHigherEducation],
			[QT_Apprenticeship],
			[QT_BasicKeySkill],
			[QT_CertificateOfAttendance],
			[QT_CourseProviderCertificate],
			[QT_ExternalAwardedQualification],
			[QT_FoundationalDegree],
			[QT_FunctionalSkill],
			[QT_GCEOrEquivalent],
			[QT_GCSEOrEquivalent],
			[QT_HncHnd],
			[QT_InternationalBacculaureate],
			[QT_NoQualification],
			[QT_NVQ],
			[QT_OtherAccreditedQualification],
			[QT_Postgraduate],
			[QT_Undergraduate],
			[QT_IndustrySpecificQualification],
			[QL_EntryLevel],
			[QL_HigherLevel],
			[QL_Level1],
			[QL_Level2],
			[QL_Level3],
			[QL_Level4],
			[QL_Level5],
			[QL_Level6],
			[QL_Level7],
			[QL_Level8],
			[QL_NotKnown]
		)
		SELECT @PeriodToRun,
			COALESCE(C.NumberOfCourses, 0),
			COALESCE(C.NumberOfLiveCourses, 0),
			COALESCE(C.NumberOfOpportunities, 0),
			COALESCE(C.NumberOfLiveOpportunities, 0),
			COALESCE(P.NumberOfProvidersWithNoCourses, 0),
			COALESCE(SM.Flexible, 0),
			COALESCE(SM.FullTime, 0),
			COALESCE(SM.NotKnown, 0),
			COALESCE(SM.PartOfFullTimeProgramme, 0),
			COALESCE(SM.PartTime, 0),
			COALESCE(AM.DistanceWithAttendance, 0),
			COALESCE(AM.DistanceWithoutAttendance, 0),
			COALESCE(AM.FaceToFace, 0),
			COALESCE(AM.LocationCampus, 0),
			COALESCE(AM.MixedMode, 0),
			COALESCE(AM.NotKnown, 0),
			COALESCE(AM.OnlineWithAttendance, 0),
			COALESCE(AM.OnlineWithoutAttendance, 0),
			COALESCE(AM.WorkBased, 0),
			COALESCE(AP.Customised, 0),
			COALESCE(AP.DayRelease, 0),
			COALESCE(AP.Daytime, 0),
			COALESCE(AP.Evening, 0),
			COALESCE(AP.NotApplicable, 0),
			COALESCE(AP.NotKnown, 0),
			COALESCE(AP.Twilight, 0),
			COALESCE(AP.Weekend, 0),
			COALESCE(DU.OneWeekOrLess, 0),
			COALESCE(DU.OneToFourWeeks, 0),
			COALESCE(DU.OneToThreeMonths, 0),
			COALESCE(DU.ThreeToSixMonths, 0),
			COALESCE(DU.SixToTwelveMonths, 0),
			COALESCE(DU.OneToTwoYears, 0),
			COALESCE(DU.NotKnown, 0),
			COALESCE(QT.Diploma, 0),
			COALESCE(QT.AccessToHigherEducation, 0),
			COALESCE(QT.Apprenticeship, 0),
			COALESCE(QT.BasicSkill, 0),
			COALESCE(QT.CertificateOfAttendance, 0),
			COALESCE(QT.CourseProviderCertificate, 0),
			COALESCE(QT.ExternalAward, 0),
			COALESCE(QT.FoundationDegree, 0),
			COALESCE(QT.FunctionalSkill, 0),
			COALESCE(QT.GCEOrEquivalent, 0),
			COALESCE(QT.GCSEOrEquivalent, 0),
			COALESCE(QT.HncHnd, 0),
			COALESCE(QT.InternationalBacculaureate, 0),
			COALESCE(QT.NoQualification, 0),
			COALESCE(QT.NVQ, 0),
			COALESCE(QT.AccreditedQualification, 0),
			COALESCE(QT.Postgraduate, 0),
			COALESCE(QT.Undergraduate, 0),
			COALESCE(QT.ProfessionalQualification, 0),
			COALESCE(QL.EntryLevel, 0),
			COALESCE(QL.HigherLevel, 0),
			COALESCE(QL.Level1, 0),
			COALESCE(QL.Level2, 0),
			COALESCE(QL.Level3, 0),
			COALESCE(QL.Level4, 0),
			COALESCE(QL.Level5, 0),
			COALESCE(QL.Level6, 0),
			COALESCE(QL.Level7, 0),
			COALESCE(QL.Level8, 0),
			COALESCE(QL.NotKnown, 0)
		FROM (
				SELECT Count(*) AS NumberOfCourses,
					Sum(CASE WHEN SC.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveCourses,
					Sum(COALESCE(NumOpportunities, 0)) AS NumberOfOpportunities,
					Sum(COALESCE(NumLiveOpportunities, 0)) AS NumberOfLiveOpportunities
				FROM Snapshot_Course SC					
					LEFT OUTER JOIN (
						SELECT CI.CourseId, Count(*) AS NumOpportunities, 
						Sum(CASE WHEN CI.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumLiveOpportunities 
						FROM Snapshot_CourseInstance CI
						Join Snapshot_Course C on CI.CourseId = C.CourseID and C.Period = @PeriodToRun
						JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
						WHERE CI.[Period] = @PeriodToRun GROUP BY CI.CourseId) SCI ON SCI.CourseId = SC.CourseId
				JOIN Snapshot_Provider P on SC.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SC.[Period] = @PeriodToRun and P.SFAFunded = 1
					
			 ) C,
			 (
				SELECT Count(*) AS NumberOfProvidersWithNoCourses
				FROM Snapshot_Provider SP
				WHERE SP.[Period] = @PeriodToRun
					AND SP.ProviderId NOT IN (SELECT DISTINCT ProviderId FROM Snapshot_Course WHERE [Period] = @PeriodToRun AND RecordStatusId = @LiveStatusId)
					AND SP.IsTASOnly = 0
					AND SP.RecordStatusId = @LiveStatusId
					AND SP.SFAFunded = 1
			 ) P,
			 (
				SELECT Sum(CASE WHEN StudyModeId = 4 THEN 1 ELSE 0 END) AS Flexible,
					Sum(CASE WHEN StudyModeId = 1 THEN 1 ELSE 0 END) AS FullTime,
					Sum(CASE WHEN StudyModeId = 5 THEN 1 ELSE 0 END) AS NotKnown,
					Sum(CASE WHEN StudyModeId = 3 THEN 1 ELSE 0 END) AS PartOfFullTimeProgramme,
					Sum(CASE WHEN StudyModeId = 2 THEN 1 ELSE 0 END) AS PartTime
				FROM Snapshot_CourseInstance SCI
					Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
					JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) SM,
			 (
				SELECT Sum(CASE WHEN AttendanceTypeId = 5 THEN 1 ELSE 0 END) AS DistanceWithAttendance,
					Sum(CASE WHEN AttendanceTypeId = 6 THEN 1 ELSE 0 END) AS DistanceWithoutAttendance,
					Sum(CASE WHEN AttendanceTypeId = 2 THEN 1 ELSE 0 END) AS FaceToFace,
					Sum(CASE WHEN AttendanceTypeId = 1 THEN 1 ELSE 0 END) AS LocationCampus,
					Sum(CASE WHEN AttendanceTypeId = 4 THEN 1 ELSE 0 END) AS MixedMode,
					Sum(CASE WHEN AttendanceTypeId = 9 THEN 1 ELSE 0 END) AS NotKnown,
					Sum(CASE WHEN AttendanceTypeId = 2 THEN 1 ELSE 0 END) AS OnlineWithAttendance,
					Sum(CASE WHEN AttendanceTypeId = 8 THEN 1 ELSE 0 END) AS OnlineWithoutAttendance,
					Sum(CASE WHEN AttendanceTypeId = 3 THEN 1 ELSE 0 END) AS WorkBased
				FROM Snapshot_CourseInstance SCI
				Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
				JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) AM,
			 (
				SELECT Sum(CASE WHEN AttendancePatternId = 6 THEN 1 ELSE 0 END) AS Customised,
					Sum(CASE WHEN AttendancePatternId = 2 THEN 1 ELSE 0 END) AS DayRelease,
					Sum(CASE WHEN AttendancePatternId = 1 THEN 1 ELSE 0 END) AS Daytime,
					Sum(CASE WHEN AttendancePatternId = 3 THEN 1 ELSE 0 END) AS Evening,
					Sum(CASE WHEN AttendancePatternId = 8 THEN 1 ELSE 0 END) AS NotApplicable,
					Sum(CASE WHEN AttendancePatternId = 7 THEN 1 ELSE 0 END) AS NotKnown,
					Sum(CASE WHEN AttendancePatternId = 4 THEN 1 ELSE 0 END) AS Twilight,
					Sum(CASE WHEN AttendancePatternId = 5 THEN 1 ELSE 0 END) AS Weekend
				FROM Snapshot_CourseInstance SCI
				Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
				JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) AP,
			 (
				SELECT Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent < 1 THEN 1 ELSE 0 END) AS OneWeekOrLess,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 1 AND 4 THEN 1 ELSE 0 END) AS OneToFourWeeks,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 4 AND 13 THEN 1 ELSE 0 END) AS OneToThreeMonths,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 13 AND 26 THEN 1 ELSE 0 END) AS ThreeToSixMonths,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 26 AND 52 THEN 1 ELSE 0 END) AS SixToTwelveMonths,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 52 AND 104 THEN 1 ELSE 0 END) AS OneToTwoYears,
					Sum(CASE WHEN DU.DurationUnitId IS NULL THEN 1 ELSE 0 END) AS NotKnown
				FROM Snapshot_CourseInstance SCI
					Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
					JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
					LEFT OUTER JOIN [Remote].DurationUnit DU ON DU.DurationUnitId = SCI.DurationUnitId
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) DU,
			 (
				SELECT Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 9 THEN 1 ELSE 0 END) AS Diploma,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 14 THEN 1 ELSE 0 END) AS AccessToHigherEducation,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 10 THEN 1 ELSE 0 END) AS Apprenticeship,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 4 THEN 1 ELSE 0 END) AS BasicSkill,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 2 THEN 1 ELSE 0 END) AS CertificateOfAttendance,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 5 THEN 1 ELSE 0 END) AS CourseProviderCertificate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 6 THEN 1 ELSE 0 END) AS ExternalAward,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 16 THEN 1 ELSE 0 END) AS FoundationDegree,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 3 THEN 1 ELSE 0 END) AS FunctionalSkill,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 13 THEN 1 ELSE 0 END) AS GCEOrEquivalent,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 8 THEN 1 ELSE 0 END) AS GCSEOrEquivalent,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 15 THEN 1 ELSE 0 END) AS InternationalBacculaureate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 12 THEN 1 ELSE 0 END) AS NoQualification,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 1 THEN 1 ELSE 0 END) AS HncHnd,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 11 THEN 1 ELSE 0 END) AS NVQ,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 7 THEN 1 ELSE 0 END) AS AccreditedQualification,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 18 THEN 1 ELSE 0 END) AS Postgraduate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 17 THEN 1 ELSE 0 END) AS UnderGraduate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 19 THEN 1 ELSE 0 END) AS ProfessionalQualification
				FROM Snapshot_Course SC
					JOIN Snapshot_Provider P on SC.ProviderId = P.ProviderId and P.Period = @PeriodToRun
					LEFT OUTER JOIN [Remote].LearningAim LA ON LA.LearningAimRefId = SC.LearningAimRefId
				WHERE SC.[Period] = @PeriodToRun
					AND SC.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) QT,
			 (
				SELECT Sum(CASE WHEN QualificationLevelId = 10 THEN 1 ELSE 0 END) AS EntryLevel,
					Sum(CASE WHEN QualificationLevelId = 9 THEN 1 ELSE 0 END) AS HigherLevel,
					Sum(CASE WHEN QualificationLevelId = 1 THEN 1 ELSE 0 END) AS Level1,
					Sum(CASE WHEN QualificationLevelId = 2 THEN 1 ELSE 0 END) AS Level2,
					Sum(CASE WHEN QualificationLevelId = 3 THEN 1 ELSE 0 END) AS Level3,
					Sum(CASE WHEN QualificationLevelId = 4 THEN 1 ELSE 0 END) AS Level4,
					Sum(CASE WHEN QualificationLevelId = 5 THEN 1 ELSE 0 END) AS Level5,
					Sum(CASE WHEN QualificationLevelId = 6 THEN 1 ELSE 0 END) AS Level6,
					Sum(CASE WHEN QualificationLevelId = 7 THEN 1 ELSE 0 END) AS Level7,
					Sum(CASE WHEN QualificationLevelId = 8 THEN 1 ELSE 0 END) AS Level8,
					Sum(CASE WHEN QualificationLevelId = 11 THEN 1 ELSE 0 END) AS NotKnown
				FROM Snapshot_Course SC
					JOIN Snapshot_Provider P on SC.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SC.[Period] = @PeriodToRun
					AND SC.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) QL;



		SELECT @PeriodToRun = Min(Period) FROM DWH_Period WHERE Period > @PeriodToRun AND PeriodType = 'M';
	END;


	INSERT INTO __RefactorLog (OperationKey) VALUES ('4D97114D-78F1-4b7f-9556-7431D09475D2');
END;
GO




--Fix Monthly Report Quality Data. Should only be pulling in data for SFA Providers
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EAA9F053-DAA4-4c4c-BC6D-8F922BE3C22A')
BEGIN
	PRINT '[Fixing Data]';
	SET NOCOUNT ON;

	DECLARE @PeriodToRun NVARCHAR(7)
	DECLARE @CurrentPeriod NVARCHAR(7);
	DECLARE @LiveStatusId INT = 2;

	SELECT @PeriodToRun = MIN(Period) FROM DWH_Period WHERE PeriodType = 'M';
	SELECT @CurrentPeriod = Period FROM DWH_Period_Latest WHERE PeriodType = 'M';

	WHILE @PeriodToRun <= @CurrentPeriod
	BEGIN

		DELETE FROM [MonthlyReport_Quality] WHERE Period = @PeriodToRun;

		INSERT [MonthlyReport_Quality]
		(
			[Period], 
			[Poor], 
			[Average], 
			[Good], 
			[VeryGood]
		)
		SELECT @PeriodToRun,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating BETWEEN 51 AND 71 THEN 1 ELSE 0 END), 0) AS Average,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating BETWEEN 71 AND 91 THEN 1 ELSE 0 END), 0) AS Good,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
		FROM Snapshot_QualityScore SQ
			INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
		WHERE SQ.[Period] = @PeriodToRun
			AND SP.IsTASOnly = 0
			AND SP.SFAFunded = 1;
	
		SELECT @PeriodToRun = Min(Period) FROM DWH_Period WHERE Period > @PeriodToRun AND PeriodType = 'M';
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('EAA9F053-DAA4-4c4c-BC6D-8F922BE3C22A');

END;
GO

--Fix DfE Start Date Report Provision Data. Should only be pulling in data for SFA Providers
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B99D101D-06D2-4AF8-9E91-8A9166B91070')
BEGIN
	PRINT '[Fixing DfE Start Date Provision Data]';
	SET NOCOUNT ON;

	DECLARE @PeriodToRun NVARCHAR(7)
	DECLARE @CurrentPeriod NVARCHAR(7);
	DECLARE @LiveStatusId INT = 2;

	SELECT @PeriodToRun = MIN(Period) FROM DWH_Period WHERE PeriodType = 'M';
	SELECT @CurrentPeriod = Period FROM DWH_Period_Latest WHERE PeriodType = 'M';

	WHILE @PeriodToRun <= @CurrentPeriod
	BEGIN

		UPDATE Snapshot_Course SET IsDfE = 0, IsSFA = 0 WHERE [Period] = @PeriodToRun;
		UPDATE Snapshot_CourseInstance SET IsDfE = 0, IsSFA = 0 WHERE [Period] = @PeriodToRun;

		UPDATE Snapshot_CourseInstance
		SET IsDfE = 1
		WHERE [Period] = @PeriodToRun
			AND CourseInstanceId IN (
										SELECT DISTINCT SCI.CourseInstanceId
										FROM Snapshot_CourseInstanceA10FundingCode SCIFC
											INNER JOIN Snapshot_CourseInstance SCI ON SCI.CourseInstanceId = SCIFC.CourseInstanceId AND SCI.[Period] = SCIFC.[Period]
											INNER JOIN Snapshot_Course SC ON SC.CourseId = SCI.CourseId AND SC.[Period] = SCi.[Period]
											INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.[Period] = SC.[Period]
										WHERE SCIFC.[Period] = @PeriodToRun
											AND (
													SCIFC.A10FundingCode = 25
													OR
													SP.SFAFunded = 0
												)
											AND SP.DFE1619Funded = 1
									);

		UPDATE Snapshot_CourseInstance
		SET IsSFA = 1
		WHERE [Period] = @PeriodToRun
			AND CourseInstanceId NOT IN (
											SELECT DISTINCT SCI.CourseInstanceId
											FROM Snapshot_CourseInstanceA10FundingCode SCIFC
												INNER JOIN Snapshot_CourseInstance SCI ON SCI.CourseInstanceId = SCIFC.CourseInstanceId AND SCI.[Period] = SCIFC.[Period]
												INNER JOIN Snapshot_Course SC ON SC.CourseId = SCI.CourseId AND SC.[Period] = SCi.[Period]
												INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.[Period] = SC.[Period]
											WHERE SCIFC.[Period] = @PeriodToRun
												AND SP.DFE1619Funded = 1
												AND (
														SCIFC.A10FundingCode = 25
														OR
														SP.SFAFunded = 0
													)
										);


		UPDATE Snapshot_Course
		SET IsDfE = 1
		WHERE [Period] = @PeriodToRun
			AND CourseId IN (
								SELECT DISTINCT CourseId
								FROM Snapshot_CourseInstance
								WHERE RecordStatusId = @LiveStatusId
									AND IsDfE = 1
									AND [Period] = @PeriodToRun
							);


		UPDATE Snapshot_Course
		SET IsSFA = 1
		WHERE [Period] = @PeriodToRun
			AND CourseId IN (
								SELECT DISTINCT CourseId
								FROM Snapshot_CourseInstance
								WHERE RecordStatusId = @LiveStatusId
									AND IsSFA = 1
									AND [Period] = @PeriodToRun
							);


		DELETE FROM [DFEReport_Provision] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [DFEReport_Provision]
		(
			[Period], 
			[NumberOfCourses],
			[NumberOfLiveCourses],
			[NumberOfOpportunities],
			[NumberOfLiveOpportunities],
			[ProvidersWithNoCourses]
			
		)
		SELECT @PeriodToRun,
			COALESCE(C.NumberOfCourses, 0) AS NumberOfCourses,
			COALESCE(C.NumberOfLiveCourses, 0) AS NumberOfLiveCourses,
			COALESCE(C.NumberOfOpportunities, 0) AS NumberOfOpportunities,
			COALESCE(C.NumberOfLiveOpportunities, 0) AS NumberOfLiveOpportunities,
			COALESCE(P.NumberOfProvidersWithNoCourses, 0) AS NumberOfProvidersWithNoCourses			
		FROM (
				SELECT Count(*) AS NumberOfCourses,
					Sum(CASE WHEN SC.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveCourses,
					Sum(COALESCE(NumOpportunities, 0)) AS NumberOfOpportunities,
					Sum(COALESCE(NumLiveOpportunities, 0)) AS NumberOfLiveOpportunities
				FROM Snapshot_Course SC
					LEFT JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.Period = SC.Period
					LEFT OUTER JOIN (
										SELECT SCI.CourseId, 
											CASE WHEN Sum(CASE WHEN A10.CourseInstanceId IS NOT NULL THEN 1 ELSE 0 END) = 0 THEN 0 ELSE 1 END AS IsDFE, 
											Count(*) AS NumOpportunities, 
											Sum(CASE WHEN SCI.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumLiveOpportunities 
										FROM Snapshot_CourseInstance SCI 
											LEFT OUTER JOIN (
																SELECT CourseInstanceId 
																FROM Snapshot_CourseInstanceA10FundingCode 
																WHERE Period = @PeriodToRun
																	AND A10FundingCode = 25 															
																GROUP BY CourseInstanceId
															) A10 ON A10.CourseInstanceId = SCI.CourseInstanceId
										WHERE SCI.[Period] = @PeriodToRun 
										GROUP BY SCI.CourseId
									) SCI ON SC.CourseId = SCI.CourseId
				WHERE SC.[Period] = @PeriodToRun
					AND SP.RecordStatusId = @LiveStatusId
					AND SP.DFE1619Funded = 1
					AND (SP.SFAFunded = 0 OR SCI.IsDFE = 1)
			) C,
			(
				SELECT Count(*) AS NumberOfProvidersWithNoCourses
				FROM Snapshot_Provider SP
					LEFT OUTER JOIN Snapshot_Course SC ON SC.ProviderId = SP.ProviderId AND SC.[Period] = SP.[Period]
				WHERE SP.[Period] = @PeriodToRun
					AND SC.ProviderId IS NULL
					AND SP.DFE1619Funded = 1
					AND SP.RecordStatusId = @LiveStatusId
			) P;
	
		SELECT @PeriodToRun = Min(Period) FROM DWH_Period WHERE Period > @PeriodToRun AND PeriodType = 'M';
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B99D101D-06D2-4AF8-9E91-8A9166B91070');

END;
GO

--Fix Monthly Report Quality Data. There was some duplication of values where score was exactly 71 or 91.
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FF7C5C3F-1321-4935-9B4C-799BD98EE209')
BEGIN
	PRINT '[Fixing DfE Start Date and Monthly Report Quality Data]';
	SET NOCOUNT ON;

	DECLARE @PeriodToRun NVARCHAR(7)
	DECLARE @CurrentPeriod NVARCHAR(7);
	DECLARE @LiveStatusId INT = 2;

	SELECT @PeriodToRun = MIN(Period) FROM DWH_Period WHERE PeriodType = 'M';
	SELECT @CurrentPeriod = Period FROM DWH_Period_Latest WHERE PeriodType = 'M';

	WHILE @PeriodToRun <= @CurrentPeriod
	BEGIN

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [DFEReport_Quality] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [DFEReport_Quality]
		(
			[Period], 
			[Poor], 
			[Average], 
			[Good], 
			[VeryGood]
		)
		SELECT @PeriodToRun,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 51 AND AutoAggregateQualityRating < 71 THEN 1 ELSE 0 END), 0) AS Average,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 71 AND AutoAggregateQualityRating < 91 THEN 1 ELSE 0 END), 0) AS Good,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
		FROM Snapshot_QualityScore SQ
			INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
		WHERE SQ.[Period] = @PeriodToRun
			AND SP.DFE1619Funded = 1
			AND SP.RecordStatusId = @LiveStatusId;
	

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [MonthlyReport_Quality] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [MonthlyReport_Quality]
		(
			[Period], 
			[Poor], 
			[Average], 
			[Good], 
			[VeryGood]
		)
		SELECT @PeriodToRun,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 51 AND AutoAggregateQualityRating < 71 THEN 1 ELSE 0 END), 0) AS Average,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 71 AND AutoAggregateQualityRating < 91 THEN 1 ELSE 0 END), 0) AS Good,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
		FROM Snapshot_QualityScore SQ
			INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
		WHERE SQ.[Period] = @PeriodToRun
			AND SP.IsTASOnly = 0
			AND SP.SFAFunded = 1
			AND SP.RecordStatusId = @LiveStatusId;

		SELECT @PeriodToRun = Min(Period) FROM DWH_Period WHERE Period > @PeriodToRun AND PeriodType = 'M';
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FF7C5C3F-1321-4935-9B4C-799BD98EE209');

END;
GO
