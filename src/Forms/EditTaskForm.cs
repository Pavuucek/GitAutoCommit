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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GitAutoCommit.Core;
using GitAutoCommit.Properties;
using GitAutoCommit.Support;

namespace GitAutoCommit.Forms
{
    /// <summary>
    ///     Form for editing tasks
    /// </summary>
    public partial class EditTaskForm : HeadingForm
    {
        private static readonly Interval[] Intervals =
        {
            new Interval(1),
            new Interval(5),
            new Interval(10),
            new Interval(15),
            new Interval(30),
            new Interval(60),
            new Interval(60*2),
            new Interval(60*5),
            new Interval(60*10),
            new Interval(60*15),
            new Interval(60*30),
            new Interval(60*60)
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditTaskForm" /> class.
        /// </summary>
        public EditTaskForm()
        {
            InitializeComponent();
            LoadLanguage();
            commitMessageTextBox.Font = FontHelper.MonospaceFont;

            intervalComboBox.Items.AddRange(Intervals);
        }

        /// <summary>
        ///     Loads language constants from resources.
        /// </summary>
        private void LoadLanguage()
        {
            Text = Resources.EditTaskForm_Caption;
            folderLabel.Text = Resources.EditTaskForm_folderLabel;
            nameLabel.Text = Resources.EditTaskForm_nameLabel;
            intervalLabel.Text = Resources.EditTaskForm_intervalLabel;
            commitMessageLabel.Text = Resources.EditTaskForm_commitMessageLabel;
            okButton.Text = Resources.ButtonOk;
            cancelButton.Text = Resources.ButtonCancel;
        }

        /// <summary>
        ///     Edits the task.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public DialogResult EditTask(AutoCommitTask item, IWin32Window owner)
        {
            Bind(item);

            var result = ShowDialog(owner);

            if (result == DialogResult.OK)
            {
                UnBind(item);
            }

            return result;
        }

        private void UnBind(AutoCommitTask item)
        {
            var interval = intervalComboBox.SelectedItem as Interval;
            item.SetProperties(nameTextBox.Text, folderTextBox.Text, commitMessageTextBox.Text,
                interval == null ? 30 : interval.Seconds);
        }

        private void Bind(AutoCommitTask item)
        {
            if (string.IsNullOrEmpty(item.Handler.Folder))
                Text = Resources.EditTaskForm_Bind_add_task;

            if (string.IsNullOrEmpty(item.CommitMessage)) item.CommitMessage = "";

            //normalises the line endings
            var commitMessage = item.CommitMessage
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", "\r\n");

            nameTextBox.Text = item.Name;
            folderTextBox.Text = item.Folder;
            commitMessageTextBox.Text = commitMessage;

            if (nameTextBox.Text == "" && folderTextBox.Text != "")
                nameTextBox.Text = Path.GetFileName(folderTextBox.Text);

            if (commitMessageTextBox.Text == "")
                commitMessageTextBox.Text = Resources.EditTaskForm_Bind_Automatic_commit;

            intervalComboBox.SelectedItem = Intervals.FirstOrDefault(x => x.Seconds == item.Interval);
        }

        private bool FormIsValid()
        {
            if (folderTextBox.Text == "" || !Directory.Exists(folderTextBox.Text))
            {
                MessageBox.Show(this, Resources.EditTaskForm_FormIsValid_Please_enter_a_valid_folder,
                    Resources.AppName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            //check if it is a git repository
            if (!Directory.Exists(Path.Combine(folderTextBox.Text, ".git")))
            {
                MessageBox.Show(this,
                    Resources.EditTaskForm_FormIsValid_The_selected_folder_doesnt_seem_to_be_a_git_repository,
                    Resources.AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (nameTextBox.Text == "")
                nameTextBox.Text = Path.GetFileName(folderTextBox.Text);

            if (commitMessageTextBox.Text == "")
                commitMessageTextBox.Text = Resources.EditTaskForm_Bind_Automatic_commit;

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (FormIsValid())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void browseFolderButton_Click(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                browser.ShowNewFolderButton = true;
                browser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (browser.ShowDialog(this) == DialogResult.OK)
                {
                    folderTextBox.Text = browser.SelectedPath;

                    if (nameTextBox.Text == "")
                        nameTextBox.Text = Path.GetFileName(folderTextBox.Text);
                }
            }
        }
    }
}