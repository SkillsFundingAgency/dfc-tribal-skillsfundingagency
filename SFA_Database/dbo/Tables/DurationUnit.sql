CREATE TABLE [dbo].[DurationUnit] (
    [DurationUnitId]   INT          NOT NULL,
    [DurationUnitName] VARCHAR (50) NOT NULL,
    [BulkUploadRef] NVARCHAR(10) NOT NULL, 
    [DisplayOrder] INT NOT NULL, 
    [WeekEquivalent] FLOAT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_DurationUnit] PRIMARY KEY CLUSTERED ([DurationUnitId] ASC)
);

