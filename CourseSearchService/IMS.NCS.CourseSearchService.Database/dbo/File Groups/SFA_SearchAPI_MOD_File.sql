/*
Do not change the database path or name variables.
Any sqlcmd variables will be properly substituted during 
build and deployment.
*/
ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [SFA_SearchAPI_MOD_File],
		FILENAME = '$(DefaultDataPath)$(DefaultFilePrefix)_SqlFile1.ndf'
	)
	TO FILEGROUP [SFA_SearchAPI_MOD]
