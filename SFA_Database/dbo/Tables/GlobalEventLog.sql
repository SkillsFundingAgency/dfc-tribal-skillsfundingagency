CREATE TABLE [dbo].[GlobalEventLog] (
    [EventID]     [dbo].[GEL_ID_t]       IDENTITY (1, 1) NOT NULL,
    [DateTime]    [dbo].[GEL_DateTime_t] NULL,
    [TypeID]      [dbo].[GEL_ID_t]       NULL,
    [SourceID]    [dbo].[GEL_ID_t]       NULL,
    [ComputerID]  [dbo].[GEL_ID_t]       NULL,
    [UserID]      [dbo].[GEL_ID_t]       NULL,
    [Event]       [dbo].[GEL_Event_t]    NULL,
    [DateTimeUtc] [dbo].[GEL_DateTime_t] NULL,
    CONSTRAINT [PK_GlobalEventLog] PRIMARY KEY CLUSTERED ([EventID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_GlobalEventLog_ComputerID] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[GlobalEventComputer] ([ComputerID]),
    CONSTRAINT [FK_GlobalEventLog_SourceID] FOREIGN KEY ([SourceID]) REFERENCES [dbo].[GlobalEventSource] ([SourceID]),
    CONSTRAINT [FK_GlobalEventLog_TypeID] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[GlobalEventType] ([TypeID]),
    CONSTRAINT [FK_GlobalEventLog_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[GlobalEventUser] ([UserID])
)
GO

CREATE INDEX [IX_GlobalEventLog_UserID] ON [dbo].[GlobalEventLog] ([UserID]) INCLUDE ([EventID])
GO

CREATE INDEX [IX_GlobalEventLog_TypeID] ON [dbo].[GlobalEventLog] ([TypeID]) INCLUDE ([EventID])
GO

CREATE INDEX [IX_GlobalEventLog_ComputerID] ON [dbo].[GlobalEventLog] ([ComputerID]) INCLUDE ([EventID])
GO

CREATE INDEX [IX_GlobalEventLog_SourceID] ON [dbo].[GlobalEventLog] ([SourceID]) INCLUDE ([EventID])
GO
