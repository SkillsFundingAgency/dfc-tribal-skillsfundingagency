CREATE TABLE [dbo].[QAStyleFailureReason]
(
	[QAStyleFailureReasonId] INT NOT NULL PRIMARY KEY, 
    [Description] NVARCHAR(100) NOT NULL, 
	[FullDescription] NVARCHAR(1000) NULL,
    [Ordinal] INT NOT NULL, 
    [RecordStatusId] INT NOT NULL, 
    CONSTRAINT [FK_QAStyleFailureReason_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
)
