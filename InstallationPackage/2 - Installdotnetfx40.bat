start /w "" "%~dp0\dotnetfx40\dotNetFx40_Full_x86_x64.exe" /passive /norestart
IF %ERRORLEVEL% == 3010 EXIT /B 0
EXIT /B %ERRORLEVEL%