CREATE PROCEDURE [UCAS].[up_UCAS_HandleDeletions]
AS
BEGIN

	DELETE FROM [UCAS].[CourseEntry] WHERE CourseEntryId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'CourseEntry');
	DELETE FROM [UCAS].[Courses] WHERE CourseId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'Courses');
	DELETE FROM [UCAS].[CoursesIndex] WHERE CourseIndexId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'CoursesIndex');
	DELETE FROM [UCAS].[Currencies] WHERE CurrencyId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'Currencies');
	DELETE FROM [UCAS].[Fees] WHERE FeeId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'Fees');
	DELETE FROM [UCAS].[FeeYears] WHERE FeeYearId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'FeeYear');
	DELETE FROM [UCAS].[Orgs] WHERE OrgId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'Orgs');
	DELETE FROM [UCAS].[PlacesOfStudy] WHERE PlaceOfStudyId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'PlacesOfStudy');
	DELETE FROM [UCAS].[Starts] WHERE StartId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'Starts');
	DELETE FROM [UCAS].[StartsIndex] WHERE StartIndexId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'StartsIndex');
	DELETE FROM [UCAS].[Towns] WHERE TownId IN (SELECT RecordId FROM [UCAS].[Deletions] WHERE TableName = 'Towns');

END;