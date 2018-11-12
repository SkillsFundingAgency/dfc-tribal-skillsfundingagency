CREATE TABLE [dbo].[ProviderUnableToCompleteFailureReason]
(
	[ProviderUnableToCompleteId] INT NOT NULL , 
    [UnableToCompleteFailureReasonId] INT NOT NULL, 
    PRIMARY KEY ([ProviderUnableToCompleteId], [UnableToCompleteFailureReasonId]), 
    CONSTRAINT [FK_ProviderUnableToCompleteFailureReason_ProviderUnableToComplete] FOREIGN KEY ([ProviderUnableToCompleteId]) REFERENCES [ProviderUnableToComplete]([ProviderUnableToCompleteId]), 
    CONSTRAINT [FK_ProviderUnableToCompleteFailureReason_UnableToCompleteFailureReason] FOREIGN KEY ([UnableToCompleteFailureReasonId]) REFERENCES [UnableToCompleteFailureReason]([UnableToCompleteFailureReasonId])
)
