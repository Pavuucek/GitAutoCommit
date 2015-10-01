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
using System.Text;
using System.Windows.Forms;

namespace GitAutoCommit.Support
{
    /// <summary>
    /// DebugWindow helper class
    /// </summary>
    public class DebugWindowWriter : TextWriter
    {
        private readonly TextBox _textbox;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugWindowWriter"/> class.
        /// </summary>
        /// <param name="textbox">The textbox.</param>
        public DebugWindowWriter(TextBox textbox)
        {
            _textbox = textbox;
        }

        /// <summary>
        /// When overridden in a derived class, returns the character encoding in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        /// <summary>
        /// Writes a string to the text string or stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public override void Write(string value)
        {
            if (_textbox.InvokeRequired)
            {
                _textbox.Invoke(new MethodInvoker(() => _textbox.AppendText(value)));
            }
            else
            {
                _textbox.AppendText(value);
            }
        }

        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <param name="value">The string to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
        public override void WriteLine(string value)
        {
            if (_textbox.InvokeRequired)
            {
                _textbox.Invoke(new MethodInvoker(() => _textbox.AppendText(value + Environment.NewLine)));
            }
            else
            {
                _textbox.AppendText(value + Environment.NewLine);
            }
        }
    }
}