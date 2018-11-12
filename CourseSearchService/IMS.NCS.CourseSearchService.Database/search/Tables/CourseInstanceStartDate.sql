CREATE TABLE [Search].[CourseInstanceStartDate] (
    [CourseInstanceStartDateId] INT           IDENTITY (1, 1) NOT NULL,
    [CourseInstanceId]          INT           NOT NULL,
    [StartDate]                 DATETIME      NOT NULL,
    [PlacesAvailable]           INT           NULL,
    [DateFormat]                NVARCHAR (16) NULL, 
    CONSTRAINT [PK_CourseInstanceStartDate] PRIMARY KEY ([CourseInstanceStartDateId]),
);



