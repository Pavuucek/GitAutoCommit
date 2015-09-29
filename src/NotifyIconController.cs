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
    /// <summary>
    ///     Controller for icon in tray area
    /// </summary>
    public class NotifyIconController
    {
        private readonly GacApplication _application;
        private readonly NotifyIcon _notifyIcon;

        private MainForm _mainForm;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotifyIconController" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public NotifyIconController(GacApplication application)
        {
            _application = application;
            _notifyIcon = new NotifyIcon();
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(
                new ToolStripItem[]
                {
                    new ToolStripMenuItem(Resources.NotifyIconController_NotifyIconController_About, null, (s, e) => ShowAbout()),
                    new ToolStripSeparator(), 
                    new ToolStripMenuItem(Resources.NotifyIconController_NotifyIconController_Configuration, null,
                        (s, e) => ShowMainForm()),
                    new ToolStripMenuItem(Resources.NotifyIconController_NotifyIconController_Exit, null,
                        (s, e) => Close())
                }
                );

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.Icon = Resources.icon_16;
            _notifyIcon.Text = Resources.AppName;

            _notifyIcon.DoubleClick += delegate { ShowMainForm(); };

            if (!application.IsCommandLineDriven && application.Tasks.Count == 0)
            {
                ShowMainForm();
            }
        }

        private void ShowAbout()
        {
            
        }

        /// <summary>
        ///     Shows the main form.
        /// </summary>
        private void ShowMainForm()
        {
            if (_mainForm == null)
            {
                _mainForm = new MainForm(_application);
                _mainForm.Closed += (sender, args) => _mainForm = null;
            }

            _mainForm.Show();
        }

        /// <summary>
        ///     Shows this instance.
        /// </summary>
        public void Show()
        {
            _notifyIcon.Visible = true;
        }

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public void Close()
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}