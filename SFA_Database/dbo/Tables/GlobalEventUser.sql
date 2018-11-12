CREATE TABLE [dbo].[GlobalEventUser] (
    [UserID] [dbo].[GEL_ID_t]   IDENTITY (1, 1) NOT NULL,
    [User]   [dbo].[GEL_User_t] NOT NULL,
    CONSTRAINT [PK_GlobalEventUser] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (FILLFACTOR = 90)
);