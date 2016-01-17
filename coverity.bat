@echo off
if exist "cov-int" rd /q /s "cov-int"
cov-build.exe --dir cov-int "build.bat"
7z a -tzip "coverity-GitAutoCommit.zip" "cov-int"
gitversioner w coverity-submit.bat
call coverity-submit.bat
gitversioner r coverity-submit.bat
if exist "cov-int" rd /q /s "cov-int"