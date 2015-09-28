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

using System.Windows.Forms;
using GitAutoCommit.Core;
using GitAutoCommit.Forms;
using GitAutoCommit.Properties;

namespace GitAutoCommit
{
    public class NotifyIconController
    {
        private readonly GACApplication _application;
        private readonly ContextMenuStrip _contextMenu;
        private readonly NotifyIcon _notifyIcon;

        private MainForm _mainForm;

        public NotifyIconController(GACApplication application)
        {
            _application = application;
            _notifyIcon = new NotifyIcon();
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.AddRange(
                new[]
                {
                    new ToolStripMenuItem("&Configuration", null, (s, e) => ShowMainForm()),
                    new ToolStripMenuItem("E&xit", null, (s, e) => Close())
                }
                );

            _notifyIcon.ContextMenuStrip = _contextMenu;
            _notifyIcon.Icon = Resources.icon_16;
            _notifyIcon.Text = "Git auto commit";

            _notifyIcon.DoubleClick += delegate { ShowMainForm(); };

            if (!application.IsCommandLineDriven && application.Tasks.Count == 0)
            {
                ShowMainForm();
            }
        }

        private void ShowMainForm()
        {
            if (_mainForm == null)
            {
                _mainForm = new MainForm(_application);
                _mainForm.Closed += (sender, args) => _mainForm = null;
            }

            _mainForm.Show();
        }

        public void Show()
        {
            _notifyIcon.Visible = true;
        }

        public void Close()
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}