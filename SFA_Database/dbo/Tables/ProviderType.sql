CREATE TABLE [dbo].[ProviderType] (
    [ProviderTypeId]   INT           NOT NULL,
    [ProviderTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ProviderType] PRIMARY KEY CLUSTERED ([ProviderTypeId] ASC)
);

