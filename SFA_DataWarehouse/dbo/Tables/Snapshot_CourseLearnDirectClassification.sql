CREATE TABLE [dbo].[Snapshot_CourseLearnDirectClassification]
(
	[Period]						VARCHAR(7) NOT NULL,
	[CourseId]						INT NOT NULL , 
    [LearnDirectClassificationRef]	NVARCHAR(12) NOT NULL, 
    [ClassificationOrder]			INT NOT NULL, 
    PRIMARY KEY ([Period], [CourseId], [LearnDirectClassificationRef])
);

