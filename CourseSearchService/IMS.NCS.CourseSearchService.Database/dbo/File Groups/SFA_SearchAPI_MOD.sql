﻿/*
Do not change the database path or name variables.
Any sqlcmd variables will be properly substituted during 
build and deployment.
*/
ALTER DATABASE [$(DatabaseName)]
	ADD FILEGROUP [SFA_SearchAPI_MOD] CONTAINS MEMORY_OPTIMIZED_DATA
