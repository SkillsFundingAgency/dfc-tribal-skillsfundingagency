CREATE TABLE [Search].[CategoryCode] (
    [CategoryCodeId]     INT            IDENTITY (1, 1) NOT NULL,
    [CategoryCode]       NVARCHAR (10)  NOT NULL,
    [ParentCategoryCode] NVARCHAR (10)  NULL,
    [Description]        NVARCHAR (200) NOT NULL,
    [IsSearchable]       BIT            NOT NULL,
    [TotalCourses]       INT            NOT NULL DEFAULT 0,
    [TotalUCASCourses]   INT            NOT NULL DEFAULT 0,
    [Level] INT NULL, 
    [SortOrder] VARCHAR(4000) NULL, 
    CONSTRAINT [PK__Category__A3303EB5942113BC] PRIMARY KEY NONCLUSTERED ([CategoryCodeId] ASC)
);



