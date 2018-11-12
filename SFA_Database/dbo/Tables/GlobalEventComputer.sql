CREATE TABLE [dbo].[GlobalEventComputer] (
    [ComputerID] [dbo].[GEL_ID_t]       IDENTITY (1, 1) NOT NULL,
    [Computer]   [dbo].[GEL_Computer_t] NOT NULL,
    CONSTRAINT [PK_GlobalEventComputer] PRIMARY KEY CLUSTERED ([ComputerID] ASC) WITH (FILLFACTOR = 90)
);