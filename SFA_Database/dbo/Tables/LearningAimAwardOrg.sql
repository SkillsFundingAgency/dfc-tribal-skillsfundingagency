CREATE TABLE [dbo].[LearningAimAwardOrg] (
    [LearningAimAwardOrgCode] NVARCHAR (20)  NOT NULL,
    [AwardOrgName]            NVARCHAR (150) NULL,
    CONSTRAINT [PK_LearningAimAwardOrg] PRIMARY KEY CLUSTERED ([LearningAimAwardOrgCode] ASC)
);

