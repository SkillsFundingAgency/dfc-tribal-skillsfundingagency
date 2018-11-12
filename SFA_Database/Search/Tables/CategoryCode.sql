CREATE TABLE [Search].[CategoryCode] (
    [CategoryCodeId]     INT            IDENTITY (1, 1) NOT NULL,
    [CategoryCode]       NVARCHAR (10)  NOT NULL,
    [ParentCategoryCode] NVARCHAR (10)  NULL,
    [Description]        NVARCHAR (200) NOT NULL,
    [IsSearchable]       BIT            NOT NULL,
    [TotalCourses]       INT            NOT NULL DEFAULT 0,
    [TotalUCASCourses]   INT            NOT NULL DEFAULT 0,
    CONSTRAINT [PK__Category__A3303EB5942113BC] PRIMARY KEY NONCLUSTERED ([CategoryCodeId] ASC)
)
GO

CREATE INDEX [IX_CategoryCode_ParentCategoryCode] ON [Search].[CategoryCode] ([ParentCategoryCode])
GO

CREATE INDEX [IX_CategoryCode_CategoryCode] ON [Search].[CategoryCode] ([CategoryCode])
GO
