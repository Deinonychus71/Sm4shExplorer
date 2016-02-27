using DamienG.Security.Cryptography;
using Sm4shFileExplorer.Globals;
using Sm4shProjectManager;
using Sm4shProjectManager.Globals;
using Sm4shProjectManager.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sm4shFileExplorer
{
    public partial class Main : Form
    {
        #region Members
        ConsoleRedirText _ConsoleText = null;
        ConsoleRedirProgress _ConsoleProgress = null;
        bool _MainLoaded = false;
        LoadingWindow _LoadingWindow;
        Sm4shProject _ProjectManager = null;
        Options _Options = new Options();
        About _About = new About();
        #endregion

        #region Properties
        public bool MainLoaded { get { return _MainLoaded; } }
        #endregion

        #region Constructors
        public Main()
        {
            InitializeComponent();

            //Version
            this.Text += " v." + GlobalConstants.VERSION;

            //Loading ProjectManager
            _ProjectManager = new Sm4shProject();

            //Loading configuration
            if (!File.Exists(UIConstants.CONFIG_FILE))
            {
                if (!CreateConfig())
                {
                    this.Close();
                    return;
                }
            }

            //Treeview setup
            treeView.ImageList = new ImageList();
            treeView.ImageList.Images.Add(UIConstants.ICON_FOLDER, Resource.icon_folder);
            treeView.ImageList.Images.Add(UIConstants.ICON_FILE, Resource.icon_file);
            treeView.ImageList.Images.Add(UIConstants.ICON_PACKED, Resource.icon_packed);

            //Console Redirection
            _ConsoleText = new ConsoleRedirText(textConsole);
            _ConsoleProgress = new ConsoleRedirProgress(backgroundWorker);

            _MainLoaded = true;
        }
        #endregion

        #region EventHandlers
        private void menuBuild_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(Strings.INFO_PACK_REBUILD, AppConfig.Sm4shProject.ProjectExportPath), Strings.CAPTION_PACK_REBUILD);
            _ProjectManager.RebuildRFAndPatchlist();
        }

        private void menuBuildDebug_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(Strings.INFO_PACK_REBUILD, AppConfig.Sm4shProject.ProjectExportPath), Strings.CAPTION_PACK_REBUILD);
            _ProjectManager.RebuildRFAndPatchlist(false);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dataGridView.Rows.Clear();
            PopulateGridView(e.Node);
        }

        private void contextMenuTreeView_Opening(object sender, CancelEventArgs e)
        {
            TreeNode selNode = (TreeNode)treeView.GetNodeAt(treeView.PointToClient(Cursor.Position));
            extractToolStripMenuItem.Enabled = true;
            removeModToolStripMenuItem.Enabled = false;
            unlocalizeToolStripMenuItem.Enabled = false;
            removeUnlocalizeToolStripMenuItem.Enabled = false;
            packThisFolderToolStripMenuItem.Enabled = false;
            if (selNode != null)
            {
                ResourceCollection resCol = GetResourceCollectionFromNode(selNode);

                //Mod File
                string filePath = GetWorkspaceFileFromNode(selNode);
                if (File.Exists(filePath))
                {
                    removeModToolStripMenuItem.Enabled = true;
                    return;
                }
                //Mode Folder
                if (Directory.Exists(filePath))
                {
                    removeModToolStripMenuItem.Enabled = true;
                    //if (_ProjectManager.GetPackedPath(resCol, GetFullPathOfNode(selNode)) == null)
                    //    packThisFolderToolStripMenuItem.Enabled = true;
                    return;
                }

                ResourceItem rItem = GetResourceFromNode(selNode);
                if (rItem != null)
                {
                    if (!string.IsNullOrEmpty(resCol.Region) && rItem.OffInPack == 0)
                    {
                        if (!AppConfig.Sm4shProject.IsUnlocalized(resCol.Region, rItem.Path))
                            unlocalizeToolStripMenuItem.Enabled = true;
                        else
                            removeUnlocalizeToolStripMenuItem.Enabled = true;
                    }
                    //if (rItem.IsFolder && rItem.OffInPack == 0 && !rItem.IsAPackage)
                    //    packThisFolderToolStripMenuItem.Enabled = true;
                    return;
                }
            }
            e.Cancel = true;
        }

        private void unlocalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceItem rootItem = GetResourceFromNode(node);
                if (rootItem != null)
                {
                    _ProjectManager.UnlocalizeResources(rootItem.ResourceCollection, rootItem.Path);
                    if (node.TreeView != null)
                        RefreshTreeNodeStyle(node, true);
                }
            }
        }

        private void removeUnlocalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceItem rootItem = GetResourceFromNode(node);
                if (rootItem != null)
                {
                    _ProjectManager.RemoveUnlocalized(rootItem.ResourceCollection, rootItem.Path);
                    if (node.TreeView != null)
                        RefreshTreeNodeStyle(node, true);
                }
            }
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            foreach (TreeNode childNode in e.Node.Nodes)
                RefreshTreeNodeStyle(childNode, false);
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            Point p = treeView.PointToClient(new Point(e.X, e.Y));
            treeView.SelectedNode = treeView.GetNodeAt(p.X, p.Y);
            AddOrReplaceFiles(treeView.SelectedNode, e.Data.GetData(DataFormats.FileDrop) as string[]);
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            Point p = treeView.PointToClient(new Point(e.X, e.Y));
            TreeNode node = treeView.GetNodeAt(p.X, p.Y);
            treeView.SelectedNode = node;
            if (node != null && node.Parent != null && node.SelectedImageKey != UIConstants.ICON_FILE)
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null && node.SelectedImageKey != UIConstants.ICON_FOLDER && node.SelectedImageKey != UIConstants.ICON_PACKED)
            {
                if (string.IsNullOrEmpty(AppConfig.Sm4shProject.ProjectHexEditorPath))
                {
                    MessageBox.Show(Strings.INFO_FILE_HEX, Strings.CAPTION_FILE_HEX);
                    return;
                }
                ResourceCollection resCol = GetResourceCollectionFromNode(node);
                string path = GetFullPathOfNode(node);
                if (string.IsNullOrEmpty(path))
                    return;
                    
                //Extract
                string fullpath = _ProjectManager.ExtractFile(resCol, path, PathHelper.GetProjectExtractFolder());
                uint crcFile = Crc32.Compute(File.ReadAllBytes(fullpath));
                Process process = Process.Start(AppConfig.Sm4shProject.ProjectHexEditorPath, "\"" + fullpath + "\"");
                process.WaitForExit();
                uint compareCrcFile = Crc32.Compute(File.ReadAllBytes(fullpath));
                if (crcFile != compareCrcFile)
                {
                    if (MessageBox.Show(string.Format(Strings.INFO_FILE_MODIFIED, path), Strings.CAPTION_FILE_MODIFIED, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        AddOrReplaceFiles(treeView.SelectedNode.Parent, new string[] {fullpath});
                }
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceCollection resCol = GetResourceCollectionFromNode(node);
                string path = GetFullPathOfNode(node);
                if (node.Text.Contains("."))
                    _ProjectManager.ExtractFile(resCol, path, PathHelper.GetProjectExtractFolder());
                else
                    _ProjectManager.ExtractFolder(resCol, path, PathHelper.GetProjectExtractFolder());
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceCollection resCol = GetResourceCollectionFromNode(node);
                string path = GetFullPathOfNode(node);
                RemovePathFromTreeView(resCol, node);
                _ProjectManager.RemoveFilesFromWorkspace(resCol, path);
                if(node.TreeView != null)
                    RefreshTreeNodeStyle(node, true);
            }
        }

        private void packThisFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceItem rItem = GetResourceFromNode(node);
                if (rItem != null)
                {
                    if (MessageBox.Show(Strings.WARNING_PACK_FOLDER, Strings.CAPTION_PACK_FOLDER, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        _ProjectManager.PackFolder(rItem.ResourceCollection, rItem.Path);
                        if (node.TreeView != null)
                            RefreshTreeNodeStyle(node, true);
                    }
                }
            }
        }

        private void refreshTreeviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            PopulateTreeView(_ProjectManager.ResourceData);
        }

        private void menuOptions_Click(object sender, EventArgs e)
        {
            _Options.ShowDialog(this);
            AppConfig.Sm4shProject.ProjectHexEditorPath = _Options.txtDirHexEditor.Text;
            AppConfig.Sm4shProject.ProjectExtractPath = _Options.txtDirExtractionFolder.Text;
            AppConfig.Sm4shProject.ProjectExportPath = _Options.txtDirExportFolder.Text;
            AppConfig.Sm4shProject.ProjectTempPath = _Options.txtDirTempFolder.Text;
            AppConfig.Sm4shProject.ProjectWorkplacePath = _Options.txtDirWorkplaceFolder.Text;
            AppConfig.Sm4shProject.Debug = _Options.chkDebug.Checked;
            AppConfig.Sm4shProject.SkipJunkEntries = _Options.chkSkipJunkEntries.Checked;
            AppConfig.Sm4shProject.KeepOriginalFlags = _Options.chkForceOriginalFlags.Checked;
            AppConfig.Sm4shProject.ExportCSVList = _Options.chkSeeExportResults.Checked;
            AppConfig.Sm4shProject.ExportCSVIgnoreCompSize = _Options.chkCSVExportIgnoreCompSize.Checked;
            AppConfig.Sm4shProject.ExportCSVIgnoreFlags = _Options.chkCSVExportIgnoreFlags.Checked;
            AppConfig.Sm4shProject.ExportCSVIgnorePackOffsets = _Options.chkCSVExportIgnoreOffsetInPack.Checked;
            AppConfig.Sm4shProject.ExportWithDateFolder = _Options.chkExportAddDate.Checked;
            _ProjectManager.SaveProject();
        }

        private void thanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _About.ShowDialog(this);
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Methods
        #region TreeView
        private void PopulateTreeView(ResourceCollection[] resourceData)
        {
            treeView.Nodes.Clear();
            Console.WriteLine("Populating Treeview...");
            treeView.Sort();
            foreach (ResourceCollection resourceCollection in resourceData)
                PopulateTreeView(resourceCollection);
            Console.WriteLine("Done.");
        }

        private void PopulateTreeView(ResourceCollection resourceCollection)
        {
            TreeNode currentnode;
            char[] cachedpathseparator = "/".ToCharArray();
            TreeNode rootNode = new TreeNode("data" + resourceCollection.Region, 0, 0);
            rootNode.Name = resourceCollection.Region;
            rootNode.Tag = resourceCollection;
            string[] paths = resourceCollection.GetFilteredResources().Keys.ToArray();
            foreach (string path in paths)
            {
                currentnode = rootNode;
                foreach (string subPath in path.Split(cachedpathseparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (null == currentnode.Nodes[subPath])
                        currentnode = currentnode.Nodes.Add(subPath, subPath);
                    else
                        currentnode = currentnode.Nodes[subPath];
                }
            }
            PopulateTreeViewIncremental(rootNode);
            treeView.Nodes.Add(rootNode);
        }

        private void PopulateTreeViewIncremental(TreeNode treenode)
        {
            ResourceCollection resCol = GetResourceCollectionFromNode(treenode);
            if (resCol == null)
                return;
            Console.WriteLine(string.Format("Refreshing Treeview mod files for data{0}...", resCol.Region));
            char[] cachedpathseparator = "/".ToCharArray();
            string[] paths = _ProjectManager.GetAllModPaths("data" + resCol.Region);
            TreeNode currentnode;

            foreach (string path in paths)
            {
                currentnode = treenode;
                foreach (string subPath in path.Split(cachedpathseparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (null == currentnode.Nodes[subPath])
                        currentnode = currentnode.Nodes.Add(subPath, subPath);
                    else
                        currentnode = currentnode.Nodes[subPath];
                }
            }
        }

        private void RefreshTreeNodeStyle(TreeNode node, bool recursive)
        {
            if (node == null)
                return;

            ResourceItem rItem = GetResourceFromNode(node);

            //Checking if file is in workspace (mod)
            string modPath = GetWorkspaceFileFromNode(node);
            if (File.Exists(modPath))
            {
                node.ForeColor = UIConstants.NODE_MOD;
                node.SelectedImageKey = UIConstants.ICON_FILE;
                node.ImageKey = UIConstants.ICON_FILE;
            }
            else if (Directory.Exists(modPath))
            {
                node.ForeColor = UIConstants.NODE_MOD;
                node.SelectedImageKey = UIConstants.ICON_FOLDER;
                node.ImageKey = UIConstants.ICON_FOLDER;
            }
            else if (rItem != null)
            {
                if (AppConfig.Sm4shProject.IsUnlocalized(rItem.ResourceCollection.Region, rItem.Path))
                    node.ForeColor = UIConstants.NODE_MOD_UNLOCALIZED;
                else if (rItem.Source == FileSource.Patch)
                    node.ForeColor = UIConstants.NODE_PATCH;
                else if (rItem.Source == FileSource.LS || rItem.Source == FileSource.NotFound)
                    node.ForeColor = UIConstants.NODE_LS;
            }
            if (rItem != null)
            {
                if (rItem.IsFolder)
                {
                    if (rItem.IsAPackage)
                    {
                        node.SelectedImageKey = UIConstants.ICON_PACKED;
                        node.ImageKey = UIConstants.ICON_PACKED;
                    }
                    else
                    {
                        node.SelectedImageKey = UIConstants.ICON_FOLDER;
                        node.ImageKey = UIConstants.ICON_FOLDER;
                    }
                }
                else
                {
                    node.SelectedImageKey = UIConstants.ICON_FILE;
                    node.ImageKey = UIConstants.ICON_FILE;
                }
            }

            if (recursive && node.IsExpanded) 
                foreach (TreeNode childNode in node.Nodes)
                    RefreshTreeNodeStyle(childNode, recursive);
        }

        private void RemovePathFromTreeView(ResourceCollection resCol, TreeNode node)
        {
            ResourceItem rItem = GetResourceFromNode(node);
            if (rItem != null)
            {
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                    RemovePathFromTreeView(resCol, node.Nodes[i]);
                return;
            }
            node.Remove();
        }

        private ResourceItem GetResourceFromNode(TreeNode selectedNode)
        {
            ResourceItem rItem = null;
            TreeNode currentNode = selectedNode;

            if (currentNode == null || currentNode.Parent == null)
                return null;

            if (currentNode.Tag == null)
            {
                string fullpath = GetFullPathOfNode(currentNode);
                ResourceCollection resCol = GetResourceCollectionFromNode(currentNode);
                if (resCol.Resources.ContainsKey(fullpath))
                {
                    rItem = resCol.Resources[fullpath];
                    selectedNode.Tag = rItem;
                }
            }
            else if (selectedNode.Parent != null)
                rItem = ((ResourceItem)(currentNode.Tag));

            return rItem;
        }

        private ResourceCollection GetResourceCollectionFromNode(TreeNode node)
        {
            TreeNode rootNode = GetFirstLevelNode(node);
            if (rootNode != null)
                return rootNode.Tag as ResourceCollection;

            return null;
        }

        private string GetFullPathOfNodeWithRegion(TreeNode node)
        {
            TreeNode currentNode = node;
            string path = node.Text;
            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
                path = currentNode.Text + "/" + path;
            }
            if ((path.Contains("/") && !path.Substring(path.LastIndexOf("/")).Contains(".")) || !path.Contains("/"))
                path += "/";

            return path;
        }

        private string GetFullPathOfNode(TreeNode node)
        {
            string path = GetFullPathOfNodeWithRegion(node);
            return path.Substring(path.IndexOf("/") + 1);
        }

        private string GetWorkspaceFileFromNode(TreeNode node)
        {
            string path = GetFullPathOfNodeWithRegion(node);
            return PathHelper.GetProjectWorkplaceFolder() + path.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
        }

        private TreeNode GetFirstLevelNode(TreeNode node)
        {
            TreeNode rootNode = node;
            while (rootNode.Parent != null)
                rootNode = rootNode.Parent;

            return rootNode;
        }

        private void PopulateGridView(TreeNode node)
        {
            //ResourceCollection
            if (node.Parent == null && node.Tag is ResourceCollection)
            {
                ResourceCollection resCol = (ResourceCollection)node.Tag;
                dataGridView.Rows.Add("Name", "data" + resCol.Region);
                dataGridView.Rows.Add("Nbr Resources", resCol.Resources.Count);
                return;
            }

            //File
            string filePath = GetWorkspaceFileFromNode(node);
            if (File.Exists(filePath))
            {
                dataGridView.Rows.Add("Name", Path.GetFileName(filePath));
                dataGridView.Rows.Add("Source", "Mod");
                return;
            }

            //Original Resource
            ResourceItem rItem = GetResourceFromNode(node);
            if(rItem != null)
            {
                dataGridView.Rows.Add("Name", rItem.Filename);
                dataGridView.Rows.Add("Compressed size", rItem.CmpSize);
                dataGridView.Rows.Add("Decompressed size", rItem.DecSize);
                dataGridView.Rows.Add("Flags", rItem.Flags);
                dataGridView.Rows.Add("Package", rItem.IsAPackage ? "Yes" : "No");
                string source = rItem.Source.ToString();
                if (source == "NotFound")
                    source = "Folder";
                dataGridView.Rows.Add("Source", source);
                dataGridView.Rows.Add("Region", rItem.ResourceCollection.Region);
                if (rItem.OffInPack != 0)
                    dataGridView.Rows.Add("Offset in pack", String.Format("0x{0:X8}", rItem.OffInPack));
                return;
            }
        }
        #endregion

        #region Add Files
        private void AddOrReplaceFiles(TreeNode node, string[] files)
        {
            if (files == null || files.Length == 0)
                return;

            TreeNode rootNode = GetFirstLevelNode(node);
            ResourceCollection resCol = GetResourceCollectionFromNode(node);
            string nodePath = GetFullPathOfNode(node);
            if (!string.IsNullOrEmpty(nodePath))
            {
                _ProjectManager.AddFilesToWorkspace(resCol, files, nodePath);

                //Update Treeview
                PopulateTreeViewIncremental(rootNode);

                //Update Style
                RefreshTreeNodeStyle(GetFirstLevelNode(node), true);
            }
        }
        #endregion

        #region Save/Load
        public bool CreateConfig()
        {
            MessageBox.Show(this, Strings.CREATE_PROJECT_FIND_FOLDER, Strings.CAPTION_CREATE_PROJECT);
            while (true)
            {
                DialogResult result = folderBrowserDialog.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string gamePath = folderBrowserDialog.SelectedPath + UIConstants.FOLDER_SEPARATOR;
                    if (!PathHelper.IsItSmashFolder(gamePath))
                    {
                        MessageBox.Show(this, string.Format(Strings.ERROR_LOADING_GAME_FOLDER, PathHelper.GetPath(gamePath, PathHelperEnum.FILE_LS), PathHelper.GetPath(gamePath, PathHelperEnum.FILE_META), PathHelper.GetPath(gamePath, PathHelperEnum.FILE_RPX), Strings.CAPTION_ERROR_LOADING_GAME_FOLDER));
                        continue;
                    }
                    if (!PathHelper.DoesItHavePatchFolder(gamePath))
                    {
                        MessageBox.Show(this, Strings.ERROR_LOADING_GAME_PATCH_FOLDER, Strings.CAPTION_ERROR_LOADING_GAME_FOLDER);
                        continue;
                    }

                    Console.WriteLine("Creating configuration file...");
                    Sm4shMod newProject = _ProjectManager.CreateNewProject(UIConstants.CONFIG_FILE, gamePath);
                    MessageBox.Show(this, Strings.CREATE_PROJECT_SUCCESS, Strings.CAPTION_CREATE_PROJECT);

                    return true;
                }
                else
                    return false;
            }
        }

        public void LoadConfig()
        {
            Sm4shMod loadedProject = _ProjectManager.LoadProject(UIConstants.CONFIG_FILE);
            if (loadedProject == null)
                return;
            AppConfig.Sm4shProject = loadedProject;
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            this.Enabled = false;
            _LoadingWindow = new LoadingWindow();
            _LoadingWindow.Show();
            _LoadingWindow.Left = Screen.PrimaryScreen.Bounds.Width / 2 - _LoadingWindow.Size.Width / 2;
            _LoadingWindow.Top = Screen.PrimaryScreen.Bounds.Height / 2 - _LoadingWindow.Size.Height / 2;
            _LoadingWindow.BringToFront();
            Console.SetOut(_ConsoleProgress);

            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadConfig();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _LoadingWindow.LabelProgress.Text = e.UserState.ToString();
            string progress = Regex.Match(_LoadingWindow.LabelProgress.Text, @"(\d+)/(\d+)").Value;
            if (progress.Contains("/"))
            {
                string[] progressValues = progress.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (progressValues.Length == 2)
                {
                    int totalValue = -1;
                    int currentValue = -1;
                    Int32.TryParse(progressValues[0], out currentValue);
                    Int32.TryParse(progressValues[1], out totalValue);
                    if (totalValue >= 0 && currentValue >= 0)
                    {
                        _LoadingWindow.ProgressBar.Maximum = totalValue;
                        _LoadingWindow.ProgressBar.Value = currentValue - 1;
                    }
                }
            }
            else
            {
                if(_LoadingWindow.ProgressBar.Maximum > _LoadingWindow.ProgressBar.Value)
                    _LoadingWindow.ProgressBar.Value++;
            }
            textConsole.AppendText(_LoadingWindow.LabelProgress.Text + "\r\n");
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (AppConfig.Sm4shProject == null)
            {
                MessageBox.Show(Strings.ERROR_LOADING_GAME_LOAD_FOLDER, Strings.CAPTION_ERROR_LOADING_GAME_FOLDER);
                Application.Exit();
                return;
            }

            this.Enabled = true;
            _LoadingWindow.Close();
            Console.SetOut(_ConsoleText);
            lblGameIDValue.Text = AppConfig.Sm4shProject.GameID;
            lblRegionValue.Text = AppConfig.Sm4shProject.GameRegion;
            lblVersionValue.Text = AppConfig.Sm4shProject.GameVersion;
            refreshTreeviewToolStripMenuItem_Click(this, null);
        }
        #endregion
        #endregion

        
    }
}
