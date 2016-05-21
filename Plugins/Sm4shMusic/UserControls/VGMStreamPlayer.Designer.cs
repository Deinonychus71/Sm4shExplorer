namespace Sm4shMusic.UserControls
{
    partial class VGMStreamPlayer
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
            this.panelCommands = new System.Windows.Forms.Panel();
            this.btnPlay = new System.Windows.Forms.PictureBox();
            this.panelStream = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.panelCommands.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).BeginInit();
            this.panelStream.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCommands
            // 
            this.panelCommands.Controls.Add(this.btnPlay);
            this.panelCommands.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCommands.Location = new System.Drawing.Point(0, 0);
            this.panelCommands.Name = "panelCommands";
            this.panelCommands.Size = new System.Drawing.Size(26, 21);
            this.panelCommands.TabIndex = 1;
            // 
            // btnPlay
            // 
            this.btnPlay.Image = global::Sm4shMusic.Properties.Resources.play;
            this.btnPlay.Location = new System.Drawing.Point(1, 2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(16, 16);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.TabStop = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // panelStream
            // 
            this.panelStream.Controls.Add(this.lblTime);
            this.panelStream.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelStream.Location = new System.Drawing.Point(19, 0);
            this.panelStream.Name = "panelStream";
            this.panelStream.Size = new System.Drawing.Size(73, 21);
            this.panelStream.TabIndex = 2;
            // 
            // lblTime
            // 
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTime.Location = new System.Drawing.Point(0, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTime.Size = new System.Drawing.Size(73, 21);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "00:00 / 00:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VGMStreamPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStream);
            this.Controls.Add(this.panelCommands);
            this.Name = "VGMStreamPlayer";
            this.Size = new System.Drawing.Size(92, 21);
            this.panelCommands.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).EndInit();
            this.panelStream.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelCommands;
        private System.Windows.Forms.Panel panelStream;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.PictureBox btnPlay;
    }
}
