CREATE TABLE [dbo].[UnableToCompleteFailureReason]
(
	[UnableToCompleteFailureReasonId] INT NOT NULL PRIMARY KEY, 
    [Description] NVARCHAR(400) NOT NULL, 
	[FullDescription] NVARCHAR(1000) NULL,
    [Ordinal] INT NOT NULL, 
    [RecordStatusId] INT NOT NULL, 
    CONSTRAINT [FK_UnableToCompleteFailureReason_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
)
