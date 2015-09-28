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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GitAutoCommit.Controls
{
    public class NonFlickeringListView : ListView
    {
        private const int WmLButtonDblClk = 0x203;

        /// <summary>
        ///     Disable the double click functionality
        /// </summary>
        [DefaultValue(false)]
        public bool DisableDoubleClick { get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr handle, int messg, int wparam, int lparam);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // read current style
            var styles = (ListViewExtendedStyles) SendMessage(Handle, (int) ListViewMessages.GetExtendedStyle, 0, 0);
            // enable double buffer and border select
            styles |= ListViewExtendedStyles.DoubleBuffer | ListViewExtendedStyles.BorderSelect;
            // write new style
            SendMessage(Handle, (int) ListViewMessages.SetExtendedStyle, 0, (int) styles);
        }

        /// <summary>
        ///     Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            //bypass the internal double click handling to disable auto-checking on double click...
            if (DisableDoubleClick && m.Msg == WmLButtonDblClk)
            {
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons, 1, MousePosition.X, MousePosition.Y, 0));
                return;
            }

            base.WndProc(ref m);
        }

        #region Nested type: ListViewExtendedStyles

        [Flags]
        private enum ListViewExtendedStyles
        {
            /// <summary>
            ///     LVS_EX_BORDERSELECT
            /// </summary>
            BorderSelect = 0x00008000,

            /// <summary>
            ///     LVS_EX_DOUBLEBUFFER
            /// </summary>
            DoubleBuffer = 0x00010000
        }

        #endregion

        #region Nested type: ListViewMessages

        private enum ListViewMessages
        {
            First = 0x1000,
            SetExtendedStyle = (First + 54),
            GetExtendedStyle = (First + 55)
        }

        #endregion
    }
}