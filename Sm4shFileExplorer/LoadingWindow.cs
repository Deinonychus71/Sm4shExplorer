using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sm4shFileExplorer
{
    public partial class LoadingWindow : Form
    {
        public Label LabelProgress { get { return labelProgress; } }
        public ProgressBar ProgressBar { get { return progressBar; } }

        public LoadingWindow()
        {
            InitializeComponent();
        }
    }
}
