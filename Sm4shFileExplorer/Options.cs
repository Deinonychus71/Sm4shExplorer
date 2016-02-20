using Sm4shFileExplorer.Globals;
using Sm4shProjectManager.Globals;
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
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent(); 
        }

        #region Browse buttons
        private void btnDirHexEditor_Click(object sender, EventArgs e)
        {
            openFileDialog.DefaultExt = "exe";
            openFileDialog.Filter = "Application|*.exe";
            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtDirHexEditor.Text = openFileDialog.FileName;
        }

        private void btnDirTempFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtDirTempFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnDirExtractionFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtDirExtractionFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnDirWorkplaceFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtDirWorkplaceFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnDirExportFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtDirExportFolder.Text = folderBrowserDialog.SelectedPath;
        }
        #endregion

        private void Options_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                txtDirHexEditor.Text = AppConfig.Sm4shProject.ProjectHexEditorPath;

                txtDirTempFolder.Text = PathHelper.GetProjectTempFolder();
                txtDirExtractionFolder.Text = PathHelper.GetProjectExtractFolder();
                txtDirWorkplaceFolder.Text = PathHelper.GetProjectWorkplaceFolder();
                txtDirExportFolder.Text = PathHelper.GetProjectExportFolder();

                chkDebug.Checked = AppConfig.Sm4shProject.Debug;
                chkForceOriginalFlags.Checked = AppConfig.Sm4shProject.KeepOriginalFlags;
                chkSkipJunkEntries.Checked = AppConfig.Sm4shProject.SkipJunkEntries;
                chkSeeExportResults.Checked = AppConfig.Sm4shProject.ExportCSVList;
                chkCSVExportIgnoreCompSize.Checked = AppConfig.Sm4shProject.ExportCSVIgnoreCompSize;
                chkCSVExportIgnoreFlags.Checked = AppConfig.Sm4shProject.ExportCSVIgnoreFlags;
                chkCSVExportIgnoreOffsetInPack.Checked = AppConfig.Sm4shProject.ExportCSVIgnorePackOffsets;
                chkExportAddDate.Checked = AppConfig.Sm4shProject.ExportWithDateFolder;
            }
        }
    }
}
