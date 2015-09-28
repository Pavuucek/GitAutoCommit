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
using System.Xml.Serialization;

namespace GitAutoCommit.Core
{
    [XmlType("git-auto-commit-settings")]
    public class GACApplication
    {
        private static readonly string SettingsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "git-auto-commit");

        private static readonly string SettingsFile = Path.Combine(SettingsDirectory, "settings.xml");

        public GACApplication()
        {
        }

        public GACApplication(bool startup)
        {
            if (Directory.Exists(SettingsDirectory) && File.Exists(SettingsFile))
            {
                var serializer = new XmlSerializer(typeof (GACApplication));

                try
                {
                    using (var file = new FileStream(SettingsFile, FileMode.Open, FileAccess.Read))
                    {
                        Tasks = ((GACApplication) serializer.Deserialize(file)).Tasks;
                        Tasks.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    }

                    foreach (var task in Tasks)
                    {
                        try
                        {
                            task.Handler.OnConfigurationChange();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            if (Tasks == null)
                Tasks = new List<AutoCommitTask>();
        }

        public GACApplication(bool isCommandLineDriven, IEnumerable<AutoCommitTask> tasks)
        {
            IsCommandLineDriven = isCommandLineDriven;
            Tasks = tasks.ToList();
        }

        [XmlIgnore]
        public bool IsCommandLineDriven { get; set; }

        [XmlElement("task")]
        public List<AutoCommitTask> Tasks { get; set; }

        public void Save()
        {
            Tasks.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));

            var serializer = new XmlSerializer(typeof (GACApplication));

            if (!Directory.Exists(SettingsDirectory))
                Directory.CreateDirectory(SettingsDirectory);

            var tempFile = SettingsFile + ".temp";

            using (var file = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                serializer.Serialize(file, this, namespaces);
            }

            if (File.Exists(SettingsFile))
            {
                File.Replace(tempFile, SettingsFile, null, true);
            }
            else
            {
                File.Move(tempFile, SettingsFile);
            }
        }
    }
}