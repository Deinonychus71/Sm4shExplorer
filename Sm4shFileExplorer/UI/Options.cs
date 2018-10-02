using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System;
using System.Windows.Forms;

namespace Sm4shFileExplorer.UI
{
    internal partial class Options : Form
    {
        private Sm4shMod _Project;

        public Options(Sm4shMod project)
        {
            InitializeComponent();
            _Project = project;
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
                txtDirHexEditor.Text = _Project.ProjectHexEditorFile;

                txtDirTempFolder.Text = PathHelper.FolderTemp;
                txtDirExtractionFolder.Text = PathHelper.FolderExtract;
                txtDirWorkplaceFolder.Text = PathHelper.FolderWorkplace;
                txtDirExportFolder.Text = PathHelper.FolderExport;

                chkDebug.Checked = _Project.Debug;
                chkForceOriginalFlags.Checked = _Project.KeepOriginalFlags;
                chkSkipJunkEntries.Checked = _Project.SkipJunkEntries;
                chkSeeExportResults.Checked = _Project.ExportCSVList;
                chkCSVExportIgnoreCompSize.Checked = _Project.ExportCSVIgnoreCompSize;
                chkCSVExportIgnoreFlags.Checked = _Project.ExportCSVIgnoreFlags;
                chkCSVExportIgnoreOffsetInPack.Checked = _Project.ExportCSVIgnorePackOffsets;
                chkExportAddDate.Checked = _Project.ExportWithDateFolder;
            }
        }
    }
}
