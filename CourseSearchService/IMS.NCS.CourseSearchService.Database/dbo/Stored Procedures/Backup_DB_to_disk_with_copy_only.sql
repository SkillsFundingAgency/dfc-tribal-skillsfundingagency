CREATE PROCEDURE [dbo].[Backup_DB_to_disk_with_copy_only]
(   
    @DBName         varchar(128),
    @FileName        varchar(1024)
)
WITH EXECUTE AS OWNER
AS
BEGIN

    Backup database @DBName To Disk = @FileName with copy_only
	
END
