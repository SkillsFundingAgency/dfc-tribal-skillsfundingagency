CREATE FULLTEXT INDEX ON [search].[Provider]
    ([ProviderName] LANGUAGE 1033, [ProviderNameAlias] LANGUAGE 1033)
    KEY INDEX [PK__Provider__B54C687C612324A7]
    ON [Course_CourseTitle];
GO

CREATE FULLTEXT INDEX ON [search].[Course]
    ([CourseTitle] LANGUAGE 1033
	, [QualificationTitle] LANGUAGE 1033
	, [CourseSummary] LANGUAGE 1033)
    KEY INDEX [PK__Course__C92D71A6ED743580]
    ON [Course_CourseTitle];
GO

CREATE FULLTEXT INDEX ON [search].[VenueText]
    ([SearchText] LANGUAGE 1033)
    KEY INDEX [PK_VenueText]
    ON [Course_CourseTitle];
GO