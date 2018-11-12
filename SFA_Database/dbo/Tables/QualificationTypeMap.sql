CREATE TABLE [dbo].[QualificationTypeMap] (
    [LearningAimRefId]      NVARCHAR (10)  NOT NULL,
    [QualificationTypeCode] NVARCHAR (10)  NOT NULL,
    [Description]           NVARCHAR (500) NOT NULL,
    [Status]                NVARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_QualificationTypeMap] PRIMARY KEY CLUSTERED ([LearningAimRefId] ASC)
);

