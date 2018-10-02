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
    internal partial class CreationProjectInfo : Form
    {
        private Sm4shMod _Config;
        private Sm4shProject _Project;

        public CreationProjectInfo(Sm4shMod config, Sm4shProject project)
        {
            InitializeComponent();

            _Config = config;
            _Project = project;

            ddpGameRegion.Items.Add("JPN (Zone 1)");
            ddpGameRegion.Items.Add("USA (Zone 2)");
            ddpGameRegion.Items.Add("??? (Zone 3)");
            ddpGameRegion.Items.Add("EUR (Zone 4)");  
        }

        private void ddpGameRegion_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                ddpGameRegion.SelectedIndex = _Config.GameRegionID - 1;
                txtGameVersion.Value = _Config.GameVersion;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Config.GameVersion = (int)txtGameVersion.Value;
            _Config.GameRegionID = ddpGameRegion.SelectedIndex + 1;
            _Project.SaveProject();
            this.Close();
        }
    }
}
