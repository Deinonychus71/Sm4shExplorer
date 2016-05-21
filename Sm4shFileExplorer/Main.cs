using DamienG.Security.Cryptography;
using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sm4shFileExplorer
{
    internal partial class Main : Form
    {
        #region Members
        ConsoleRedirText _ConsoleText = null;
        ConsoleRedirProgress _ConsoleProgress = null;
        bool _MainLoaded = false;
        Sm4shProject _ProjectManager = null;
        Options _Options;
        About _About = new About();
        ReorderPlugins _ReorderPlugins;
        #endregion

        #region Properties
        internal bool MainLoaded { get { return _MainLoaded; } }
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

            //Console Redirection
            _ConsoleText = new ConsoleRedirText(textConsole);
            _ConsoleProgress = new ConsoleRedirProgress(backgroundWorker);

            _MainLoaded = true;
        }
        #endregion

        #region EventHandlers
        #region directory menu
        private void openExtractDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(PathHelper.FolderExtract))
                Directory.CreateDirectory(PathHelper.FolderExtract);
            Process.Start("explorer.exe", PathHelper.FolderExtract);
        }

        private void openWorkspaceDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(PathHelper.FolderWorkplace))
                Directory.CreateDirectory(PathHelper.FolderWorkplace);
            Process.Start("explorer.exe", PathHelper.FolderWorkplace);
        }

        private void openExportDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(PathHelper.FolderExport))
                Directory.CreateDirectory(PathHelper.FolderExport);
            Process.Start("explorer.exe", PathHelper.FolderExport);
        }

        private void openSm4shexplorerDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }

        private void openTempDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(PathHelper.FolderTemp))
                Directory.CreateDirectory(PathHelper.FolderTemp);
            Process.Start("explorer.exe", PathHelper.FolderTemp);
        }

        private void openGameDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(_ProjectManager.CurrentProject.GamePath))
                Process.Start("explorer.exe", _ProjectManager.CurrentProject.GamePath);
        }
        #endregion

        private void menuBuild_Click(object sender, EventArgs e)
        {
            string exportFolder = PathHelper.FolderExport + "release" + Path.DirectorySeparatorChar + (_ProjectManager.CurrentProject.ExportWithDateFolder ? string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + Path.DirectorySeparatorChar : string.Empty);
            if (!Directory.Exists(exportFolder) || (Directory.Exists(exportFolder) && MessageBox.Show(string.Format(UIStrings.WARN_EXPORT_FOLDER_EXISTS, exportFolder), UIStrings.CAPTION_PACK_REBUILD, MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                MessageBox.Show(string.Format(UIStrings.INFO_PACK_REBUILD, _ProjectManager.CurrentProject.ProjectExportFolder), UIStrings.CAPTION_PACK_REBUILD);
                _ProjectManager.RebuildRFAndPatchlist();
            }
        }

        private void plugin_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem pluginMenuItem = sender as ToolStripMenuItem;
            if (pluginMenuItem != null)
            {
                Sm4shBasePlugin plugin = pluginMenuItem.Tag as Sm4shBasePlugin;
                if (plugin != null)
                {
                    plugin.InternalOpenPluginMenu();
                }
            }
        }

        private void menuBuildDebug_Click(object sender, EventArgs e)
        {
            string exportFolder = PathHelper.FolderExport + "debug" + Path.DirectorySeparatorChar + (_ProjectManager.CurrentProject.ExportWithDateFolder ? string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + Path.DirectorySeparatorChar : string.Empty);
            if (!Directory.Exists(exportFolder) || (Directory.Exists(exportFolder) && MessageBox.Show(string.Format(UIStrings.WARN_EXPORT_FOLDER_EXISTS, exportFolder), UIStrings.CAPTION_PACK_REBUILD, MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                MessageBox.Show(string.Format(UIStrings.INFO_PACK_REBUILD, _ProjectManager.CurrentProject.ProjectExportFolder), UIStrings.CAPTION_PACK_REBUILD);
                _ProjectManager.RebuildRFAndPatchlist(false);
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dataGridView.Rows.Clear();
            PopulateGridView(e.Node);
        }

        private void contextMenuTreeView_Opening(object sender, CancelEventArgs e)
        {
            TreeNode selNode = (TreeNode)treeView.GetNodeAt(treeView.PointToClient(Cursor.Position));
            if(selNode.Parent == null)
            {
                e.Cancel = true;
                return;
            }

            extractToolStripMenuItem.Enabled = true;
            removeModToolStripMenuItem.Enabled = false;
            unlocalizeToolStripMenuItem.Enabled = false;
            removeUnlocalizeToolStripMenuItem.Enabled = false;
            packThisFolderToolStripMenuItem.Enabled = false;
            removeResourceToolStripMenuItem.Enabled = false;
            reintroduceResourceToolStripMenuItem.Enabled = false;
            if (selNode != null)
            {
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

                ResourceItem rItem = _ProjectManager.GetResource(selNode.Name);
                if (rItem != null)
                {
                    if (rItem.ResourceCollection.IsRegion && rItem.OffInPack == 0)
                    {
                        if (!_ProjectManager.CurrentProject.IsUnlocalized(rItem.ResourceCollection.PartitionName, rItem.RelativePath))
                            unlocalizeToolStripMenuItem.Enabled = true;
                        else
                            removeUnlocalizeToolStripMenuItem.Enabled = true;
                    }
                    if (!_ProjectManager.CurrentProject.IsResourceRemoved(rItem.ResourceCollection.PartitionName, rItem.RelativePath))
                        removeResourceToolStripMenuItem.Enabled = true;
                    else
                        reintroduceResourceToolStripMenuItem.Enabled = true;
                    //if (rItem.IsFolder && rItem.OffInPack == 0 && !rItem.IsAPackage)
                    //    packThisFolderToolStripMenuItem.Enabled = true;
                    //TODO
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
                ResourceItem rootItem = _ProjectManager.GetResource(node.Name);
                if (rootItem != null)
                {
                    _ProjectManager.UnlocalizePath(rootItem.ResourceCollection, rootItem.RelativePath);
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
                ResourceItem rootItem = _ProjectManager.GetResource(node.Name);
                if (rootItem != null)
                {
                    _ProjectManager.RemoveUnlocalized(rootItem.ResourceCollection, rootItem.RelativePath);
                    if (node.TreeView != null)
                        RefreshTreeNodeStyle(node, true);
                }
            }
        }

        private void removeResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceItem rootItem = _ProjectManager.GetResource(node.Name);
                if (rootItem != null)
                {
                    _ProjectManager.RemoveOriginalResource(rootItem.ResourceCollection, rootItem.RelativePath);
                    if (node.TreeView != null)
                        RefreshTreeNodeStyle(node, true);
                }
            }
        }

        private void reintroduceResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceItem rootItem = _ProjectManager.GetResource(node.Name);
                if (rootItem != null)
                {
                    _ProjectManager.ReintroduceOriginalResource(rootItem.ResourceCollection, rootItem.RelativePath);
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
            if (node != null && node.Parent != null && node.SelectedImageKey != UIConstants.ICON_FOLDER && node.SelectedImageKey != UIConstants.ICON_PACKED)
            {
                string absolutePath = node.Name;
                if (string.IsNullOrEmpty(absolutePath))
                    return;
                    
                //Extract
                string fullExtractedFile = _ProjectManager.ExtractResource(absolutePath);
                uint crcFile = Crc32.Compute(File.ReadAllBytes(fullExtractedFile));

                //Plugin ResourceSelected hooks
                bool pluginUsed = false;
                string relativePath = _ProjectManager.GetRelativePath(absolutePath);
                ResourceCollection resCol = GetFirstLevelNode(node).Tag as ResourceCollection;
                foreach (Sm4shBasePlugin plugin in _ProjectManager.Plugins)
                {
                    if (plugin.InternalResourceSelected(resCol, relativePath, fullExtractedFile))
                    {
                        pluginUsed = true;
                        break;
                    }
                }

                //If no plugin used, try hexeditor
                if (!pluginUsed)
                {
                    if (string.IsNullOrEmpty(_ProjectManager.CurrentProject.ProjectHexEditorFile))
                    {
                        LogHelper.Info(UIStrings.INFO_FILE_HEX);
                        return;
                    }
                    Process process = Process.Start(_ProjectManager.CurrentProject.ProjectHexEditorFile, "\"" + fullExtractedFile + "\"");
                    process.WaitForExit();
                }

                //Check extract file, if changed, ask to add in workspace
                uint compareCrcFile = Crc32.Compute(File.ReadAllBytes(fullExtractedFile));
                if (crcFile != compareCrcFile)
                {
                    if (MessageBox.Show(string.Format(UIStrings.INFO_FILE_MODIFIED, absolutePath), UIStrings.CAPTION_FILE_MODIFIED, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        AddOrReplaceFiles(treeView.SelectedNode.Parent, new string[] { fullExtractedFile });
                }
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
                _ProjectManager.ExtractResource(node.Name);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                string path = node.Name;
                RemovePathFromTreeView(node);
                _ProjectManager.RemoveFileFromWorkspace(path);
                if(node.TreeView != null)
                    RefreshTreeNodeStyle(node, true);
            }
        }

        private void packThisFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null)
            {
                ResourceItem rItem = _ProjectManager.GetResource(node.Name);
                if (rItem != null)
                {
                    if (MessageBox.Show(UIStrings.WARNING_PACK_FOLDER, UIStrings.CAPTION_PACK_FOLDER, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        _ProjectManager.PackFolder(rItem.ResourceCollection, rItem.AbsolutePath);
                        if (node.TreeView != null)
                            RefreshTreeNodeStyle(node, true);
                    }
                }
            }
        }

        private void refreshTreeviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            PopulateTreeView(_ProjectManager.ResourceDataCollection);
        }

        private void menuOptions_Click(object sender, EventArgs e)
        {
            _Options.ShowDialog(this);
            _ProjectManager.CurrentProject.ProjectHexEditorFile = _Options.txtDirHexEditor.Text;
            _ProjectManager.CurrentProject.ProjectExtractFolder = _Options.txtDirExtractionFolder.Text;
            _ProjectManager.CurrentProject.ProjectExportFolder = _Options.txtDirExportFolder.Text;
            _ProjectManager.CurrentProject.ProjectTempFolder = _Options.txtDirTempFolder.Text;
            _ProjectManager.CurrentProject.ProjectWorkplaceFolder = _Options.txtDirWorkplaceFolder.Text;
            _ProjectManager.CurrentProject.Debug = _Options.chkDebug.Checked;
            _ProjectManager.CurrentProject.SkipJunkEntries = _Options.chkSkipJunkEntries.Checked;
            _ProjectManager.CurrentProject.KeepOriginalFlags = _Options.chkForceOriginalFlags.Checked;
            _ProjectManager.CurrentProject.ExportCSVList = _Options.chkSeeExportResults.Checked;
            _ProjectManager.CurrentProject.ExportCSVIgnoreCompSize = _Options.chkCSVExportIgnoreCompSize.Checked;
            _ProjectManager.CurrentProject.ExportCSVIgnoreFlags = _Options.chkCSVExportIgnoreFlags.Checked;
            _ProjectManager.CurrentProject.ExportCSVIgnorePackOffsets = _Options.chkCSVExportIgnoreOffsetInPack.Checked;
            _ProjectManager.CurrentProject.ExportWithDateFolder = _Options.chkExportAddDate.Checked;
            _ProjectManager.SaveProject();
        }

        private void orderPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ReorderPlugins == null)
            {
                _ReorderPlugins = new ReorderPlugins(_ProjectManager);
                _ReorderPlugins.LoadGridView();
            }
            _ReorderPlugins.ShowDialog(this);
        }

        private void thanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _About.ShowDialog(this);
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Loading
        private void Main_Shown(object sender, EventArgs e)
        {
            this.Enabled = false;
            Console.SetOut(_ConsoleProgress);

            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadConfig();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            textConsole.AppendText(e.UserState.ToString()  + Environment.NewLine);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadConfigCompleted();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_ProjectManager != null)
                _ProjectManager.CleanTempFolder();
        }
        #endregion
        #endregion

        #region Methods
        #region TreeView
        private void PopulateTreeView(ResourceCollection[] resourceData)
        {
            if (resourceData == null || resourceData.Length == 0)
            {
                LogHelper.Error("No data was found.");
                return;
            }

            treeView.BeginUpdate();
            LogHelper.Info("Populating Treeview...");
            treeView.Sort();
            treeView.Nodes.Clear();
            foreach (ResourceCollection resourceCollection in resourceData)
            {
                TreeNode rootNode = new TreeNode(resourceCollection.PartitionName, 0, 0);
                rootNode.Name = resourceCollection.PartitionName + "/";
                rootNode.Tag = resourceCollection;
                PopulateTreeViewResources(rootNode, resourceCollection.Nodes, resourceCollection.IsRegion);
                PopulateTreeViewWorkspace(rootNode);
                treeView.Nodes.Add(rootNode);
            }

            treeView.EndUpdate();
            LogHelper.Info("Done.");
        }

        private void PopulateTreeViewResources(TreeNode currentNode, List<ResourceItem> resourceCollection, bool isRegion)
        {
            foreach (ResourceItem resource in resourceCollection)
            {
                if (isRegion && resource.Source == FileSource.NotFound)
                    continue;
                string nodeName = resource.Filename;
                if (nodeName.EndsWith("/"))
                    nodeName = nodeName.Substring(0, nodeName.Length - 1);
                TreeNode subNode = new TreeNode(nodeName);
                subNode.Name = resource.AbsolutePath;
                //PopulateTreeViewResources(subNode, resource.Nodes, isRegion); //LAZY LOADING
                currentNode.Nodes.Add(subNode);
            }
        }

        private void PopulateTreeViewWorkspace(TreeNode currentNode)
        {
            TreeNode rootNode = GetFirstLevelNode(currentNode);
            char[] cachedpathseparator = "/".ToCharArray();
            string[] paths = _ProjectManager.GetAllWorkplaceRelativePaths(currentNode.Name, false);

            foreach (string path in paths)
            {
                if (null == currentNode.Nodes[currentNode.Name + path])
                {
                    string nodeName = path;
                    if (nodeName.EndsWith("/"))
                        nodeName = nodeName.Substring(0, nodeName.Length - 1);
                    currentNode.Nodes.Add(currentNode.Name + path, nodeName);
                }
            }
        }

        private void RefreshTreeNodeStyle(TreeNode node, bool recursive)
        {
            if (node == null)
                return;

            ResourceItem rItem = _ProjectManager.GetResource(node.Name);

            //See for subnodes
            if (rItem != null && node.Nodes.Count == 0 && rItem.Nodes.Count != 0)
                PopulateTreeViewResources(node, rItem.Nodes, rItem.ResourceCollection.IsRegion);
            PopulateTreeViewWorkspace(node);

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
                if (_ProjectManager.CurrentProject.IsResourceRemoved(rItem.ResourceCollection.PartitionName, rItem.RelativePath))
                    node.ForeColor = UIConstants.NODE_MOD_DELETED;
                else if (_ProjectManager.CurrentProject.IsUnlocalized(rItem.ResourceCollection.PartitionName, rItem.RelativePath))
                    node.ForeColor = UIConstants.NODE_MOD_UNLOCALIZED;
                else if (rItem.Source == FileSource.Patch)
                    node.ForeColor = UIConstants.NODE_PATCH;
                else if (rItem.Source == FileSource.LS || rItem.Source == FileSource.NotFound)
                    node.ForeColor = UIConstants.NODE_LS;
            }
            else
            {
                LogHelper.Warning(string.Format("The node '{0}' could not be found and has been removed.", node.Name));
                node.Remove();
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

            //Plugins
            ResourceCollection resCol = _ProjectManager.GetResourceCollection(node.Name);
            foreach (Sm4shBasePlugin plugin in _ProjectManager.Plugins)
            {
                if (plugin.Icons == null)
                    continue;

                int result = plugin.InternalCanResourceBeLoaded(resCol, _ProjectManager.GetRelativePath(node.Name));
                if (result > -1 && result < plugin.Icons.Length)
                {
                    node.SelectedImageKey = plugin.Name + result.ToString();
                    node.ImageKey = plugin.Name + result.ToString();
                    break;
                }
            }

            if (recursive && node.IsExpanded) 
                foreach (TreeNode childNode in node.Nodes)
                    RefreshTreeNodeStyle(childNode, recursive);
        }

        private void RemovePathFromTreeView(TreeNode node)
        {
            ResourceItem rItem = _ProjectManager.GetResource(node.Name);
            if (rItem != null)
            {
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                    RemovePathFromTreeView(node.Nodes[i]);
                return;
            }
            node.Remove();
        }

        private string GetWorkspaceFileFromNode(TreeNode node)
        {
            return PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH) + node.FullPath.Replace('/', Path.DirectorySeparatorChar);
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
                dataGridView.Rows.Add("Name", resCol.PartitionName);
                dataGridView.Rows.Add("Path", node.Name);
                dataGridView.Rows.Add("Nbr Resources", resCol.Resources.Count);
                return;
            }

            ResourceItem rItem = null;

            //File
            string filePath = GetWorkspaceFileFromNode(node);
            if (File.Exists(filePath) || Directory.Exists(filePath))
            {
                dataGridView.Rows.Add("Name", Path.GetFileName(filePath));
                dataGridView.Rows.Add("Path", node.Name);
                dataGridView.Rows.Add("Source", "Mod");
            }
            else
            {
                //Original Resource
                rItem = _ProjectManager.GetResource(node.Name);
                if (rItem != null)
                {
                    dataGridView.Rows.Add("Name", rItem.Filename);
                    dataGridView.Rows.Add("Path", rItem.AbsolutePath);
                    dataGridView.Rows.Add("Compressed size", rItem.CmpSize);
                    dataGridView.Rows.Add("Decompressed size", rItem.DecSize);
                    dataGridView.Rows.Add("Flags", rItem.Flags);
                    dataGridView.Rows.Add("Package", rItem.IsAPackage ? "Yes" : "No");
                    string source = rItem.Source.ToString();
                    if (source == "NotFound")
                        source = "Folder";
                    dataGridView.Rows.Add("Source", source);
                    dataGridView.Rows.Add("Partition", rItem.ResourceCollection.PartitionName);
                    if (rItem.OffInPack != 0)
                        dataGridView.Rows.Add("Offset in pack", String.Format("0x{0:X8}", rItem.OffInPack));
                }
            }

            ResourceCollection rootCol = GetFirstLevelNode(node).Tag as ResourceCollection;
            foreach (Sm4shBasePlugin plugin in _ProjectManager.Plugins)
            {
                Dictionary<string, string> dict = plugin.InternalGridViewPopulated(rootCol, node.Name, filePath);
                if(dict != null)
                    foreach(string key in dict.Keys)
                        dataGridView.Rows.Add(key, dict[key]);
            }
        }
        #endregion

        #region Add Files
        private void AddOrReplaceFiles(TreeNode node, string[] files)
        {
            if (files == null || files.Length == 0)
                return;

            TreeNode rootNode = GetFirstLevelNode(node);
            if (!string.IsNullOrEmpty(node.Name))
            {
                foreach (string file in files)
                    _ProjectManager.AddFileToWorkspace(file, node.Name);


                //Update Treeview
                PopulateTreeViewWorkspace(node);

                //Update Style
                RefreshTreeNodeStyle(rootNode, true);
            }
        }
        #endregion

        #region Save/Load
        public bool CreateConfig()
        {
            MessageBox.Show(this, UIStrings.CREATE_PROJECT_FIND_FOLDER, UIStrings.CAPTION_CREATE_PROJECT);
            while (true)
            {
                DialogResult result = folderBrowserDialog.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string gameFolder = folderBrowserDialog.SelectedPath + Path.DirectorySeparatorChar;
                    if (!PathHelper.IsItSmashFolder(gameFolder))
                    {
                        MessageBox.Show(this, UIStrings.ERROR_LOADING_GAME_FOLDER, UIStrings.CAPTION_ERROR_LOADING_GAME_FOLDER);
                        continue;
                    }
                    if (!PathHelper.DoesItHavePatchFolder(gameFolder))
                    {
                        MessageBox.Show(this, UIStrings.ERROR_LOADING_GAME_PATCH_FOLDER, UIStrings.CAPTION_ERROR_LOADING_GAME_FOLDER);
                        continue;
                    }

                    LogHelper.Info("Creating configuration file...");
                    Sm4shMod newProject = _ProjectManager.CreateNewProject(UIConstants.CONFIG_FILE, gameFolder);
                    new CreationProjectInfo(newProject, _ProjectManager).ShowDialog(this);
                    MessageBox.Show(this, UIStrings.CREATE_PROJECT_SUCCESS, UIStrings.CAPTION_CREATE_PROJECT);

                    return true;
                }
                else
                    return false;
            }
        }

        public void LoadConfig()
        {
            _ProjectManager.LoadProject(UIConstants.CONFIG_FILE);
            if (_ProjectManager.CurrentProject == null)
            {
                MessageBox.Show(this, UIStrings.ERROR_LOADING_PROJECT, UIStrings.CAPTION_ERROR_LOADING_GAME_FOLDER);
                Application.Exit();
                return;
            }
            _Options = new Options(_ProjectManager.CurrentProject);
        }

        public void LoadConfigCompleted()
        {
            if (_ProjectManager.CurrentProject == null)
            {
                MessageBox.Show(UIStrings.ERROR_LOADING_GAME_LOAD_FOLDER, UIStrings.CAPTION_ERROR_LOADING_GAME_FOLDER);
                Application.Exit();
                return;
            }

            this.Enabled = true;
            Console.SetOut(_ConsoleText);
            lblGameIDValue.Text = _ProjectManager.CurrentProject.GameID;
            lblRegionValue.Text = _ProjectManager.CurrentProject.GameRegion;
            lblVersionValue.Text = _ProjectManager.CurrentProject.GameVersion.ToString();

            //Treeview setup
            treeView.ImageList = new ImageList();
            treeView.ImageList.Images.Add(UIConstants.ICON_FOLDER, Resources.Resource.icon_folder);
            treeView.ImageList.Images.Add(UIConstants.ICON_FILE, Resources.Resource.icon_file);
            treeView.ImageList.Images.Add(UIConstants.ICON_PACKED, Resources.Resource.icon_packed);

            //Loading Plugins
            if (_ProjectManager.Plugins != null)
            {
                foreach (Sm4shBasePlugin plugin in _ProjectManager.Plugins)
                {
                    //Icon
                    if (plugin.Icons != null)
                    {
                        for(int i = 0; i < plugin.Icons.Length; i++)
                        treeView.ImageList.Images.Add(plugin.Name + i.ToString(), plugin.Icons[0]);
                    }

                    //Menu
                    if (!plugin.ShowInPluginList)
                        continue;

                    ToolStripMenuItem newMenuItem = new ToolStripMenuItem(string.Format("{0} [Research {1}, GUI {2}]", plugin.Name, plugin.Research, plugin.GUI));
                    newMenuItem.ToolTipText = plugin.Description;
                    newMenuItem.Click += new EventHandler(this.plugin_Click);
                    newMenuItem.Tag = plugin;

                    menuPlugins.Enabled = true;
                    menuPlugins.DropDownItems.Insert(0, newMenuItem);
                }
            }

            refreshTreeviewToolStripMenuItem_Click(this, null);
        }
        #endregion

        #endregion
    }
}
