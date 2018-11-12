CREATE TABLE [dbo].[Import_LearningAim]
(
	[LearningAimRefId] [varchar](10) NOT NULL,
	[LearningAimTitle] [nvarchar](255) NOT NULL,
	[LearningAimAwardOrgCode] [nvarchar](20) NOT NULL,
	[IndependentLivingSkills] [bit] NOT NULL DEFAULT ((0)),
	[LDCS1] [nvarchar](12) NULL,
	[LDCS2] [nvarchar](12) NULL,
	[LDCS3] [nvarchar](12) NULL, 
    [QualificationLevelId] NVARCHAR(12) NULL, 
    CONSTRAINT [PK_Import_LearningAim] PRIMARY KEY ([LearningAimRefId])
)
