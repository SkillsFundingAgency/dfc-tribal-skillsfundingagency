CREATE TABLE [dbo].[CourseLanguage]
(
	[LanguageId] INT NOT NULL PRIMARY KEY, 
    [Language] NVARCHAR(100) NOT NULL, 
    [LanguageCode] NVARCHAR(5) NOT NULL, 
    [DisplayOrder] INT NOT NULL
)
