CREATE FUNCTION [dbo].[GetPrimaryKeyForTable](@TableName NVARCHAR(128))
RETURNS NVARCHAR(128)
AS 
BEGIN

DECLARE @PrimaryKey NVARCHAR(128)
	
SET @PrimaryKey = (SELECT c.name 
from sys.index_columns ic 
    join sys.indexes i on ic.index_id=i.index_id
    join sys.columns c on c.column_id=ic.column_id
where 
    i.[object_id] = object_id(@TableName) and 
    ic.[object_id] = object_id(@TableName) and 
    c.[object_id] = object_id(@TableName) and
    is_primary_key = 1)
    
    RETURN @PrimaryKey	
END

