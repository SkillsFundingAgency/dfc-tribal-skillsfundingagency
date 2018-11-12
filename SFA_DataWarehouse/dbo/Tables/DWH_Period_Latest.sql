CREATE TABLE [dbo].[DWH_Period_Latest]
(
	[Period]				VARCHAR(7)		NOT NULL PRIMARY KEY, 
	[PeriodType]			NVARCHAR(1)		NOT NULL,
    [PeriodName]			NVARCHAR(25)	NOT NULL, 
    [PeriodStartDate]		DATE			NOT NULL, 
    [PreviousPeriod]		VARCHAR(7)		NOT NULL, 
    [NextPeriod]			VARCHAR(7)		NOT NULL, 
    [NextPeriodStartDate]	DATE			NOT NULL 
)
