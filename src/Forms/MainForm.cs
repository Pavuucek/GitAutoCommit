﻿#region License

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
using System.Reflection;
using System.Windows.Forms;
using GitAutoCommit.Core;
using GitAutoCommit.Properties;

namespace GitAutoCommit.Forms
{
    /// <summary>
    ///     Main Form class
    /// </summary>
    public partial class MainForm : HeadingForm
    {
        private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private readonly GacApplication _application;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public MainForm(GacApplication application)
        {
            _application = application;
            InitializeComponent();
            LoadLanguage();
            tasksLabel.Font = FontHelper.SubHeadingGuiFont;

            list.Bind(application.Tasks);
            list.ItemAdd += ListOnItemChange;
            list.ItemEdit += ListOnItemChange;
            list.ListChanged += ListOnChange;

            versionLabel.Text = Version;
            versionLabel.Left = pictureBox1.Left - 1 - versionLabel.Width;
        }

        /// <summary>
        ///     Loads language constants from resources.
        /// </summary>
        private void LoadLanguage()
        {
            Text = Resources.AppName;
            tasksLabel.Text = Resources.MainForm_tasksLabel;
        }

        private void ListOnChange(object sender, EventArgs e)
        {
            _application.Save();
        }

        private void ListOnItemChange(object sender, ValueEventArgs<AutoCommitTask> e)
        {
            var item = e.Value ?? new AutoCommitTask();

            using (var form = new EditTaskForm())
            {
                if (form.EditTask(item, this) == DialogResult.OK)
                {
                    e.Value = item;
                }
            }
        }
    }
}