CREATE TABLE [dbo].[Prison]
(
	[PrisonId] INT NOT NULL IDENTITY , 
    [PrisonName] NVARCHAR(100) NOT NULL, 
    [PrisonPostcode] NVARCHAR(30) NULL, 
    CONSTRAINT [PK_Prison] PRIMARY KEY ([PrisonId])
)

GO


