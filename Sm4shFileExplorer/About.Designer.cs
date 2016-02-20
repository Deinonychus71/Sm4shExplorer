namespace Sm4shFileExplorer
{
    partial class About
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblThanks = new System.Windows.Forms.Label();
            this.lblRepo = new System.Windows.Forms.Label();
            this.lkRepo = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(77, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Sm4shExplorer";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.ForeColor = System.Drawing.Color.Gray;
            this.lblAuthor.Location = new System.Drawing.Point(12, 38);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(93, 13);
            this.lblAuthor.TabIndex = 1;
            this.lblAuthor.Text = "by deinonychus71";
            // 
            // lblThanks
            // 
            this.lblThanks.AutoSize = true;
            this.lblThanks.Location = new System.Drawing.Point(12, 120);
            this.lblThanks.Name = "lblThanks";
            this.lblThanks.Size = new System.Drawing.Size(95, 13);
            this.lblThanks.TabIndex = 2;
            this.lblThanks.Text = "Special thanks to :";
            // 
            // lblRepo
            // 
            this.lblRepo.AutoSize = true;
            this.lblRepo.Location = new System.Drawing.Point(12, 70);
            this.lblRepo.Name = "lblRepo";
            this.lblRepo.Size = new System.Drawing.Size(44, 13);
            this.lblRepo.TabIndex = 3;
            this.lblRepo.Text = "Github :";
            // 
            // lkRepo
            // 
            this.lkRepo.AutoSize = true;
            this.lkRepo.Location = new System.Drawing.Point(12, 88);
            this.lkRepo.Name = "lkRepo";
            this.lkRepo.Size = new System.Drawing.Size(254, 13);
            this.lkRepo.TabIndex = 4;
            this.lkRepo.TabStop = true;
            this.lkRepo.Text = "https://github.com/Deinonychus71/Sm4shExplorer/";
            this.lkRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkRepo_LinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 202);
            this.Controls.Add(this.lkRepo);
            this.Controls.Add(this.lblRepo);
            this.Controls.Add(this.lblThanks);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblThanks;
        private System.Windows.Forms.Label lblRepo;
        private System.Windows.Forms.LinkLabel lkRepo;
    }
}