CREATE TABLE [dbo].[Import_LearningAimValidity]
(
	[LearningAimRefId] [varchar](10) NOT NULL,
	[ValidityCategory] [nvarchar](50) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[LastNewStartDate] [date] NULL, 
    CONSTRAINT [PK_Import_LearningAimValidity] PRIMARY KEY ([LearningAimRefId], [ValidityCategory])
)
