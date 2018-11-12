CREATE  PROCEDURE [dbo].[Backup_DB]
(   
    @DBName         varchar(128),
    @Directory         varchar(1024)
)
AS
BEGIN

    Declare @FileName      varchar(256)
    Declare @Date             datetime
    Declare @FullPath        varchar(1280) 
    Declare @UserName      varchar(256)


    Set  @Date = getdate()
    Set @UserName  = (select SUBSTRING(SUSER_NAME(),CHARINDEX('\', SUSER_NAME())+1,100)) 
    Set  @FullPath= ''

    print @UserName

    Set @FileName = @DBName + '_'  + Cast(Year(@Date) as varchar(4)) + '_' + 
                    Right('00' + Cast(Month(@Date) as varchar(2)),2) + '_' +
                    Right('00' + Cast(Day(@Date) as varchar(2)),2) + '-' + 
                    Right('00' + Cast(DatePart(hh,@Date) as varchar(2)),2) + '_' + 
                    Right('00' + Cast(DatePart(mi,@Date) as varchar(2)),2) + '_' +
                    Right('00' + Cast(DatePart(ss,@Date) as varchar(2)),2) + '_' + 'FULL_' + LOWER(@UserName) + '.BAK'


    If Right(@Directory,1) <> '\'
    --Begin
        Set @Directory = @Directory + '\'
    --End
    Set @FullPath = @Directory + @FileName

    Print @FullPath

    DECLARE @execute_backup VARCHAR(MAX)

    set @execute_backup = @DBName + '.dbo.Backup_DB_to_disk_with_copy_only ' + '"' + @DBName + '"' + ',' + '"' + @FullPath + '"'


    execute (@execute_backup)

END
