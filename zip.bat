@echo off
7z a git-auto-commit_debug-$Branch$-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\bin\Debug\*.*
7z a git-auto-commit_release-$Branch$-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\bin\Release\*.*
exit