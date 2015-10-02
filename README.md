# Git Auto-Commit #
-------------------
[![Build status](https://ci.appveyor.com/api/projects/status/hygwdaf7jbn2r8pm?svg=true)](https://ci.appveyor.com/project/Pavuucek/git-auto-commit) [![Stories in Ready](https://badge.waffle.io/Pavuucek/git-auto-commit.png?label=ready&title=Ready)](https://waffle.io/Pavuucek/git-auto-commit)

Git Auto-Commit is a tiny C#/.net 4 app that sits in your system tray, 
watching folders you specify. When a file is changed or added, it waits 
a specified length of time, then automatically commits the changed files 
to a git repository.


### usage: ###
    git-auto-commit <commit-interval> <dir 1>, <dir 2>, ..., <dir n>

where:

    commit-interval:   commit interval in seconds
    dir1,2,n:          directories to watch

Note that [Git for Windows](https://git-for-windows.github.io/) is required for this app to function.
Also it is assumed that all directories are already git repositories.


git-auto-commit is licensed under the [BSD license](license.txt). See license.txt for 
more information.

git-auto-commit is copyright (c) 2011 Gareth Lennox (garethl@dwakn.com)
All rights reserved.
Original version version can be found at [BitBucket](https://bitbucket.org/garethl/git-auto-commit).
This is a modified version by Michal Kuncl (https://github.com/pavuucek)

**Have questions?**
Feel free to ask in chat!
[![Join the chat at https://gitter.im/pavuucek/git-auto-commit](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/pavuucek/git-auto-commit?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

**Want to contribute?**
Fork this repo and open a pull request!

**Found a bug?**
Well, nobody's perfect ;-) [Just open an issue](https://github.com/Pavuucek/git-auto-commit/issues/new). 

