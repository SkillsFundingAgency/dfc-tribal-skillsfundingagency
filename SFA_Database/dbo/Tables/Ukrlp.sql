CREATE TABLE [dbo].[Ukrlp]
(
	[Ukprn] INT NOT NULL PRIMARY KEY, 
    [LegalName] NVARCHAR(255) NOT NULL, 
    [TradingName] NVARCHAR(255) NULL, 
    [LegalPhoneNumber] NVARCHAR(100) NULL, 
    [LegalFaxNumber] NVARCHAR(100) NULL, 
    [PrimaryUKPhone] NVARCHAR(100) NULL, 
    [PrimaryUKFax] NVARCHAR(100) NULL, 
    [PrimaryContactName] NVARCHAR(255) NULL, 
    [CompanyRegistration] NVARCHAR(255) NULL, 
    [CharityRegistration] NVARCHAR(255) NULL, 
    [TaxReferenceNumber] NVARCHAR(100) NULL, 
    [UkrlpStatus] INT NULL, 
    [UpdatedDateTimeUtc] DATETIME NULL, 
    [LegalAddressId] INT NULL, 
    [PrimaryAddressId] INT NULL, 
    CONSTRAINT [FK_LegalAddressId] FOREIGN KEY ([LegalAddressId]) REFERENCES Address([AddressId]), 
    CONSTRAINT [FK_PrimaryAddressId] FOREIGN KEY ([PrimaryAddressId]) REFERENCES Address([AddressId]), 
    CONSTRAINT [FK_UkrlpStatus] FOREIGN KEY ([UkrlpStatus]) REFERENCES [RecordStatus]([RecordStatusId]), 
)
GO

CREATE INDEX [IX_Ukrlp_LegalAddressId] ON [dbo].[Ukrlp] ([LegalAddressId])
GO

CREATE INDEX [IX_Ukrlp_PrimaryAddressId] ON [dbo].[Ukrlp] ([PrimaryAddressId])
GO
