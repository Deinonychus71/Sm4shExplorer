using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sm4shFileExplorer.UI
{
    internal partial class ReorderPlugins : Form
    {
        private Sm4shProject _Project;

        public ReorderPlugins(Sm4shProject project)
        {
            InitializeComponent();

            _Project = project;
        }

        public void LoadGridView()
        {
            dataGrd.DataSource = _Project.Plugins;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (dataGrd.SelectedRows.Count == 0)
                return;

            Sm4shBasePlugin selectedPlugin = dataGrd.SelectedRows[0].DataBoundItem as Sm4shBasePlugin;
            int index = dataGrd.SelectedRows[0].Index;
            if (selectedPlugin == null || index == 0)
                return;

            _Project.Plugins.RemoveAt(index);
            _Project.Plugins.Insert(index - 1, selectedPlugin);

            dataGrd.Rows[index - 1].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (dataGrd.SelectedRows.Count == 0)
                return;

            Sm4shBasePlugin selectedPlugin = dataGrd.SelectedRows[0].DataBoundItem as Sm4shBasePlugin;
            int index = dataGrd.SelectedRows[0].Index;
            if (selectedPlugin == null)
                return;

            if (index >= _Project.Plugins.Count - 1)
                return;

            _Project.Plugins.RemoveAt(index);
            _Project.Plugins.Insert(index + 1, selectedPlugin);

            dataGrd.Rows[index + 1].Selected = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Sm4shMod mod = _Project.CurrentProject;
            mod.PluginsOrder = new List<string>();
            foreach (Sm4shBasePlugin plugin in _Project.Plugins)
                mod.PluginsOrder.Add(plugin.Filename);

            _Project.SaveProject();

            this.Close();
        }
    }
}
