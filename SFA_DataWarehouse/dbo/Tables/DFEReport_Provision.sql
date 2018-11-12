CREATE TABLE [dbo].[DFEReport_Provision]
(
	[Period]								VARCHAR(7)	NOT NULL PRIMARY KEY, 
    [NumberOfCourses]						INT			NOT NULL, 
    [NumberOfLiveCourses]					INT			NOT NULL, 
    [NumberOfOpportunities]					INT			NOT NULL, 
    [NumberOfLiveOpportunities]				INT			NOT NULL, 
    [ProvidersWithNoCourses]				INT			NOT NULL 
);
