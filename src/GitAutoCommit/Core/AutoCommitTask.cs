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

using System.IO;
using System.Xml.Serialization;

namespace GitAutoCommit.Core
{
    [XmlType("task")]
    public class AutoCommitTask
    {
        public AutoCommitTask()
        {
            Handler = new AutoCommitHandler();
        }

        public AutoCommitTask(int interval, string folder)
        {
            Handler = new AutoCommitHandler(interval, folder);
        }

        [XmlIgnore]
        public AutoCommitHandler Handler { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("folder")]
        public string Folder
        {
            get { return Handler.Folder; }
            set { Handler.Folder = value; }
        }

        [XmlAttribute("interval")]
        public int Interval
        {
            get { return Handler.Interval; }
            set { Handler.Interval = value; }
        }

        [XmlElement("message")]
        public string CommitMessage
        {
            get { return Handler.CommitMessage; }
            set { Handler.CommitMessage = value; }
        }

        public void SetProperties(string name, string folder, string commitMessage, int interval)
        {
            Name = name;
            Handler.SetProperties(folder, commitMessage, interval, true);
        }

        public bool IsValid()
        {
            return Directory.Exists(Folder) && Directory.Exists(Path.Combine(Folder, ".git"));
        }
    }
}