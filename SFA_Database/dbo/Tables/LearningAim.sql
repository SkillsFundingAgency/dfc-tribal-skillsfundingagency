CREATE TABLE [dbo].[LearningAim] (
    [LearningAimRefId]        VARCHAR (10)   NOT NULL,
    [Qualification]           VARCHAR (150)  NOT NULL,
    [LearningAimTitle]        NVARCHAR (255) NOT NULL,
    [LearningAimAwardOrgCode] NVARCHAR (20)  NOT NULL,
	[ErAppStatus]             NVARCHAR (50)  NULL,
    [SkillsForLife]           NVARCHAR (5)   NULL,
    [QualificationTypeId] INT NULL, 
    [IndependentLivingSkills] BIT NOT NULL DEFAULT 0, 
    [LearnDirectClassSystemCode1] NVARCHAR(12) NULL, 
    [LearnDirectClassSystemCode2] NVARCHAR(12) NULL, 
    [LearnDirectClassSystemCode3] NVARCHAR(12) NULL, 
    [RecordStatusId] INT NOT NULL DEFAULT 2, 
    [QualificationLevelId] INT NULL, 
    CONSTRAINT [PK_LearningAimReference] PRIMARY KEY CLUSTERED ([LearningAimRefId] ASC),
    CONSTRAINT [FK_LearningAim_LearningAimAwardOrg] FOREIGN KEY ([LearningAimAwardOrgCode]) REFERENCES [dbo].[LearningAimAwardOrg] ([LearningAimAwardOrgCode]),
	CONSTRAINT [FK_LearningAim_QualificationTypeId] FOREIGN KEY ([QualificationTypeId]) REFERENCES [dbo].[QualificationType] ([QualificationTypeId]), 
    CONSTRAINT [FK_LearningAim_LearnDirect1] FOREIGN KEY ([LearnDirectClassSystemCode1]) REFERENCES [dbo].[LearnDirectClassification]([LearnDirectClassificationRef]),
    CONSTRAINT [FK_LearningAim_LearnDirect2] FOREIGN KEY ([LearnDirectClassSystemCode2]) REFERENCES [dbo].[LearnDirectClassification]([LearnDirectClassificationRef]),
    CONSTRAINT [FK_LearningAim_LearnDirect3] FOREIGN KEY ([LearnDirectClassSystemCode3]) REFERENCES [dbo].[LearnDirectClassification]([LearnDirectClassificationRef]), 
    CONSTRAINT [FK_LearningAim_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
);

