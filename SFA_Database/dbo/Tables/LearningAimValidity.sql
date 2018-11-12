CREATE TABLE [dbo].[LearningAimValidity]
(
	[LearningAimRefId] VARCHAR(10) NOT NULL , 
    [ValidityCategory] NVARCHAR(50) NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [EndDate] DATE NULL, 
    [LastNewStartDate] DATE NULL, 
    PRIMARY KEY ([LearningAimRefId], [ValidityCategory])
)
