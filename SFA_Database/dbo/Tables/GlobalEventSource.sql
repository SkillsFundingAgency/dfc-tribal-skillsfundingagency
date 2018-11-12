CREATE TABLE [dbo].[GlobalEventSource] (
    [SourceID] [dbo].[GEL_ID_t]     IDENTITY (1, 1) NOT NULL,
    [Source]   [dbo].[GEL_Source_t] NOT NULL,
    CONSTRAINT [PK_GlobalEventSource] PRIMARY KEY CLUSTERED ([SourceID] ASC) WITH (FILLFACTOR = 90)
);