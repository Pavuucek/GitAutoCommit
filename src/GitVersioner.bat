@ECHO OFF
rem This file by Joshua Flanagan https://gist.github.com/joshuaflanagan/1044159
SETLOCAL
REM This can be used for any .exe installed by a nuget package
REM Example usage: nuget_tool.bat nunit-console.exe myproject.tests.dll
SET TOOL=GitVersioner.exe
FOR /R %~dp0\packages %%G IN (%TOOL%) DO (
  IF EXIST %%G (
    SET TOOLPATH=%%G
    GOTO FOUND
  )
)
IF '%TOOLPATH%'=='' GOTO NOTFOUND

:FOUND
%TOOLPATH% %*
GOTO :EOF

:NOTFOUND
ECHO %TOOL% not found.
EXIT /B 1