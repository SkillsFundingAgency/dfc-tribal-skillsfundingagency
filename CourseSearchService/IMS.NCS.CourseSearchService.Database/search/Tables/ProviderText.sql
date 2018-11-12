CREATE TABLE [Search].[ProviderText] (
    [ProviderTextId] INT            IDENTITY (1, 1) NOT NULL,
    [ProviderId]     INT            NULL,
    [OrganisationId] INT            NULL,
    [SearchText]     NVARCHAR (714) NOT NULL,
    [ProviderName]   NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK__Provider__1A72B934B1B0996A] PRIMARY KEY NONCLUSTERED ([ProviderTextId] ASC)
);




GO

CREATE FULLTEXT INDEX ON [Search].[ProviderText] ([SearchText]) KEY INDEX [PK__Provider__1A72B934B1B0996A] ON [Course_CourseTitle]

