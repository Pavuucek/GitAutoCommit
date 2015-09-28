#region License

/*
Copyright (c) 2011 Gareth Lennox (garethl@dwakn.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.
    * Neither the name of Gareth Lennox nor the names of its
    contributors may be used to endorse or promote products derived from this
    software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using GitAutoCommit.Properties;
using Microsoft.Win32;

namespace GitAutoCommit.Core
{
    public class AutoCommitHandler : IDisposable
    {
        private static readonly string GitExeName = Environment.OSVersion.Platform == PlatformID.Unix
            ? "git"
            : "git.exe";

        private readonly ConcurrentBag<string> _changes = new ConcurrentBag<string>();
        private int _intervalSeconds = 30;
        private Timer _timer;
        private string _verboseCommitMessage = string.Empty;
        private FileSystemWatcher _watcher;

        public AutoCommitHandler()
        {
        }

        /// <exception cref="FileNotFoundException">The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" /> could not be found.</exception>
        public AutoCommitHandler(int intervalSeconds, string folder)
        {
            _intervalSeconds = intervalSeconds;
            Folder = folder;

            OnConfigurationChange();
        }

        public string Folder { get; set; }

        public int Interval
        {
            get { return _intervalSeconds; }
            set
            {
                if (value <= 0)
                    value = 30;

                _intervalSeconds = value;
            }
        }

        public string CommitMessage { get; set; }


        /// <summary>
        ///     Gets a value indicating whether OS is 64bit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [is64 bit]; otherwise, <c>false</c>.
        /// </value>
        private static bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8 ||
                       !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        /// <exception cref="FileNotFoundException">The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" /> could not be found.</exception>
        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                _watcher = null;
            }

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        #endregion

        /// <exception cref="FileNotFoundException">The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" /> could not be found.</exception>
        public void OnConfigurationChange()
        {
            if (string.IsNullOrEmpty(Folder) || Interval <= 0)
                return;

            Dispose();

            // first run git to add untracked changes
            Console.WriteLine(Resources.AutoCommitHandler_OnConfigurationChange_Synchronizing_untracked_changes, Folder);
            RunGit("add .");
            RunGit("commit --file=-", Resources.AutoCommitHandler_OnConfigurationChange_Synchronizing_untracked_changes_commit);
            //then proceed...

            _watcher = new FileSystemWatcher(Folder) {IncludeSubdirectories = true};
            _watcher.Changed += watcher_Changed;
            _watcher.Created += watcher_Changed;
            _watcher.Deleted += watcher_Changed;
            _watcher.Renamed += watcher_Renamed;
            _watcher.EnableRaisingEvents = true;

            _timer = new Timer(_intervalSeconds*1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }


        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_changes.Count == 0)
                return;

            try
            {
                _timer.Stop();

                var changes = new HashSet<string>();

                string result;
                while (_changes.TryTake(out result))
                    changes.Add(result);

                if (changes.Count > 0)
                {
                    foreach (var file in changes)
                    {
                        //no file...
                        /* if (!File.Exists(file))
                            continue;*/

                        Console.WriteLine(Resources.AutoCommitHandler__timer_Elapsed_Committing_changes, file);
                        //RunGit("add \"" + file + "\"");
                    }
                    RunGit("add .");
                    RunGit("commit --file=-", CommitMessage.Replace("{DETAILS}", _verboseCommitMessage));
                    // erase verbose commit message
                    _verboseCommitMessage = string.Empty;
                }
            }
            finally
            {
                _timer.Start();
            }
        }

        /// <summary>
        ///     Gets Program Files directory
        /// </summary>
        /// <returns>Program Files or Program Files (x86) directory</returns>
        private static string ProgramFilesX86()
        {
            if (Is64Bit)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        /// <summary>
        ///     Finds the git binary.
        /// </summary>
        /// <returns></returns>
        private static string FindGitBinary()
        {
            string git = null;
            RegistryKey key;

            // Try the PATH environment variable

            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (pathEnv != null)
                foreach (var dir in pathEnv.Split(Path.PathSeparator))
                {
                    var sdir = dir;
                    if (sdir.StartsWith("\"") && sdir.EndsWith("\""))
                    {
                        // Strip quotes (no Path.PathSeparator supported in quoted directories though)
                        sdir = sdir.Substring(1, sdir.Length - 2);
                    }
                    git = Path.Combine(sdir, GitExeName);
                    if (File.Exists(git)) break;
                }
            if (!File.Exists(git)) git = null;


            // Read registry uninstaller key
            if (git == null)
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1");
                if (key != null)
                {
                    var loc = key.GetValue("InstallLocation");
                    if (loc is string)
                    {
                        git = Path.Combine((string) loc, Path.Combine("bin", GitExeName));
                        if (!File.Exists(git)) git = null;
                    }
                }
            }


            // Try 64-bit registry key
            if (git == null && Is64Bit)
            {
                key =
                    Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1");
                if (key != null)
                {
                    var loc = key.GetValue("InstallLocation");
                    if (loc is string)
                    {
                        git = Path.Combine((string) loc, Path.Combine("bin", GitExeName));
                        if (!File.Exists(git)) git = null;
                    }
                }
            }

            // Search program files directory
            if (git == null)
            {
                foreach (
                    var dir in
                        Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                            "git*"))
                {
                    git = Path.Combine(dir, Path.Combine("bin", GitExeName));
                    if (!File.Exists(git)) git = null;
                }
            }

            // Try 32-bit program files directory
            if (git != null || !Is64Bit) return git;
            foreach (var dir in Directory.GetDirectories(ProgramFilesX86(), "git*"))
            {
                git = Path.Combine(dir, Path.Combine("bin", GitExeName));
                if (!File.Exists(git)) git = null;
            }

            return git;
        }

        private string StripFolder(string fullPath)
        {
            fullPath = fullPath.Replace(Folder, string.Empty);
            if (fullPath[0] == '\\') fullPath = fullPath.Substring(1);
            return fullPath;
        }

        private void RunGit(string arguments, string pipeIn = null)
        {
            var start = new ProcessStartInfo(FindGitBinary(), arguments)
            {
                WorkingDirectory = Folder,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(start);

            if (process != null)
            {
                process.StandardInput.Write(pipeIn);
                process.StandardInput.Close();
#if DEBUG
                var output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
#endif
                var error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(error))
                    Console.WriteLine(error);

                process.WaitForExit();
            }
        }

        private void watcher_Renamed(object source, RenamedEventArgs e)
        {
            if (e.Name.StartsWith(".git") || e.Name.EndsWith(".tmp"))
                return;
            _verboseCommitMessage += string.Format("{1} renamed to {0}{2}", StripFolder(e.FullPath),
                StripFolder(e.OldFullPath), Environment.NewLine);
            _changes.Add(e.FullPath);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name.StartsWith(".git") || e.Name.EndsWith(".tmp"))
                return;
            var line = string.Format("{1} {0}{2}", StripFolder(e.FullPath), e.ChangeType, Environment.NewLine);
            // we don't want multiple same messages
            if (!_verboseCommitMessage.EndsWith(line)) _verboseCommitMessage += line;
            _changes.Add(e.FullPath);
        }

        /// <exception cref="FileNotFoundException">The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" /> could not be found.</exception>
        public void SetProperties(string folder, string commitMessage, int intervalSeconds,
            bool fireConfigurationChange = false)
        {
            Folder = folder;
            CommitMessage = commitMessage;
            _intervalSeconds = intervalSeconds;

            if (fireConfigurationChange)
                OnConfigurationChange();
        }
    }
}