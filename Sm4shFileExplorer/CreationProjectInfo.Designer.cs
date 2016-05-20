namespace Sm4shFileExplorer
{
    partial class CreationProjectInfo
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblGameRegion = new System.Windows.Forms.Label();
            this.lblGameVersion = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtGameVersion = new System.Windows.Forms.NumericUpDown();
            this.ddpGameRegion = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtGameVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(12, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(425, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "In order for Sm4shExplorer to work as intended the following information must be " +
    "correct :";
            // 
            // lblGameRegion
            // 
            this.lblGameRegion.AutoSize = true;
            this.lblGameRegion.Location = new System.Drawing.Point(12, 37);
            this.lblGameRegion.Name = "lblGameRegion";
            this.lblGameRegion.Size = new System.Drawing.Size(81, 13);
            this.lblGameRegion.TabIndex = 1;
            this.lblGameRegion.Text = "Game Region : ";
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.AutoSize = true;
            this.lblGameVersion.Location = new System.Drawing.Point(12, 63);
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(82, 13);
            this.lblGameVersion.TabIndex = 2;
            this.lblGameVersion.Text = "Game Version : ";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(376, 82);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtGameVersion
            // 
            this.txtGameVersion.Location = new System.Drawing.Point(100, 61);
            this.txtGameVersion.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.txtGameVersion.Name = "txtGameVersion";
            this.txtGameVersion.Size = new System.Drawing.Size(120, 20);
            this.txtGameVersion.TabIndex = 4;
            // 
            // ddpGameRegion
            // 
            this.ddpGameRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddpGameRegion.FormattingEnabled = true;
            this.ddpGameRegion.Location = new System.Drawing.Point(99, 34);
            this.ddpGameRegion.Name = "ddpGameRegion";
            this.ddpGameRegion.Size = new System.Drawing.Size(121, 21);
            this.ddpGameRegion.TabIndex = 5;
            this.ddpGameRegion.VisibleChanged += new System.EventHandler(this.ddpGameRegion_VisibleChanged);
            // 
            // CreationProjectInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 113);
            this.ControlBox = false;
            this.Controls.Add(this.ddpGameRegion);
            this.Controls.Add(this.txtGameVersion);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblGameVersion);
            this.Controls.Add(this.lblGameRegion);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreationProjectInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Creation Project";
            ((System.ComponentModel.ISupportInitialize)(this.txtGameVersion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblGameRegion;
        private System.Windows.Forms.Label lblGameVersion;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown txtGameVersion;
        private System.Windows.Forms.ComboBox ddpGameRegion;
    }
}