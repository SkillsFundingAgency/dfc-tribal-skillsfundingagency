CREATE TABLE [dbo].[OrganisationType] (
    [OrganisationTypeId]   INT            NOT NULL,
    [OrganisationTypeName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_OrganisationType] PRIMARY KEY CLUSTERED ([OrganisationTypeId] ASC)
);

