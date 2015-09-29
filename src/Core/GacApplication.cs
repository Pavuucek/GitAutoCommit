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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GitAutoCommit.Core
{
    /// <summary>
    ///     GitAutoCommitApplication class
    /// </summary>
    [XmlType("git-auto-commit-settings")]
    public class GacApplication
    {
        /// <summary>
        ///     The settings directory
        /// </summary>
        private static readonly string SettingsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "git-auto-commit");

        /// <summary>
        ///     The settings directory if running in portable mode
        /// </summary>
        private static readonly string PortableSettingsDirectory = Path.GetDirectoryName(Application.ExecutablePath);

        /// <summary>
        ///     The settings file if running in portable mode
        /// </summary>
        private static readonly string PortableSettingsFile = Path.Combine(PortableSettingsDirectory, "settings.xml");

        /// <summary>
        ///     The settings file
        /// </summary>
        private static readonly string SettingsFile = Path.Combine(SettingsDirectory, "settings.xml");

        /// <summary>
        ///     Is application in portable mode?
        /// </summary>
        [XmlIgnore]
        public bool IsPortableMode;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GacApplication" /> class.
        /// </summary>
        public GacApplication()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GacApplication" /> class.
        /// </summary>
        /// <param name="startup">if set to <c>true</c> [startup].</param>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">Access to <paramref name="fileName" /> is denied. </exception>
        public GacApplication(bool startup)
        {
            if (File.Exists(PortableSettingsFile)) IsPortableMode = true; // enable portable mode
            var settingsFile = IsPortableMode ? PortableSettingsFile : SettingsFile;
            var settingsDirectory = IsPortableMode ? PortableSettingsDirectory : SettingsDirectory;
            if (Directory.Exists(settingsDirectory) && File.Exists(settingsFile))
            {
                if (new FileInfo(settingsFile).Length > 0)
                {
                    var serializer = new XmlSerializer(typeof (GacApplication));

                    try
                    {
                        using (var file = new FileStream(settingsFile, FileMode.Open, FileAccess.Read))
                        {
                            Tasks = ((GacApplication) serializer.Deserialize(file)).Tasks;
                            Tasks.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                        }

                        foreach (var task in Tasks)
                        {
                            try
                            {
                                task.Handler.OnConfigurationChange();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            if (Tasks == null)
                Tasks = new List<AutoCommitTask>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GacApplication" /> class.
        /// </summary>
        /// <param name="isCommandLineDriven">if set to <c>true</c> [is command line driven].</param>
        /// <param name="tasks">The tasks.</param>
        public GacApplication(bool isCommandLineDriven, IEnumerable<AutoCommitTask> tasks)
        {
            IsCommandLineDriven = isCommandLineDriven;
            Tasks = tasks.ToList();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is command line driven.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is command line driven; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsCommandLineDriven { get; set; }

        /// <summary>
        ///     Gets or sets the tasks.
        /// </summary>
        /// <value>
        ///     The tasks.
        /// </value>
        [XmlElement("task")]
        public List<AutoCommitTask> Tasks { get; set; }

        /// <summary>
        ///     Saves settings to xml file
        /// </summary>
        /// <exception cref="IOException">
        ///     The directory specified by <paramref name="path" /> is a file .-or-The network name is
        ///     not known.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="FileNotFoundException">
        ///     The file cannot be found, such as when <paramref name="mode" /> is
        ///     FileMode.Truncate or FileMode.Open, and the file specified by <paramref name="path" /> does not exist. The file
        ///     must already exist in these modes.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="DriveNotFoundException">An invalid drive was specified. </exception>
        public void Save()
        {
            var settingsFile = IsPortableMode ? PortableSettingsFile : SettingsFile;
            var settingsDirectory = IsPortableMode ? PortableSettingsDirectory : SettingsDirectory;

            Tasks.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));

            var serializer = new XmlSerializer(typeof (GacApplication));

            if (!Directory.Exists(settingsDirectory))
                Directory.CreateDirectory(settingsDirectory);

            var tempFile = settingsFile + ".temp";

            using (var file = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                serializer.Serialize(file, this, namespaces);
            }

            if (File.Exists(settingsFile))
            {
                File.Replace(tempFile, settingsFile, null, true);
            }
            else
            {
                File.Move(tempFile, settingsFile);
            }
        }
    }
}