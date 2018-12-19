start /w "" "%~dp0\dotnetfx35\wcu\dotNetFramework\dotNetFx35setup.exe" /qb /norestart
IF %ERRORLEVEL% == 3010 EXIT /B 0
EXIT /B %ERRORLEVEL%