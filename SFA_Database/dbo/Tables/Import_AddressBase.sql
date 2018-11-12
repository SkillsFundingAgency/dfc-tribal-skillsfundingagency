CREATE TABLE [dbo].[Import_AddressBase]
(
	[UPRN] NUMERIC(12) NOT NULL PRIMARY KEY, 
    [Postcode] NVARCHAR(8) NOT NULL, 
    [OrganisationName] NVARCHAR(60) NULL, 
    [DepartmentName] NVARCHAR(60) NULL, 
    [POBoxNumber] NVARCHAR(6) NULL, 
    [BuildingName] NVARCHAR(50) NULL, 
    [SubBuildingName] NVARCHAR(30) NULL, 
    [BuildingNumber] INT NULL, 
    [DependentThoroughfareName] NVARCHAR(80) NULL, 
    [ThoroughfareName] NVARCHAR(80) NULL, 
    [Town] NVARCHAR(30) NULL, 
    [DoubleDependentLocality] NVARCHAR(35) NULL, 
    [DependentLocality] NVARCHAR(35) NULL, 
    [Latitude] DECIMAL(9, 7) NULL, 
    [Longitude] DECIMAL(8, 7) NULL  
)
