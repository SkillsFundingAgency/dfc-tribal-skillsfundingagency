CREATE TABLE [dbo].[GlobalEventType] (
    [TypeID] [dbo].[GEL_ID_t]   IDENTITY (1, 1) NOT NULL,
    [Type]   [dbo].[GEL_Type_t] NOT NULL,
    CONSTRAINT [PK_GlobalEventType] PRIMARY KEY CLUSTERED ([TypeID] ASC) WITH (FILLFACTOR = 90)
);