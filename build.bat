@ECHO OFF
pushd src
nuget restore
popd
rem call version.bat
%windir%\Microsoft.Net\Framework\v4.0.30319\msbuild src\GitAutoCommit.sln /m /property:Configuration=Debug /property:Platform="Any CPU"
%windir%\Microsoft.Net\Framework\v4.0.30319\msbuild src\GitAutoCommit.sln /m /property:Configuration=Release /property:Platform="Any CPU"
if ERRORLEVEL 1 pause
call src\gitversioner.bat ba
call src\gitversioner.bat w zip.bat
call zip.bat
call src\gitversioner.bat r zip.bat