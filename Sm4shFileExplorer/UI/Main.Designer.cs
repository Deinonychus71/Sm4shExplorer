namespace Sm4shFileExplorer.UI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBuildDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshTreeviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.oMenuFileSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.directoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGameDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSdDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.openExtractDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkspaceDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openExportDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.openSm4shexplorerDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTempDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlugins = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.orderPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSm4shExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.unlocalizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeUnlocalizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.removeResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reintroduceResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.packThisFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doNotPackThisFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.textConsole = new System.Windows.Forms.TextBox();
            this.oPanelLeft = new System.Windows.Forms.Panel();
            this.oPanelFileInformation = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.gridViewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridViewValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPanelInformation = new System.Windows.Forms.Panel();
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.lblRegionValue = new System.Windows.Forms.Label();
            this.lblVersionValue = new System.Windows.Forms.Label();
            this.lblGameIDValue = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblGameID = new System.Windows.Forms.Label();
            this.oTreeViewPanel = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip.SuspendLayout();
            this.contextMenuTreeView.SuspendLayout();
            this.oPanelLeft.SuspendLayout();
            this.oPanelFileInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.oPanelInformation.SuspendLayout();
            this.groupInfo.SuspendLayout();
            this.oTreeViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProject,
            this.directoriesToolStripMenuItem,
            this.menuPlugins,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(884, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuProject
            // 
            this.menuProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBuild,
            this.menuBuildDebug,
            this.sendToSDToolStripMenuItem,
            this.toolStripSeparator3,
            this.refreshTreeviewToolStripMenuItem,
            this.menuOptions,
            this.oMenuFileSeparator,
            this.menuExit});
            this.menuProject.Name = "menuProject";
            this.menuProject.Size = new System.Drawing.Size(56, 20);
            this.menuProject.Text = "Project";
            this.menuProject.DropDownOpening += new System.EventHandler(this.menuProject_DropDownOpening);
            // 
            // menuBuild
            // 
            this.menuBuild.Name = "menuBuild";
            this.menuBuild.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.menuBuild.Size = new System.Drawing.Size(299, 22);
            this.menuBuild.Text = "Build Modpack";
            this.menuBuild.Click += new System.EventHandler(this.menuBuild_Click);
            // 
            // menuBuildDebug
            // 
            this.menuBuildDebug.Name = "menuBuildDebug";
            this.menuBuildDebug.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.menuBuildDebug.Size = new System.Drawing.Size(299, 22);
            this.menuBuildDebug.Text = "Build Modpack (No Packing)";
            this.menuBuildDebug.Click += new System.EventHandler(this.menuBuildDebug_Click);
            // 
            // sendToSDToolStripMenuItem
            // 
            this.sendToSDToolStripMenuItem.Name = "sendToSDToolStripMenuItem";
            this.sendToSDToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.sendToSDToolStripMenuItem.Text = "Send to SD/USB";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(296, 6);
            // 
            // refreshTreeviewToolStripMenuItem
            // 
            this.refreshTreeviewToolStripMenuItem.Name = "refreshTreeviewToolStripMenuItem";
            this.refreshTreeviewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshTreeviewToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.refreshTreeviewToolStripMenuItem.Text = "Refresh Treeview";
            this.refreshTreeviewToolStripMenuItem.Click += new System.EventHandler(this.refreshTreeviewToolStripMenuItem_Click);
            // 
            // menuOptions
            // 
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.menuOptions.Size = new System.Drawing.Size(299, 22);
            this.menuOptions.Text = "Options";
            this.menuOptions.Click += new System.EventHandler(this.menuOptions_Click);
            // 
            // oMenuFileSeparator
            // 
            this.oMenuFileSeparator.Name = "oMenuFileSeparator";
            this.oMenuFileSeparator.Size = new System.Drawing.Size(296, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.menuExit.Size = new System.Drawing.Size(299, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // directoriesToolStripMenuItem
            // 
            this.directoriesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openGameDirectoryToolStripMenuItem,
            this.openSdDirectoryToolStripMenuItem,
            this.toolStripSeparator4,
            this.openExtractDirectoryToolStripMenuItem,
            this.openWorkspaceDirectoryToolStripMenuItem,
            this.openExportDirectoryToolStripMenuItem,
            this.toolStripSeparator6,
            this.openSm4shexplorerDirectoryToolStripMenuItem,
            this.openTempDirectoryToolStripMenuItem});
            this.directoriesToolStripMenuItem.Name = "directoriesToolStripMenuItem";
            this.directoriesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.directoriesToolStripMenuItem.Text = "Folders";
            // 
            // openGameDirectoryToolStripMenuItem
            // 
            this.openGameDirectoryToolStripMenuItem.Name = "openGameDirectoryToolStripMenuItem";
            this.openGameDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openGameDirectoryToolStripMenuItem.Text = "Dump";
            this.openGameDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openGameDirectoryToolStripMenuItem_Click);
            // 
            // openSdDirectoryToolStripMenuItem
            // 
            this.openSdDirectoryToolStripMenuItem.Name = "openSdDirectoryToolStripMenuItem";
            this.openSdDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openSdDirectoryToolStripMenuItem.Text = "SD Card";
            this.openSdDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openSdDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(174, 6);
            // 
            // openExtractDirectoryToolStripMenuItem
            // 
            this.openExtractDirectoryToolStripMenuItem.Name = "openExtractDirectoryToolStripMenuItem";
            this.openExtractDirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.openExtractDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openExtractDirectoryToolStripMenuItem.Text = "Extract";
            this.openExtractDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openExtractDirectoryToolStripMenuItem_Click);
            // 
            // openWorkspaceDirectoryToolStripMenuItem
            // 
            this.openWorkspaceDirectoryToolStripMenuItem.Name = "openWorkspaceDirectoryToolStripMenuItem";
            this.openWorkspaceDirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.openWorkspaceDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openWorkspaceDirectoryToolStripMenuItem.Text = "Workspace";
            this.openWorkspaceDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openWorkspaceDirectoryToolStripMenuItem_Click);
            // 
            // openExportDirectoryToolStripMenuItem
            // 
            this.openExportDirectoryToolStripMenuItem.Name = "openExportDirectoryToolStripMenuItem";
            this.openExportDirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.openExportDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openExportDirectoryToolStripMenuItem.Text = "Export";
            this.openExportDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openExportDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(174, 6);
            // 
            // openSm4shexplorerDirectoryToolStripMenuItem
            // 
            this.openSm4shexplorerDirectoryToolStripMenuItem.Name = "openSm4shexplorerDirectoryToolStripMenuItem";
            this.openSm4shexplorerDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openSm4shexplorerDirectoryToolStripMenuItem.Text = "Sm4shExplorer";
            this.openSm4shexplorerDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openSm4shexplorerDirectoryToolStripMenuItem_Click);
            // 
            // openTempDirectoryToolStripMenuItem
            // 
            this.openTempDirectoryToolStripMenuItem.Name = "openTempDirectoryToolStripMenuItem";
            this.openTempDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openTempDirectoryToolStripMenuItem.Text = "Temp";
            this.openTempDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openTempDirectoryToolStripMenuItem_Click);
            // 
            // menuPlugins
            // 
            this.menuPlugins.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.orderPluginsToolStripMenuItem});
            this.menuPlugins.Enabled = false;
            this.menuPlugins.Name = "menuPlugins";
            this.menuPlugins.Size = new System.Drawing.Size(58, 20);
            this.menuPlugins.Text = "Plugins";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(143, 6);
            // 
            // orderPluginsToolStripMenuItem
            // 
            this.orderPluginsToolStripMenuItem.Name = "orderPluginsToolStripMenuItem";
            this.orderPluginsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.orderPluginsToolStripMenuItem.Text = "Order Plugins";
            this.orderPluginsToolStripMenuItem.Click += new System.EventHandler(this.orderPluginsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutSm4shExplorerToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutSm4shExplorerToolStripMenuItem
            // 
            this.aboutSm4shExplorerToolStripMenuItem.Name = "aboutSm4shExplorerToolStripMenuItem";
            this.aboutSm4shExplorerToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.aboutSm4shExplorerToolStripMenuItem.Text = "About Sm4shExplorer";
            this.aboutSm4shExplorerToolStripMenuItem.Click += new System.EventHandler(this.thanksToolStripMenuItem_Click);
            // 
            // contextMenuTreeView
            // 
            this.contextMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.removeModToolStripMenuItem,
            this.toolStripSeparator1,
            this.unlocalizeToolStripMenuItem,
            this.removeUnlocalizeToolStripMenuItem,
            this.toolStripSeparator2,
            this.removeResourceToolStripMenuItem,
            this.reintroduceResourceToolStripMenuItem,
            this.toolStripSeparator5,
            this.packThisFolderToolStripMenuItem,
            this.doNotPackThisFolderToolStripMenuItem});
            this.contextMenuTreeView.Name = "contextMenuTreeView";
            this.contextMenuTreeView.Size = new System.Drawing.Size(195, 198);
            this.contextMenuTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTreeView_Opening);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // removeModToolStripMenuItem
            // 
            this.removeModToolStripMenuItem.Name = "removeModToolStripMenuItem";
            this.removeModToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.removeModToolStripMenuItem.Text = "Remove mod files";
            this.removeModToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // unlocalizeToolStripMenuItem
            // 
            this.unlocalizeToolStripMenuItem.Name = "unlocalizeToolStripMenuItem";
            this.unlocalizeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.unlocalizeToolStripMenuItem.Text = "Unlocalize";
            this.unlocalizeToolStripMenuItem.Click += new System.EventHandler(this.unlocalizeToolStripMenuItem_Click);
            // 
            // removeUnlocalizeToolStripMenuItem
            // 
            this.removeUnlocalizeToolStripMenuItem.Name = "removeUnlocalizeToolStripMenuItem";
            this.removeUnlocalizeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.removeUnlocalizeToolStripMenuItem.Text = "Remove unlocalize";
            this.removeUnlocalizeToolStripMenuItem.Click += new System.EventHandler(this.removeUnlocalizeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(191, 6);
            // 
            // removeResourceToolStripMenuItem
            // 
            this.removeResourceToolStripMenuItem.Name = "removeResourceToolStripMenuItem";
            this.removeResourceToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.removeResourceToolStripMenuItem.Text = "Remove resource";
            this.removeResourceToolStripMenuItem.Click += new System.EventHandler(this.removeResourceToolStripMenuItem_Click);
            // 
            // reintroduceResourceToolStripMenuItem
            // 
            this.reintroduceResourceToolStripMenuItem.Name = "reintroduceResourceToolStripMenuItem";
            this.reintroduceResourceToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.reintroduceResourceToolStripMenuItem.Text = "Reintroduce resource";
            this.reintroduceResourceToolStripMenuItem.Click += new System.EventHandler(this.reintroduceResourceToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(191, 6);
            // 
            // packThisFolderToolStripMenuItem
            // 
            this.packThisFolderToolStripMenuItem.Name = "packThisFolderToolStripMenuItem";
            this.packThisFolderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.packThisFolderToolStripMenuItem.Text = "Pack this folder";
            this.packThisFolderToolStripMenuItem.Click += new System.EventHandler(this.packThisFolderToolStripMenuItem_Click);
            // 
            // doNotPackThisFolderToolStripMenuItem
            // 
            this.doNotPackThisFolderToolStripMenuItem.Name = "doNotPackThisFolderToolStripMenuItem";
            this.doNotPackThisFolderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.doNotPackThisFolderToolStripMenuItem.Text = "Do not pack this folder";
            this.doNotPackThisFolderToolStripMenuItem.Click += new System.EventHandler(this.doNotPackThisFolderToolStripMenuItem_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select the game folder";
            // 
            // textConsole
            // 
            this.textConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textConsole.Location = new System.Drawing.Point(0, 0);
            this.textConsole.Multiline = true;
            this.textConsole.Name = "textConsole";
            this.textConsole.ReadOnly = true;
            this.textConsole.Size = new System.Drawing.Size(884, 178);
            this.textConsole.TabIndex = 0;
            // 
            // oPanelLeft
            // 
            this.oPanelLeft.Controls.Add(this.oPanelFileInformation);
            this.oPanelLeft.Controls.Add(this.oPanelInformation);
            this.oPanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.oPanelLeft.Location = new System.Drawing.Point(0, 0);
            this.oPanelLeft.Name = "oPanelLeft";
            this.oPanelLeft.Size = new System.Drawing.Size(213, 455);
            this.oPanelLeft.TabIndex = 1;
            // 
            // oPanelFileInformation
            // 
            this.oPanelFileInformation.Controls.Add(this.dataGridView);
            this.oPanelFileInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oPanelFileInformation.Location = new System.Drawing.Point(0, 130);
            this.oPanelFileInformation.Name = "oPanelFileInformation";
            this.oPanelFileInformation.Size = new System.Drawing.Size(213, 325);
            this.oPanelFileInformation.TabIndex = 1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridViewName,
            this.gridViewValue});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(213, 325);
            this.dataGridView.TabIndex = 0;
            // 
            // gridViewName
            // 
            this.gridViewName.HeaderText = "Name";
            this.gridViewName.Name = "gridViewName";
            this.gridViewName.ReadOnly = true;
            // 
            // gridViewValue
            // 
            this.gridViewValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.gridViewValue.HeaderText = "Value";
            this.gridViewValue.Name = "gridViewValue";
            this.gridViewValue.ReadOnly = true;
            // 
            // oPanelInformation
            // 
            this.oPanelInformation.Controls.Add(this.groupInfo);
            this.oPanelInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.oPanelInformation.Location = new System.Drawing.Point(0, 0);
            this.oPanelInformation.Name = "oPanelInformation";
            this.oPanelInformation.Size = new System.Drawing.Size(213, 130);
            this.oPanelInformation.TabIndex = 0;
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.lblRegionValue);
            this.groupInfo.Controls.Add(this.lblVersionValue);
            this.groupInfo.Controls.Add(this.lblGameIDValue);
            this.groupInfo.Controls.Add(this.lblRegion);
            this.groupInfo.Controls.Add(this.lblVersion);
            this.groupInfo.Controls.Add(this.lblGameID);
            this.groupInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupInfo.Location = new System.Drawing.Point(0, 0);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(213, 130);
            this.groupInfo.TabIndex = 0;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "Game info";
            // 
            // lblRegionValue
            // 
            this.lblRegionValue.AutoSize = true;
            this.lblRegionValue.Location = new System.Drawing.Point(76, 92);
            this.lblRegionValue.Name = "lblRegionValue";
            this.lblRegionValue.Size = new System.Drawing.Size(0, 13);
            this.lblRegionValue.TabIndex = 5;
            // 
            // lblVersionValue
            // 
            this.lblVersionValue.AutoSize = true;
            this.lblVersionValue.Location = new System.Drawing.Point(76, 61);
            this.lblVersionValue.Name = "lblVersionValue";
            this.lblVersionValue.Size = new System.Drawing.Size(0, 13);
            this.lblVersionValue.TabIndex = 4;
            // 
            // lblGameIDValue
            // 
            this.lblGameIDValue.AutoSize = true;
            this.lblGameIDValue.Location = new System.Drawing.Point(76, 30);
            this.lblGameIDValue.Name = "lblGameIDValue";
            this.lblGameIDValue.Size = new System.Drawing.Size(0, 13);
            this.lblGameIDValue.TabIndex = 3;
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(12, 92);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(50, 13);
            this.lblRegion.TabIndex = 2;
            this.lblRegion.Text = "Region : ";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(12, 61);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(51, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version : ";
            // 
            // lblGameID
            // 
            this.lblGameID.AutoSize = true;
            this.lblGameID.Location = new System.Drawing.Point(12, 30);
            this.lblGameID.Name = "lblGameID";
            this.lblGameID.Size = new System.Drawing.Size(58, 13);
            this.lblGameID.TabIndex = 0;
            this.lblGameID.Text = "Game ID : ";
            // 
            // oTreeViewPanel
            // 
            this.oTreeViewPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.oTreeViewPanel.Controls.Add(this.treeView);
            this.oTreeViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oTreeViewPanel.Location = new System.Drawing.Point(213, 0);
            this.oTreeViewPanel.Name = "oTreeViewPanel";
            this.oTreeViewPanel.Size = new System.Drawing.Size(671, 455);
            this.oTreeViewPanel.TabIndex = 2;
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.ContextMenuStrip = this.contextMenuTreeView;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(671, 455);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.oTreeViewPanel);
            this.splitContainer.Panel1.Controls.Add(this.oPanelLeft);
            this.splitContainer.Panel1MinSize = 300;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.textConsole);
            this.splitContainer.Panel2MinSize = 100;
            this.splitContainer.Size = new System.Drawing.Size(884, 637);
            this.splitContainer.SplitterDistance = 455;
            this.splitContainer.TabIndex = 3;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.Text = "Sm4shExplorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.contextMenuTreeView.ResumeLayout(false);
            this.oPanelLeft.ResumeLayout(false);
            this.oPanelFileInformation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.oPanelInformation.ResumeLayout(false);
            this.groupInfo.ResumeLayout(false);
            this.groupInfo.PerformLayout();
            this.oTreeViewPanel.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuProject;
        private System.Windows.Forms.ToolStripSeparator oMenuFileSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem menuBuild;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeView;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.TextBox textConsole;
        private System.Windows.Forms.Panel oPanelLeft;
        private System.Windows.Forms.Panel oPanelFileInformation;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel oPanelInformation;
        private System.Windows.Forms.GroupBox groupInfo;
        private System.Windows.Forms.Panel oTreeViewPanel;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem removeModToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem unlocalizeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridViewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridViewValue;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblGameID;
        private System.Windows.Forms.Label lblRegionValue;
        private System.Windows.Forms.Label lblVersionValue;
        private System.Windows.Forms.Label lblGameIDValue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem packThisFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuPlugins;
        private System.Windows.Forms.ToolStripMenuItem aboutSm4shExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeUnlocalizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuBuildDebug;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshTreeviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openExtractDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkspaceDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openExportDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSm4shexplorerDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTempDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reintroduceResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem openGameDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem orderPluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToSDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSdDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doNotPackThisFolderToolStripMenuItem;
    }
}

