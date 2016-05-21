namespace Sm4shMusic.Forms
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBGMManagement = new System.Windows.Forms.TabPage();
            this.tabStage = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileTheModificationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loadConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshBGMFilesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitWillCancelAllChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateCSVForSoundDBAndMSBTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateCSVForBGMEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateCSVForMyMusicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.listAllOrphanBGMsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thanksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBGMManagement);
            this.tabControl.Controls.Add(this.tabStage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(884, 637);
            this.tabControl.TabIndex = 0;
            // 
            // tabBGMManagement
            // 
            this.tabBGMManagement.Location = new System.Drawing.Point(4, 22);
            this.tabBGMManagement.Name = "tabBGMManagement";
            this.tabBGMManagement.Size = new System.Drawing.Size(876, 611);
            this.tabBGMManagement.TabIndex = 2;
            this.tabBGMManagement.Text = "BGM Management";
            this.tabBGMManagement.UseVisualStyleBackColor = true;
            // 
            // tabStage
            // 
            this.tabStage.Location = new System.Drawing.Point(4, 22);
            this.tabStage.Name = "tabStage";
            this.tabStage.Size = new System.Drawing.Size(876, 611);
            this.tabStage.TabIndex = 1;
            this.tabStage.Text = "My Music";
            this.tabStage.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileTheModificationsToolStripMenuItem,
            this.toolStripSeparator1,
            this.loadConfigurationToolStripMenuItem,
            this.saveConfigurationToolStripMenuItem,
            this.toolStripSeparator4,
            this.refreshBGMFilesListToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitWillCancelAllChangesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // compileTheModificationsToolStripMenuItem
            // 
            this.compileTheModificationsToolStripMenuItem.Name = "compileTheModificationsToolStripMenuItem";
            this.compileTheModificationsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.compileTheModificationsToolStripMenuItem.Text = "Compile the modifications";
            this.compileTheModificationsToolStripMenuItem.Click += new System.EventHandler(this.compileTheModificationsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // loadConfigurationToolStripMenuItem
            // 
            this.loadConfigurationToolStripMenuItem.Name = "loadConfigurationToolStripMenuItem";
            this.loadConfigurationToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.loadConfigurationToolStripMenuItem.Text = "Load configuration";
            this.loadConfigurationToolStripMenuItem.Click += new System.EventHandler(this.loadConfigurationToolStripMenuItem_Click);
            // 
            // saveConfigurationToolStripMenuItem
            // 
            this.saveConfigurationToolStripMenuItem.Name = "saveConfigurationToolStripMenuItem";
            this.saveConfigurationToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.saveConfigurationToolStripMenuItem.Text = "Save configuration";
            this.saveConfigurationToolStripMenuItem.Click += new System.EventHandler(this.saveConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(219, 6);
            // 
            // refreshBGMFilesListToolStripMenuItem
            // 
            this.refreshBGMFilesListToolStripMenuItem.Name = "refreshBGMFilesListToolStripMenuItem";
            this.refreshBGMFilesListToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.refreshBGMFilesListToolStripMenuItem.Text = "Refresh BGM Files list";
            this.refreshBGMFilesListToolStripMenuItem.Click += new System.EventHandler(this.refreshBGMFilesListToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // exitWillCancelAllChangesToolStripMenuItem
            // 
            this.exitWillCancelAllChangesToolStripMenuItem.Name = "exitWillCancelAllChangesToolStripMenuItem";
            this.exitWillCancelAllChangesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.exitWillCancelAllChangesToolStripMenuItem.Text = "Exit (Will cancel all changes)";
            this.exitWillCancelAllChangesToolStripMenuItem.Click += new System.EventHandler(this.exitWillCancelAllChangesToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateCSVForSoundDBAndMSBTToolStripMenuItem,
            this.generateCSVForBGMEntriesToolStripMenuItem,
            this.generateCSVForMyMusicToolStripMenuItem,
            this.toolStripSeparator3,
            this.listAllOrphanBGMsToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // generateCSVForSoundDBAndMSBTToolStripMenuItem
            // 
            this.generateCSVForSoundDBAndMSBTToolStripMenuItem.Name = "generateCSVForSoundDBAndMSBTToolStripMenuItem";
            this.generateCSVForSoundDBAndMSBTToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.generateCSVForSoundDBAndMSBTToolStripMenuItem.Text = "Generate CSV for SoundDB and MSBT";
            this.generateCSVForSoundDBAndMSBTToolStripMenuItem.Click += new System.EventHandler(this.generateCSVForSoundDBAndMSBTToolStripMenuItem_Click);
            // 
            // generateCSVForBGMEntriesToolStripMenuItem
            // 
            this.generateCSVForBGMEntriesToolStripMenuItem.Name = "generateCSVForBGMEntriesToolStripMenuItem";
            this.generateCSVForBGMEntriesToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.generateCSVForBGMEntriesToolStripMenuItem.Text = "Generate CSV for BGM entries";
            this.generateCSVForBGMEntriesToolStripMenuItem.Click += new System.EventHandler(this.generateCSVForBGMEntriesToolStripMenuItem_Click);
            // 
            // generateCSVForMyMusicToolStripMenuItem
            // 
            this.generateCSVForMyMusicToolStripMenuItem.Name = "generateCSVForMyMusicToolStripMenuItem";
            this.generateCSVForMyMusicToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.generateCSVForMyMusicToolStripMenuItem.Text = "Generate CSV for MyMusic";
            this.generateCSVForMyMusicToolStripMenuItem.Click += new System.EventHandler(this.generateCSVForMyMusicToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(269, 6);
            // 
            // listAllOrphanBGMsToolStripMenuItem
            // 
            this.listAllOrphanBGMsToolStripMenuItem.Name = "listAllOrphanBGMsToolStripMenuItem";
            this.listAllOrphanBGMsToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.listAllOrphanBGMsToolStripMenuItem.Text = "List all orphan BGMs";
            this.listAllOrphanBGMsToolStripMenuItem.Click += new System.EventHandler(this.listAllOrphanBGMsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thanksToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // thanksToolStripMenuItem
            // 
            this.thanksToolStripMenuItem.Name = "thanksToolStripMenuItem";
            this.thanksToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.thanksToolStripMenuItem.Text = "Thanks";
            this.thanksToolStripMenuItem.Click += new System.EventHandler(this.thanksToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // Sm4shMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sm4shMusic";
            this.Text = "Sm4shMusic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabStage;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileTheModificationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitWillCancelAllChangesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateCSVForSoundDBAndMSBTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateCSVForBGMEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateCSVForMyMusicToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thanksToolStripMenuItem;
        private System.Windows.Forms.TabPage tabBGMManagement;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem listAllOrphanBGMsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem refreshBGMFilesListToolStripMenuItem;
    }
}