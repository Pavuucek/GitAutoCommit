@echo off
7z a git-auto-commit_debug-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\bin\Debug\*.*
7z a git-auto-commit_release-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\bin\Release\*.*
exit