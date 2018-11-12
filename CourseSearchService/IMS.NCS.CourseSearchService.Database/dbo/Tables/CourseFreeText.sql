CREATE TABLE [dbo].[CourseFreeText]
(
    [CourseId]                   INT             NOT NULL,
    [CourseTitle]                NVARCHAR (255)  NOT NULL,
    [CourseSummary]              NVARCHAR (2000) NULL,
    [QualificationTitle]         NVARCHAR (255)  NULL,
    CONSTRAINT [PK__CourseFreeText__C92D71A6ED743580] PRIMARY KEY NONCLUSTERED ([CourseId] ASC)
);

GO

CREATE FULLTEXT INDEX ON [dbo].[CourseFreeText]
    ([CourseTitle] LANGUAGE 1033
	, [QualificationTitle] LANGUAGE 1033
	, [CourseSummary] LANGUAGE 1033)
    KEY INDEX [PK__CourseFreeText__C92D71A6ED743580]
    ON [Course_CourseTitle];

GO
