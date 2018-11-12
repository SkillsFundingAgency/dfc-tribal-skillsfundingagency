CREATE PROCEDURE [Search].[CourseBrowseList] (@IncludeUCASData BIT = 0)
AS

BEGIN

	DECLARE @LiveStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);
	DECLARE @OneYearAgo DATE = CAST(DATEADD(YEAR, -1, GETUTCDATE()) AS DATE);

	SELECT CategoryCodeId, 
		ParentCategoryCodeId, 
		LCSBC.[Description], 
		LCSBC.IsSearchable, 
		COALESCE(CLDC.TotalCourses, 0) AS Total,
		CASE WHEN @IncludeUCASData = 1 THEN COALESCE(UCAS.TotalCourses, 0) ELSE 0 END AS TotalUCAS
	FROM LegacyCourseSubjectBrowseCategories LCSBC
		LEFT OUTER JOIN (
							SELECT LearnDirectClassificationRef,
								Sum(TotalCourses) AS TotalCourses
							FROM (
									SELECT CLDC.LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.CourseLearnDirectClassification CLDC
										INNER JOIN dbo.Course C ON CLDC.CourseId = C.CourseId AND C.RecordStatusId = @LiveStatusId  
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM dbo.CourseInstance CI INNER JOIN dbo.CourseInstanceStartDate CISD ON CISD.CourseInstanceId = CI.CourseInstanceId WHERE CI.RecordStatusId = @LiveStatusId GROUP BY CourseId) CISD ON CISD.CourseId = C.CourseId
										INNER JOIN dbo.Provider P ON C.ProviderId = P.ProviderId AND P.RecordStatusId = @LiveStatusId AND P.PublishData = 1
									WHERE CISD.StartDate IS NULL OR CISD.StartDate >= @OneYearAgo
									GROUP BY CLDC.LearnDirectClassificationRef	

									UNION ALL

									SELECT LA.LearnDirectClassSystemCode1 AS LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.LearningAim LA
										INNER JOIN dbo.Course C ON C.LearningAimRefId = LA.LearningAimRefId AND C.RecordStatusId = @LiveStatusId
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM dbo.CourseInstance CI INNER JOIN dbo.CourseInstanceStartDate CISD ON CISD.CourseInstanceId = CI.CourseInstanceId WHERE CI.RecordStatusId = @LiveStatusId GROUP BY CourseId) CISD ON CISD.CourseId = C.CourseId
										INNER JOIN dbo.Provider P ON C.ProviderId = P.ProviderId AND P.RecordStatusId = @LiveStatusId AND P.PublishData = 1
									WHERE CISD.StartDate IS NULL OR CISD.StartDate >= @OneYearAgo
									GROUP BY LA.LearnDirectClassSystemCode1	

									UNION ALL

									SELECT LA.LearnDirectClassSystemCode2 AS LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.LearningAim LA
										INNER JOIN dbo.Course C ON C.LearningAimRefId = LA.LearningAimRefId AND C.RecordStatusId = @LiveStatusId
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM dbo.CourseInstance CI INNER JOIN dbo.CourseInstanceStartDate CISD ON CISD.CourseInstanceId = CI.CourseInstanceId WHERE CI.RecordStatusId = @LiveStatusId GROUP BY CourseId) CISD ON CISD.CourseId = C.CourseId
										INNER JOIN dbo.Provider P ON C.ProviderId = P.ProviderId AND P.RecordStatusId = @LiveStatusId AND P.PublishData = 1
									WHERE CISD.StartDate IS NULL OR CISD.StartDate >= @OneYearAgo
									GROUP BY LA.LearnDirectClassSystemCode2	

									UNION ALL

									SELECT LA.LearnDirectClassSystemCode3 AS LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.LearningAim LA
										INNER JOIN dbo.Course C ON C.LearningAimRefId = LA.LearningAimRefId AND C.RecordStatusId = @LiveStatusId
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM dbo.CourseInstance CI INNER JOIN dbo.CourseInstanceStartDate CISD ON CISD.CourseInstanceId = CI.CourseInstanceId WHERE CI.RecordStatusId = @LiveStatusId GROUP BY CourseId) CISD ON CISD.CourseId = C.CourseId
										INNER JOIN dbo.Provider P ON C.ProviderId = P.ProviderId AND P.RecordStatusId = @LiveStatusId AND P.PublishData = 1
									WHERE CISD.StartDate IS NULL OR CISD.StartDate >= @OneYearAgo
									GROUP BY LA.LearnDirectClassSystemCode3	
								) LD
							GROUP BY LearnDirectClassificationRef
						) CLDC ON CLDC.LearnDirectClassificationRef = LCSBC.CategoryCodeId
		LEFT OUTER JOIN (
							SELECT LearnDirectClassificationRef,
								Sum(TotalCourses) AS TotalCourses
							FROM (
									SELECT LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.[LearnDirectClassificationToJACS3Mapping] Map
										INNER JOIN [UCAS].[Courses] C ON C.HESA1 = Map.JACS3
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM [UCAS].Starts GROUP BY CourseId) SD ON SD.CourseId = C.CourseId
									WHERE SD.StartDate IS NULL OR SD.StartDate >= @OneYearAgo
									GROUP BY LearnDirectClassificationRef

									UNION

									SELECT LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.[LearnDirectClassificationToJACS3Mapping] Map
										INNER JOIN [UCAS].[Courses] C ON C.HESA2 = Map.JACS3
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM [UCAS].Starts GROUP BY CourseId) SD ON SD.CourseId = C.CourseId
									WHERE SD.StartDate IS NULL OR SD.StartDate >= @OneYearAgo
									GROUP BY LearnDirectClassificationRef

									UNION

									SELECT LearnDirectClassificationRef,
										Count(*) As TotalCourses
									FROM dbo.[LearnDirectClassificationToJACS3Mapping] Map
										INNER JOIN [UCAS].[Courses] C ON C.HESA3 = Map.JACS3
										LEFT OUTER JOIN (SELECT CourseId, Max(StartDate) AS StartDate FROM [UCAS].Starts GROUP BY CourseId) SD ON SD.CourseId = C.CourseId
									WHERE SD.StartDate IS NULL OR SD.StartDate >= @OneYearAgo
									GROUP BY LearnDirectClassificationRef
								) A 
							GROUP BY LearnDirectClassificationRef
						) UCAS ON UCAS.LearnDirectClassificationRef = LCSBC.CategoryCodeId;

	IF(@@ERROR <> 0) 
		RETURN 1 ;
	ELSE 
		RETURN 0;

END;