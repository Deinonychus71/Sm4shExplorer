using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sm4shFileExplorer.Globals
{
    public class ConsoleRedirProgress : TextWriter
    {
        BackgroundWorker _output = null;

        public ConsoleRedirProgress(BackgroundWorker output)
        {
            _output = output;
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
            _output.ReportProgress(0, value.ToString());
        }
 
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
