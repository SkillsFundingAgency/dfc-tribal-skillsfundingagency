	CREATE TABLE [dbo].[DeliveryMode] (
    [DeliveryModeId]	INT				NOT NULL,
    [DeliveryModeName]	NVARCHAR (100)	NOT NULL,
	[DeliveryModeDescription] NVARCHAR(100) NOT NULL DEFAULT '',
	[BulkUploadRef]		NVARCHAR(10)	NOT NULL,
	[DASRef]			NVARCHAR(100)	NOT NULL DEFAULT '',
    [RecordStatusId] INT NOT NULL, 
    [MustHaveFullLocation] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_DeliveryMode] PRIMARY KEY CLUSTERED ([DeliveryModeId] ASC), 
    CONSTRAINT [FK_DeliveryMode_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
);