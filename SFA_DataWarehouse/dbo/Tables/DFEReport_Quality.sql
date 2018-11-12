CREATE TABLE [dbo].[DFEReport_Quality]
(
	[Period] VARCHAR(7) NOT NULL PRIMARY KEY, 
    [Poor] INT NOT NULL, 
    [Average] INT NOT NULL, 
    [Good] INT NOT NULL, 
    [VeryGood] INT NOT NULL
);
