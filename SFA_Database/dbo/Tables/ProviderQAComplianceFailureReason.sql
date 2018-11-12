CREATE TABLE [dbo].[ProviderQAComplianceFailureReason]
(
	[ProviderQAComplianceId] INT NOT NULL , 
    [QAComplianceFailureReasonId] INT NOT NULL, 
    PRIMARY KEY ([ProviderQAComplianceId], [QAComplianceFailureReasonId]), 
    CONSTRAINT [FK_ProviderQAComplianceFailureReason_ProviderQACompliance] FOREIGN KEY ([ProviderQAComplianceId]) REFERENCES [ProviderQACompliance]([ProviderQAComplianceId]), 
    CONSTRAINT [FK_ProviderQAComplianceFailureReason_QAComplianceFailureReason] FOREIGN KEY ([QAComplianceFailureReasonId]) REFERENCES [QAComplianceFailureReason]([QAComplianceFailureReasonId])
)
