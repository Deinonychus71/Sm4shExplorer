namespace Sm4shSound.UserControls
{
    partial class MusicStageList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelSoundDBAction = new System.Windows.Forms.Panel();
            this.ddlEntries = new System.Windows.Forms.ComboBox();
            this.help = new System.Windows.Forms.PictureBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panelSoundDBStatus = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblEntries = new System.Windows.Forms.Label();
            this.vgmStream = new Sm4shSound.UserControls.VGMStreamPlayer();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.panelSoundDBAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.help)).BeginInit();
            this.panelSoundDBStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSoundDBAction
            // 
            this.panelSoundDBAction.Controls.Add(this.ddlEntries);
            this.panelSoundDBAction.Controls.Add(this.help);
            this.panelSoundDBAction.Controls.Add(this.btnAdd);
            this.panelSoundDBAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSoundDBAction.Location = new System.Drawing.Point(0, 0);
            this.panelSoundDBAction.Name = "panelSoundDBAction";
            this.panelSoundDBAction.Size = new System.Drawing.Size(288, 35);
            this.panelSoundDBAction.TabIndex = 1;
            // 
            // ddlEntries
            // 
            this.ddlEntries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEntries.FormattingEnabled = true;
            this.ddlEntries.Location = new System.Drawing.Point(0, 9);
            this.ddlEntries.Name = "ddlEntries";
            this.ddlEntries.Size = new System.Drawing.Size(205, 21);
            this.ddlEntries.TabIndex = 29;
            // 
            // help
            // 
            this.help.Image = global::Sm4shSound.Properties.Resources.help;
            this.help.Location = new System.Drawing.Point(268, 12);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(16, 16);
            this.help.TabIndex = 28;
            this.help.TabStop = false;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(211, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 21);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panelSoundDBStatus
            // 
            this.panelSoundDBStatus.Controls.Add(this.btnRemove);
            this.panelSoundDBStatus.Controls.Add(this.lblEntries);
            this.panelSoundDBStatus.Controls.Add(this.vgmStream);
            this.panelSoundDBStatus.Controls.Add(this.btnMoveUp);
            this.panelSoundDBStatus.Controls.Add(this.btnMoveDown);
            this.panelSoundDBStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSoundDBStatus.Location = new System.Drawing.Point(0, 270);
            this.panelSoundDBStatus.Name = "panelSoundDBStatus";
            this.panelSoundDBStatus.Size = new System.Drawing.Size(288, 30);
            this.panelSoundDBStatus.TabIndex = 3;
            // 
            // btnRemove
            // 
            this.btnRemove.Image = global::Sm4shSound.Properties.Resources.remove;
            this.btnRemove.Location = new System.Drawing.Point(65, 2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 25);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblEntries
            // 
            this.lblEntries.Location = new System.Drawing.Point(223, 7);
            this.lblEntries.Name = "lblEntries";
            this.lblEntries.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblEntries.Size = new System.Drawing.Size(62, 17);
            this.lblEntries.TabIndex = 5;
            this.lblEntries.Text = "values";
            // 
            // vgmStream
            // 
            this.vgmStream.File = null;
            this.vgmStream.Location = new System.Drawing.Point(101, 4);
            this.vgmStream.Name = "vgmStream";
            this.vgmStream.Size = new System.Drawing.Size(91, 21);
            this.vgmStream.TabIndex = 4;
            this.vgmStream.Valid = false;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Image = global::Sm4shSound.Properties.Resources.arrowup;
            this.btnMoveUp.Location = new System.Drawing.Point(29, 2);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(30, 25);
            this.btnMoveUp.TabIndex = 3;
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Image = global::Sm4shSound.Properties.Resources.arrowdown;
            this.btnMoveDown.Location = new System.Drawing.Point(0, 2);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(30, 25);
            this.btnMoveDown.TabIndex = 2;
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(0, 35);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(288, 235);
            this.listBox.TabIndex = 4;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // MusicStageList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.panelSoundDBStatus);
            this.Controls.Add(this.panelSoundDBAction);
            this.Name = "MusicStageList";
            this.Size = new System.Drawing.Size(288, 300);
            this.panelSoundDBAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.help)).EndInit();
            this.panelSoundDBStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSoundDBAction;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panelSoundDBStatus;
        private VGMStreamPlayer vgmStream;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label lblEntries;
        private System.Windows.Forms.PictureBox help;
        private System.Windows.Forms.ComboBox ddlEntries;
        private System.Windows.Forms.Button btnRemove;
    }
}
