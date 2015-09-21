using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitAutoCommit.Support
{
    public class DebugWindowWriter : TextWriter
    {
        private TextBox textbox;

        public DebugWindowWriter(TextBox textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(string value)
        {
            if (textbox.InvokeRequired)
            {
                textbox.Invoke(new MethodInvoker(() => textbox.AppendText(value)));
            }
            else
            {
                textbox.AppendText(value);
            }
        }

        public override void WriteLine(string value)
        {
            if (textbox.InvokeRequired)
            {
                textbox.Invoke(new MethodInvoker(() => textbox.AppendText(value + Environment.NewLine)));
            }
            else
            {
                textbox.AppendText(value + Environment.NewLine);
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}
