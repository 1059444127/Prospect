@ECHO OFF
start /w "" "%~dp0SQLServer\en_sql_server_2008_r2_express_x86.exe" /QS /ACTION=Install /FEATURES=SQL /INSTANCENAME=SQLEXPRESS /SQLSVCACCOUNT="NT AUTHORITY\NETWORK SERVICE" /SECURITYMODE=SQL /SAPWD="Prospect2013" /IACCEPTSQLSERVERLICENSETERMS
IF %ERRORLEVEL% == 3010 EXIT /B 0
EXIT /B %ERRORLEVEL%