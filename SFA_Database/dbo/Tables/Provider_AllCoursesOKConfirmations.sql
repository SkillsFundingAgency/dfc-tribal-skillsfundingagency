CREATE TABLE [dbo].[Provider_AllCoursesOKConfirmations]
(
	[ProviderId] INT NOT NULL, 
    [DateTimeUtc] DATETIME NOT NULL, 
    [CreatedByUserId]           NVARCHAR(128)	NOT NULL, 
    PRIMARY KEY ([ProviderId], [DateTimeUtc]), 
    CONSTRAINT [FK_Provider_AllCoursesOKConfirmations_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([ProviderId]), 
    CONSTRAINT [FK_Provider_AllCoursesOKConfirmations_AspNetUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id])
)
