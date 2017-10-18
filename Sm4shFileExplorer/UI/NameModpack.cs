using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sm4shFileExplorer.UI
{
    public partial class ModpackName : Form
    {
        public string _ModpackName = null;

        public ModpackName(string defaultName)
        {
            InitializeComponent();

            textBox1.Text = defaultName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Regex badChars = new Regex("[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]");
            if (textBox1.Text == null || textBox1.Text.Length == 0 || badChars.IsMatch(textBox1.Text))
            {
                MessageBox.Show($"Invalid name. You cannot use a blank name, and you cannot use any of these characters in the modpack name: {new string(Path.GetInvalidPathChars())}");
                return;
            }
            _ModpackName = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
