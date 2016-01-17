@curl --form token=3aO3AS5M0k8tsNAXXMeKiQ --form email=michal.kuncl@gmail.com --form file=@coverity-GitAutoCommit.tar --form version="$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$" --form description="$Branch$:$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$" https://scan.coverity.com/builds?project=Pavuucek%2FGitAutoCommit
@exit
