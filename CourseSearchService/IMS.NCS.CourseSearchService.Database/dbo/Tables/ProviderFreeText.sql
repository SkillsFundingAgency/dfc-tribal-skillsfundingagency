CREATE TABLE [dbo].[ProviderFreeText] (
    [ProviderTextId] INT            NOT NULL,
    [SearchText]     NVARCHAR (714) NOT NULL,
    CONSTRAINT [PK__ProviderFreeText__1A72B934B1B0996A] PRIMARY KEY NONCLUSTERED ([ProviderTextId] ASC)
);


GO

CREATE FULLTEXT INDEX ON [dbo].[ProviderFreeText] ([SearchText]) KEY INDEX [PK__ProviderFreeText__1A72B934B1B0996A] ON [Course_CourseTitle];


