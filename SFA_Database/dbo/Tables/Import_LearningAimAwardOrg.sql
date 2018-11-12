CREATE TABLE [dbo].[Import_LearningAimAwardOrg]
(
	[LearningAimAwardOrgCode] [nvarchar](20) NOT NULL,
	[AwardOrgName] [nvarchar](150) NULL, 
    CONSTRAINT [PK_Import_LearningAimAwardOrg] PRIMARY KEY ([LearningAimAwardOrgCode])
)
