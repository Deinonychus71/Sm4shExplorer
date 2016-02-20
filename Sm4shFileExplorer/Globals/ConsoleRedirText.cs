using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sm4shFileExplorer.Globals
{
    public class ConsoleRedirText : TextWriter
    {
        TextBox _output = null;

        public ConsoleRedirText(TextBox output)
        {
            _output = output;
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
            _output.AppendText(value.ToString() + "\r\n");
        }

        public override void Write(string value)
        {
            base.Write(value);
            _output.AppendText(value.ToString());
        }
 
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
