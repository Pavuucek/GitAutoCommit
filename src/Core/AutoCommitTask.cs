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
    /// <summary>
    ///     Auto commit task class
    /// </summary>
    [XmlType("task")]
    public class AutoCommitTask
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AutoCommitTask" /> class.
        /// </summary>
        public AutoCommitTask()
        {
            Handler = new AutoCommitHandler();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutoCommitTask" /> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="folder">The folder.</param>
        public AutoCommitTask(int interval, string folder)
        {
            Handler = new AutoCommitHandler(interval, folder);
        }

        /// <summary>
        ///     Gets or sets the handler.
        /// </summary>
        /// <value>
        ///     The handler.
        /// </value>
        [XmlIgnore]
        public AutoCommitHandler Handler { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the folder.
        /// </summary>
        /// <value>
        ///     The folder.
        /// </value>
        [XmlAttribute("folder")]
        public string Folder
        {
            get { return Handler.Folder; }
            set { Handler.Folder = value; }
        }

        /// <summary>
        ///     Gets or sets the interval.
        /// </summary>
        /// <value>
        ///     The interval.
        /// </value>
        [XmlAttribute("interval")]
        public int Interval
        {
            get { return Handler.Interval; }
            set { Handler.Interval = value; }
        }

        /// <summary>
        ///     Gets or sets the commit message.
        /// </summary>
        /// <value>
        ///     The commit message.
        /// </value>
        [XmlElement("message")]
        public string CommitMessage
        {
            get { return Handler.CommitMessage; }
            set { Handler.CommitMessage = value; }
        }

        /// <summary>
        ///     Sets the properties.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="commitMessage">The commit message.</param>
        /// <param name="interval">The interval.</param>
        /// <exception cref="FileNotFoundException">
        ///     The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" />
        ///     could not be found.
        /// </exception>
        public void SetProperties(string name, string folder, string commitMessage, int interval)
        {
            Name = name;
            Handler.SetProperties(folder, commitMessage, interval, true);
        }

        /// <summary>
        ///     Determines whether this instance is valid.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Directory.Exists(Folder) && Directory.Exists(Path.Combine(Folder, ".git"));
        }
    }
}