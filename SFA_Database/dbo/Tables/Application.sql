CREATE TABLE [dbo].[Application] (
    [ApplicationId]   INT          NOT NULL,
    [ApplicationName] VARCHAR (50) NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([ApplicationId] ASC)
);

