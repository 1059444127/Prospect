@ECHO OFF
start /w "" "%~dp0\WindowsInstaller\WindowsInstaller4.5.exe" /passive /promptrestart
IF %ERRORLEVEL% == 3010 EXIT /B 0
EXIT /B %ERRORLEVEL%