@ECHO OFF

rem call version.bat
%windir%\Microsoft.Net\Framework\v4.0.30319\msbuild src\GitAutoCommit.sln  /property:Configuration=Debug /property:Platform="Any CPU"
%windir%\Microsoft.Net\Framework\v4.0.30319\msbuild src\GitAutoCommit.sln  /property:Configuration=Release /property:Platform="Any CPU"

if ERRORLEVEL 1 pause

copy /y license.txt bin\Debug\license.txt
copy /y license.txt bin\Release\license.txt

gitversioner.exe w zip.bat
call zip.bat
gitversioner r zip.bat