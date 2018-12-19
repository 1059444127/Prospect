@ECHO OFF
mkdir C:\data
sqlcmd -S .\SQLEXPRESS -Q "RESTORE DATABASE [Prospect] FROM  DISK = N'%~dp0Data\Prospect.bak' WITH  FILE = 1,  MOVE N'Prospect' TO N'C:\Data\Prospect.mdf',  MOVE N'Prospect_log' TO N'C:\Data\Prospect_log.ldf',  NOUNLOAD,  STATS = 10"