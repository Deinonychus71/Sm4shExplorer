namespace Sm4shFileExplorer
{
    partial class Options
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
            this.groupDirectories = new System.Windows.Forms.GroupBox();
            this.lblDirExportFolderDesc = new System.Windows.Forms.Label();
            this.lblDirWorkplaceFolderDesc = new System.Windows.Forms.Label();
            this.lblDirExtractionFolderDesc = new System.Windows.Forms.Label();
            this.lblDirTempFolderDesc = new System.Windows.Forms.Label();
            this.btnDirExportFolder = new System.Windows.Forms.Button();
            this.txtDirExportFolder = new System.Windows.Forms.TextBox();
            this.lblDirExportFolder = new System.Windows.Forms.Label();
            this.btnDirWorkplaceFolder = new System.Windows.Forms.Button();
            this.txtDirWorkplaceFolder = new System.Windows.Forms.TextBox();
            this.btnDirExtractionFolder = new System.Windows.Forms.Button();
            this.txtDirExtractionFolder = new System.Windows.Forms.TextBox();
            this.btnDirTempFolder = new System.Windows.Forms.Button();
            this.txtDirTempFolder = new System.Windows.Forms.TextBox();
            this.lblDirWorkplaceFolder = new System.Windows.Forms.Label();
            this.lblDirExtractionFolder = new System.Windows.Forms.Label();
            this.lblDirTempFolder = new System.Windows.Forms.Label();
            this.groupTools = new System.Windows.Forms.GroupBox();
            this.btnDirHexEditor = new System.Windows.Forms.Button();
            this.lblDirHexEditor = new System.Windows.Forms.Label();
            this.txtDirHexEditor = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupDebug = new System.Windows.Forms.GroupBox();
            this.lblForceOriginalFlagsDesc = new System.Windows.Forms.Label();
            this.chkForceOriginalFlags = new System.Windows.Forms.CheckBox();
            this.lblForceOriginalFlags = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.lblDebug = new System.Windows.Forms.Label();
            this.lblSkipJunkEntriesDesc = new System.Windows.Forms.Label();
            this.chkSkipJunkEntries = new System.Windows.Forms.CheckBox();
            this.lblSkipJunkEntries = new System.Windows.Forms.Label();
            this.lblSeeExportResults = new System.Windows.Forms.Label();
            this.chkSeeExportResults = new System.Windows.Forms.CheckBox();
            this.lblSeeExportResultsDesc = new System.Windows.Forms.Label();
            this.groupCSVExport = new System.Windows.Forms.GroupBox();
            this.lblSeeExportResultsIgnoreDesc = new System.Windows.Forms.Label();
            this.chkCSVExportIgnoreOffsetInPack = new System.Windows.Forms.CheckBox();
            this.chkCSVExportIgnoreCompSize = new System.Windows.Forms.CheckBox();
            this.chkCSVExportIgnoreFlags = new System.Windows.Forms.CheckBox();
            this.lblExportAddDate = new System.Windows.Forms.Label();
            this.chkExportAddDate = new System.Windows.Forms.CheckBox();
            this.lblExportAddDateDesc = new System.Windows.Forms.Label();
            this.groupDirectories.SuspendLayout();
            this.groupTools.SuspendLayout();
            this.groupDebug.SuspendLayout();
            this.groupCSVExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupDirectories
            // 
            this.groupDirectories.Controls.Add(this.lblExportAddDateDesc);
            this.groupDirectories.Controls.Add(this.chkExportAddDate);
            this.groupDirectories.Controls.Add(this.lblExportAddDate);
            this.groupDirectories.Controls.Add(this.lblDirExportFolderDesc);
            this.groupDirectories.Controls.Add(this.lblDirWorkplaceFolderDesc);
            this.groupDirectories.Controls.Add(this.lblDirExtractionFolderDesc);
            this.groupDirectories.Controls.Add(this.lblDirTempFolderDesc);
            this.groupDirectories.Controls.Add(this.btnDirExportFolder);
            this.groupDirectories.Controls.Add(this.txtDirExportFolder);
            this.groupDirectories.Controls.Add(this.lblDirExportFolder);
            this.groupDirectories.Controls.Add(this.btnDirWorkplaceFolder);
            this.groupDirectories.Controls.Add(this.txtDirWorkplaceFolder);
            this.groupDirectories.Controls.Add(this.btnDirExtractionFolder);
            this.groupDirectories.Controls.Add(this.txtDirExtractionFolder);
            this.groupDirectories.Controls.Add(this.btnDirTempFolder);
            this.groupDirectories.Controls.Add(this.txtDirTempFolder);
            this.groupDirectories.Controls.Add(this.lblDirWorkplaceFolder);
            this.groupDirectories.Controls.Add(this.lblDirExtractionFolder);
            this.groupDirectories.Controls.Add(this.lblDirTempFolder);
            this.groupDirectories.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDirectories.Location = new System.Drawing.Point(0, 0);
            this.groupDirectories.Name = "groupDirectories";
            this.groupDirectories.Size = new System.Drawing.Size(538, 180);
            this.groupDirectories.TabIndex = 0;
            this.groupDirectories.TabStop = false;
            this.groupDirectories.Text = "Directories";
            // 
            // lblDirExportFolderDesc
            // 
            this.lblDirExportFolderDesc.AutoSize = true;
            this.lblDirExportFolderDesc.Location = new System.Drawing.Point(348, 120);
            this.lblDirExportFolderDesc.Name = "lblDirExportFolderDesc";
            this.lblDirExportFolderDesc.Size = new System.Drawing.Size(180, 13);
            this.lblDirExportFolderDesc.TabIndex = 15;
            this.lblDirExportFolderDesc.Text = "Directory of the files after packaging.";
            // 
            // lblDirWorkplaceFolderDesc
            // 
            this.lblDirWorkplaceFolderDesc.AutoSize = true;
            this.lblDirWorkplaceFolderDesc.Location = new System.Drawing.Point(348, 90);
            this.lblDirWorkplaceFolderDesc.Name = "lblDirWorkplaceFolderDesc";
            this.lblDirWorkplaceFolderDesc.Size = new System.Drawing.Size(161, 13);
            this.lblDirWorkplaceFolderDesc.TabIndex = 14;
            this.lblDirWorkplaceFolderDesc.Text = "Directory of the files to be rebuilt.";
            // 
            // lblDirExtractionFolderDesc
            // 
            this.lblDirExtractionFolderDesc.AutoSize = true;
            this.lblDirExtractionFolderDesc.Location = new System.Drawing.Point(348, 60);
            this.lblDirExtractionFolderDesc.Name = "lblDirExtractionFolderDesc";
            this.lblDirExtractionFolderDesc.Size = new System.Drawing.Size(135, 13);
            this.lblDirExtractionFolderDesc.TabIndex = 13;
            this.lblDirExtractionFolderDesc.Text = "Directory for extracted files.";
            // 
            // lblDirTempFolderDesc
            // 
            this.lblDirTempFolderDesc.AutoSize = true;
            this.lblDirTempFolderDesc.Location = new System.Drawing.Point(348, 30);
            this.lblDirTempFolderDesc.Name = "lblDirTempFolderDesc";
            this.lblDirTempFolderDesc.Size = new System.Drawing.Size(146, 13);
            this.lblDirTempFolderDesc.TabIndex = 12;
            this.lblDirTempFolderDesc.Text = "Used for packing/unpacking.";
            // 
            // btnDirExportFolder
            // 
            this.btnDirExportFolder.Location = new System.Drawing.Point(279, 115);
            this.btnDirExportFolder.Name = "btnDirExportFolder";
            this.btnDirExportFolder.Size = new System.Drawing.Size(63, 23);
            this.btnDirExportFolder.TabIndex = 11;
            this.btnDirExportFolder.Text = "Browse";
            this.btnDirExportFolder.UseVisualStyleBackColor = true;
            this.btnDirExportFolder.Click += new System.EventHandler(this.btnDirExportFolder_Click);
            // 
            // txtDirExportFolder
            // 
            this.txtDirExportFolder.Location = new System.Drawing.Point(123, 117);
            this.txtDirExportFolder.Name = "txtDirExportFolder";
            this.txtDirExportFolder.Size = new System.Drawing.Size(150, 20);
            this.txtDirExportFolder.TabIndex = 10;
            // 
            // lblDirExportFolder
            // 
            this.lblDirExportFolder.AutoSize = true;
            this.lblDirExportFolder.Location = new System.Drawing.Point(12, 120);
            this.lblDirExportFolder.Name = "lblDirExportFolder";
            this.lblDirExportFolder.Size = new System.Drawing.Size(75, 13);
            this.lblDirExportFolder.TabIndex = 9;
            this.lblDirExportFolder.Text = "Export Folder :";
            // 
            // btnDirWorkplaceFolder
            // 
            this.btnDirWorkplaceFolder.Location = new System.Drawing.Point(279, 85);
            this.btnDirWorkplaceFolder.Name = "btnDirWorkplaceFolder";
            this.btnDirWorkplaceFolder.Size = new System.Drawing.Size(63, 23);
            this.btnDirWorkplaceFolder.TabIndex = 8;
            this.btnDirWorkplaceFolder.Text = "Browse";
            this.btnDirWorkplaceFolder.UseVisualStyleBackColor = true;
            this.btnDirWorkplaceFolder.Click += new System.EventHandler(this.btnDirWorkplaceFolder_Click);
            // 
            // txtDirWorkplaceFolder
            // 
            this.txtDirWorkplaceFolder.Location = new System.Drawing.Point(123, 87);
            this.txtDirWorkplaceFolder.Name = "txtDirWorkplaceFolder";
            this.txtDirWorkplaceFolder.Size = new System.Drawing.Size(150, 20);
            this.txtDirWorkplaceFolder.TabIndex = 7;
            // 
            // btnDirExtractionFolder
            // 
            this.btnDirExtractionFolder.Location = new System.Drawing.Point(279, 55);
            this.btnDirExtractionFolder.Name = "btnDirExtractionFolder";
            this.btnDirExtractionFolder.Size = new System.Drawing.Size(63, 23);
            this.btnDirExtractionFolder.TabIndex = 6;
            this.btnDirExtractionFolder.Text = "Browse";
            this.btnDirExtractionFolder.UseVisualStyleBackColor = true;
            this.btnDirExtractionFolder.Click += new System.EventHandler(this.btnDirExtractionFolder_Click);
            // 
            // txtDirExtractionFolder
            // 
            this.txtDirExtractionFolder.Location = new System.Drawing.Point(123, 57);
            this.txtDirExtractionFolder.Name = "txtDirExtractionFolder";
            this.txtDirExtractionFolder.Size = new System.Drawing.Size(150, 20);
            this.txtDirExtractionFolder.TabIndex = 5;
            // 
            // btnDirTempFolder
            // 
            this.btnDirTempFolder.Location = new System.Drawing.Point(279, 25);
            this.btnDirTempFolder.Name = "btnDirTempFolder";
            this.btnDirTempFolder.Size = new System.Drawing.Size(63, 23);
            this.btnDirTempFolder.TabIndex = 4;
            this.btnDirTempFolder.Text = "Browse";
            this.btnDirTempFolder.UseVisualStyleBackColor = true;
            this.btnDirTempFolder.Click += new System.EventHandler(this.btnDirTempFolder_Click);
            // 
            // txtDirTempFolder
            // 
            this.txtDirTempFolder.Location = new System.Drawing.Point(123, 27);
            this.txtDirTempFolder.Name = "txtDirTempFolder";
            this.txtDirTempFolder.Size = new System.Drawing.Size(150, 20);
            this.txtDirTempFolder.TabIndex = 3;
            // 
            // lblDirWorkplaceFolder
            // 
            this.lblDirWorkplaceFolder.AutoSize = true;
            this.lblDirWorkplaceFolder.Location = new System.Drawing.Point(12, 90);
            this.lblDirWorkplaceFolder.Name = "lblDirWorkplaceFolder";
            this.lblDirWorkplaceFolder.Size = new System.Drawing.Size(97, 13);
            this.lblDirWorkplaceFolder.TabIndex = 2;
            this.lblDirWorkplaceFolder.Text = "Workplace Folder :";
            // 
            // lblDirExtractionFolder
            // 
            this.lblDirExtractionFolder.AutoSize = true;
            this.lblDirExtractionFolder.Location = new System.Drawing.Point(12, 60);
            this.lblDirExtractionFolder.Name = "lblDirExtractionFolder";
            this.lblDirExtractionFolder.Size = new System.Drawing.Size(92, 13);
            this.lblDirExtractionFolder.TabIndex = 1;
            this.lblDirExtractionFolder.Text = "Extraction Folder :";
            // 
            // lblDirTempFolder
            // 
            this.lblDirTempFolder.AutoSize = true;
            this.lblDirTempFolder.Location = new System.Drawing.Point(12, 30);
            this.lblDirTempFolder.Name = "lblDirTempFolder";
            this.lblDirTempFolder.Size = new System.Drawing.Size(72, 13);
            this.lblDirTempFolder.TabIndex = 0;
            this.lblDirTempFolder.Text = "Temp Folder :";
            // 
            // groupTools
            // 
            this.groupTools.Controls.Add(this.btnDirHexEditor);
            this.groupTools.Controls.Add(this.lblDirHexEditor);
            this.groupTools.Controls.Add(this.txtDirHexEditor);
            this.groupTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupTools.Location = new System.Drawing.Point(0, 180);
            this.groupTools.Name = "groupTools";
            this.groupTools.Size = new System.Drawing.Size(538, 60);
            this.groupTools.TabIndex = 3;
            this.groupTools.TabStop = false;
            this.groupTools.Text = "Tools";
            // 
            // btnDirHexEditor
            // 
            this.btnDirHexEditor.Location = new System.Drawing.Point(469, 25);
            this.btnDirHexEditor.Name = "btnDirHexEditor";
            this.btnDirHexEditor.Size = new System.Drawing.Size(63, 23);
            this.btnDirHexEditor.TabIndex = 10;
            this.btnDirHexEditor.Text = "Browse";
            this.btnDirHexEditor.UseVisualStyleBackColor = true;
            this.btnDirHexEditor.Click += new System.EventHandler(this.btnDirHexEditor_Click);
            // 
            // lblDirHexEditor
            // 
            this.lblDirHexEditor.AutoSize = true;
            this.lblDirHexEditor.Location = new System.Drawing.Point(12, 30);
            this.lblDirHexEditor.Name = "lblDirHexEditor";
            this.lblDirHexEditor.Size = new System.Drawing.Size(62, 13);
            this.lblDirHexEditor.TabIndex = 0;
            this.lblDirHexEditor.Text = "Hex Editor :";
            // 
            // txtDirHexEditor
            // 
            this.txtDirHexEditor.Location = new System.Drawing.Point(123, 27);
            this.txtDirHexEditor.Name = "txtDirHexEditor";
            this.txtDirHexEditor.Size = new System.Drawing.Size(340, 20);
            this.txtDirHexEditor.TabIndex = 9;
            // 
            // groupDebug
            // 
            this.groupDebug.Controls.Add(this.lblForceOriginalFlagsDesc);
            this.groupDebug.Controls.Add(this.chkForceOriginalFlags);
            this.groupDebug.Controls.Add(this.lblForceOriginalFlags);
            this.groupDebug.Controls.Add(this.label1);
            this.groupDebug.Controls.Add(this.chkDebug);
            this.groupDebug.Controls.Add(this.lblDebug);
            this.groupDebug.Controls.Add(this.lblSkipJunkEntriesDesc);
            this.groupDebug.Controls.Add(this.chkSkipJunkEntries);
            this.groupDebug.Controls.Add(this.lblSkipJunkEntries);
            this.groupDebug.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDebug.Location = new System.Drawing.Point(0, 240);
            this.groupDebug.Name = "groupDebug";
            this.groupDebug.Size = new System.Drawing.Size(538, 120);
            this.groupDebug.TabIndex = 4;
            this.groupDebug.TabStop = false;
            this.groupDebug.Text = "Debug";
            // 
            // lblForceOriginalFlagsDesc
            // 
            this.lblForceOriginalFlagsDesc.AutoSize = true;
            this.lblForceOriginalFlagsDesc.Location = new System.Drawing.Point(146, 90);
            this.lblForceOriginalFlagsDesc.Name = "lblForceOriginalFlagsDesc";
            this.lblForceOriginalFlagsDesc.Size = new System.Drawing.Size(322, 13);
            this.lblForceOriginalFlagsDesc.TabIndex = 19;
            this.lblForceOriginalFlagsDesc.Text = "Keep original flags for existing entries. Will break unlocalize feature.";
            // 
            // chkForceOriginalFlags
            // 
            this.chkForceOriginalFlags.AutoSize = true;
            this.chkForceOriginalFlags.Location = new System.Drawing.Point(123, 90);
            this.chkForceOriginalFlags.Name = "chkForceOriginalFlags";
            this.chkForceOriginalFlags.Size = new System.Drawing.Size(15, 14);
            this.chkForceOriginalFlags.TabIndex = 18;
            this.chkForceOriginalFlags.UseVisualStyleBackColor = true;
            // 
            // lblForceOriginalFlags
            // 
            this.lblForceOriginalFlags.AutoSize = true;
            this.lblForceOriginalFlags.Location = new System.Drawing.Point(12, 90);
            this.lblForceOriginalFlags.Name = "lblForceOriginalFlags";
            this.lblForceOriginalFlags.Size = new System.Drawing.Size(101, 13);
            this.lblForceOriginalFlags.TabIndex = 17;
            this.lblForceOriginalFlags.Text = "Force original flags :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(379, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "More output information to the console, keep .dec versions of rebuilt resources.";
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Location = new System.Drawing.Point(123, 29);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(15, 14);
            this.chkDebug.TabIndex = 15;
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(12, 29);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(45, 13);
            this.lblDebug.TabIndex = 14;
            this.lblDebug.Text = "Debug :";
            // 
            // lblSkipJunkEntriesDesc
            // 
            this.lblSkipJunkEntriesDesc.AutoSize = true;
            this.lblSkipJunkEntriesDesc.Location = new System.Drawing.Point(146, 60);
            this.lblSkipJunkEntriesDesc.Name = "lblSkipJunkEntriesDesc";
            this.lblSkipJunkEntriesDesc.Size = new System.Drawing.Size(339, 13);
            this.lblSkipJunkEntriesDesc.TabIndex = 13;
            this.lblSkipJunkEntriesDesc.Text = "Skip empty folders and files without extension while reading the RF file.";
            // 
            // chkSkipJunkEntries
            // 
            this.chkSkipJunkEntries.AutoSize = true;
            this.chkSkipJunkEntries.Location = new System.Drawing.Point(123, 60);
            this.chkSkipJunkEntries.Name = "chkSkipJunkEntries";
            this.chkSkipJunkEntries.Size = new System.Drawing.Size(15, 14);
            this.chkSkipJunkEntries.TabIndex = 12;
            this.chkSkipJunkEntries.UseVisualStyleBackColor = true;
            // 
            // lblSkipJunkEntries
            // 
            this.lblSkipJunkEntries.AutoSize = true;
            this.lblSkipJunkEntries.Location = new System.Drawing.Point(12, 60);
            this.lblSkipJunkEntries.Name = "lblSkipJunkEntries";
            this.lblSkipJunkEntries.Size = new System.Drawing.Size(91, 13);
            this.lblSkipJunkEntries.TabIndex = 11;
            this.lblSkipJunkEntries.Text = "Skip junk entries :";
            // 
            // lblSeeExportResults
            // 
            this.lblSeeExportResults.AutoSize = true;
            this.lblSeeExportResults.Location = new System.Drawing.Point(12, 30);
            this.lblSeeExportResults.Name = "lblSeeExportResults";
            this.lblSeeExportResults.Size = new System.Drawing.Size(105, 13);
            this.lblSeeExportResults.TabIndex = 20;
            this.lblSeeExportResults.Text = "Export Results CSV :";
            // 
            // chkSeeExportResults
            // 
            this.chkSeeExportResults.AutoSize = true;
            this.chkSeeExportResults.Location = new System.Drawing.Point(123, 28);
            this.chkSeeExportResults.Name = "chkSeeExportResults";
            this.chkSeeExportResults.Size = new System.Drawing.Size(15, 14);
            this.chkSeeExportResults.TabIndex = 21;
            this.chkSeeExportResults.UseVisualStyleBackColor = true;
            // 
            // lblSeeExportResultsDesc
            // 
            this.lblSeeExportResultsDesc.AutoSize = true;
            this.lblSeeExportResultsDesc.Location = new System.Drawing.Point(146, 28);
            this.lblSeeExportResultsDesc.Name = "lblSeeExportResultsDesc";
            this.lblSeeExportResultsDesc.Size = new System.Drawing.Size(323, 13);
            this.lblSeeExportResultsDesc.TabIndex = 22;
            this.lblSeeExportResultsDesc.Text = "Will export a CSV of all the modified entries after a sucessful export.";
            // 
            // groupCSVExport
            // 
            this.groupCSVExport.Controls.Add(this.lblSeeExportResultsIgnoreDesc);
            this.groupCSVExport.Controls.Add(this.chkCSVExportIgnoreOffsetInPack);
            this.groupCSVExport.Controls.Add(this.chkCSVExportIgnoreCompSize);
            this.groupCSVExport.Controls.Add(this.chkCSVExportIgnoreFlags);
            this.groupCSVExport.Controls.Add(this.lblSeeExportResultsDesc);
            this.groupCSVExport.Controls.Add(this.lblSeeExportResults);
            this.groupCSVExport.Controls.Add(this.chkSeeExportResults);
            this.groupCSVExport.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupCSVExport.Location = new System.Drawing.Point(0, 360);
            this.groupCSVExport.Name = "groupCSVExport";
            this.groupCSVExport.Size = new System.Drawing.Size(538, 115);
            this.groupCSVExport.TabIndex = 5;
            this.groupCSVExport.TabStop = false;
            this.groupCSVExport.Text = "CSV Export";
            // 
            // lblSeeExportResultsIgnoreDesc
            // 
            this.lblSeeExportResultsIgnoreDesc.AutoSize = true;
            this.lblSeeExportResultsIgnoreDesc.Location = new System.Drawing.Point(12, 56);
            this.lblSeeExportResultsIgnoreDesc.Name = "lblSeeExportResultsIgnoreDesc";
            this.lblSeeExportResultsIgnoreDesc.Size = new System.Drawing.Size(317, 13);
            this.lblSeeExportResultsIgnoreDesc.TabIndex = 26;
            this.lblSeeExportResultsIgnoreDesc.Text = "You can choose to ignore some modifications in your CSV export :";
            // 
            // chkCSVExportIgnoreOffsetInPack
            // 
            this.chkCSVExportIgnoreOffsetInPack.AutoSize = true;
            this.chkCSVExportIgnoreOffsetInPack.Location = new System.Drawing.Point(255, 82);
            this.chkCSVExportIgnoreOffsetInPack.Name = "chkCSVExportIgnoreOffsetInPack";
            this.chkCSVExportIgnoreOffsetInPack.Size = new System.Drawing.Size(117, 17);
            this.chkCSVExportIgnoreOffsetInPack.TabIndex = 25;
            this.chkCSVExportIgnoreOffsetInPack.Text = "Ignore pack offsets";
            this.chkCSVExportIgnoreOffsetInPack.UseVisualStyleBackColor = true;
            // 
            // chkCSVExportIgnoreCompSize
            // 
            this.chkCSVExportIgnoreCompSize.AutoSize = true;
            this.chkCSVExportIgnoreCompSize.Location = new System.Drawing.Point(123, 82);
            this.chkCSVExportIgnoreCompSize.Name = "chkCSVExportIgnoreCompSize";
            this.chkCSVExportIgnoreCompSize.Size = new System.Drawing.Size(106, 17);
            this.chkCSVExportIgnoreCompSize.TabIndex = 24;
            this.chkCSVExportIgnoreCompSize.Text = "Ignore CompSize";
            this.chkCSVExportIgnoreCompSize.UseVisualStyleBackColor = true;
            // 
            // chkCSVExportIgnoreFlags
            // 
            this.chkCSVExportIgnoreFlags.AutoSize = true;
            this.chkCSVExportIgnoreFlags.Location = new System.Drawing.Point(15, 82);
            this.chkCSVExportIgnoreFlags.Name = "chkCSVExportIgnoreFlags";
            this.chkCSVExportIgnoreFlags.Size = new System.Drawing.Size(84, 17);
            this.chkCSVExportIgnoreFlags.TabIndex = 23;
            this.chkCSVExportIgnoreFlags.Text = "Ignore Flags";
            this.chkCSVExportIgnoreFlags.UseVisualStyleBackColor = true;
            // 
            // lblExportAddDate
            // 
            this.lblExportAddDate.AutoSize = true;
            this.lblExportAddDate.Location = new System.Drawing.Point(12, 150);
            this.lblExportAddDate.Name = "lblExportAddDate";
            this.lblExportAddDate.Size = new System.Drawing.Size(101, 13);
            this.lblExportAddDate.TabIndex = 27;
            this.lblExportAddDate.Text = "Add date to Export :";
            // 
            // chkExportAddDate
            // 
            this.chkExportAddDate.AutoSize = true;
            this.chkExportAddDate.Location = new System.Drawing.Point(123, 150);
            this.chkExportAddDate.Name = "chkExportAddDate";
            this.chkExportAddDate.Size = new System.Drawing.Size(15, 14);
            this.chkExportAddDate.TabIndex = 20;
            this.chkExportAddDate.UseVisualStyleBackColor = true;
            // 
            // lblExportAddDateDesc
            // 
            this.lblExportAddDateDesc.AutoSize = true;
            this.lblExportAddDateDesc.Location = new System.Drawing.Point(146, 150);
            this.lblExportAddDateDesc.Name = "lblExportAddDateDesc";
            this.lblExportAddDateDesc.Size = new System.Drawing.Size(275, 13);
            this.lblExportAddDateDesc.TabIndex = 20;
            this.lblExportAddDateDesc.Text = "Will add a subfolder with the current date to your exports.";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 479);
            this.Controls.Add(this.groupCSVExport);
            this.Controls.Add(this.groupDebug);
            this.Controls.Add(this.groupTools);
            this.Controls.Add(this.groupDirectories);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.VisibleChanged += new System.EventHandler(this.Options_VisibleChanged);
            this.groupDirectories.ResumeLayout(false);
            this.groupDirectories.PerformLayout();
            this.groupTools.ResumeLayout(false);
            this.groupTools.PerformLayout();
            this.groupDebug.ResumeLayout(false);
            this.groupDebug.PerformLayout();
            this.groupCSVExport.ResumeLayout(false);
            this.groupCSVExport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupDirectories;
        private System.Windows.Forms.Button btnDirWorkplaceFolder;
        public System.Windows.Forms.TextBox txtDirWorkplaceFolder;
        private System.Windows.Forms.Button btnDirExtractionFolder;
        public System.Windows.Forms.TextBox txtDirExtractionFolder;
        private System.Windows.Forms.Button btnDirTempFolder;
        public System.Windows.Forms.TextBox txtDirTempFolder;
        private System.Windows.Forms.Label lblDirWorkplaceFolder;
        private System.Windows.Forms.Label lblDirExtractionFolder;
        private System.Windows.Forms.Label lblDirTempFolder;
        private System.Windows.Forms.GroupBox groupTools;
        private System.Windows.Forms.Button btnDirHexEditor;
        private System.Windows.Forms.Label lblDirHexEditor;
        public System.Windows.Forms.TextBox txtDirHexEditor;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupDebug;
        private System.Windows.Forms.Label lblSkipJunkEntriesDesc;
        public System.Windows.Forms.CheckBox chkSkipJunkEntries;
        private System.Windows.Forms.Label lblSkipJunkEntries;
        public System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblForceOriginalFlagsDesc;
        public System.Windows.Forms.CheckBox chkForceOriginalFlags;
        private System.Windows.Forms.Label lblForceOriginalFlags;
        private System.Windows.Forms.Label lblDirExtractionFolderDesc;
        private System.Windows.Forms.Label lblDirTempFolderDesc;
        private System.Windows.Forms.Button btnDirExportFolder;
        public System.Windows.Forms.TextBox txtDirExportFolder;
        private System.Windows.Forms.Label lblDirExportFolder;
        private System.Windows.Forms.Label lblDirWorkplaceFolderDesc;
        private System.Windows.Forms.Label lblDirExportFolderDesc;
        private System.Windows.Forms.Label lblSeeExportResultsDesc;
        public System.Windows.Forms.CheckBox chkSeeExportResults;
        private System.Windows.Forms.Label lblSeeExportResults;
        private System.Windows.Forms.GroupBox groupCSVExport;
        public System.Windows.Forms.CheckBox chkCSVExportIgnoreOffsetInPack;
        public System.Windows.Forms.CheckBox chkCSVExportIgnoreCompSize;
        public System.Windows.Forms.CheckBox chkCSVExportIgnoreFlags;
        private System.Windows.Forms.Label lblSeeExportResultsIgnoreDesc;
        private System.Windows.Forms.Label lblExportAddDateDesc;
        public System.Windows.Forms.CheckBox chkExportAddDate;
        private System.Windows.Forms.Label lblExportAddDate;
    }
}