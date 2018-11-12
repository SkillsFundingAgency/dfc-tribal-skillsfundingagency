-- LC: This table holds subject browsing data which appears to be be-spoke for the course directory search engine.  The codes
-- appear fairly static and are included in the nightly CSV outputs along with a count of courses falling into each category
-- Once the legacy CSV outputs are decommissioned this table may no longer be relevant.
CREATE TABLE [dbo].[LegacyCourseSubjectBrowseCategories]
(
	[CategoryCodeId] NVARCHAR(10) NOT NULL , 
    [ParentCategoryCodeId] NVARCHAR(10) NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [IsSearchable] BIT NOT NULL
)
GO

CREATE INDEX [IX_LegacyCourseSubjectBrowseCategories_CategoryCode] ON [dbo].[LegacyCourseSubjectBrowseCategories] ([CategoryCodeId])
GO

CREATE INDEX [IX_LegacyCourseSubjectBrowseCategories_ParentCategoryCode] ON [dbo].[LegacyCourseSubjectBrowseCategories] ([ParentCategoryCodeId])
GO