namespace Sm4shSound.Forms
{
    partial class SortMusic
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
            this.groupActions = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnMoveUp10 = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown10 = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.btnRestoreOrder = new System.Windows.Forms.Button();
            this.groupActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupActions
            // 
            this.groupActions.Controls.Add(this.btnCancel);
            this.groupActions.Controls.Add(this.btnMoveUp10);
            this.groupActions.Controls.Add(this.btnMoveUp);
            this.groupActions.Controls.Add(this.btnMoveDown10);
            this.groupActions.Controls.Add(this.btnMoveDown);
            this.groupActions.Controls.Add(this.btnSave);
            this.groupActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupActions.Location = new System.Drawing.Point(0, 0);
            this.groupActions.Name = "groupActions";
            this.groupActions.Size = new System.Drawing.Size(385, 79);
            this.groupActions.TabIndex = 0;
            this.groupActions.TabStop = false;
            this.groupActions.Text = "Actions";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(233, 48);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Discard changes and exit";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnMoveUp10
            // 
            this.btnMoveUp10.Location = new System.Drawing.Point(113, 48);
            this.btnMoveUp10.Name = "btnMoveUp10";
            this.btnMoveUp10.Size = new System.Drawing.Size(95, 23);
            this.btnMoveUp10.TabIndex = 4;
            this.btnMoveUp10.Text = "Move Up 10";
            this.btnMoveUp10.UseVisualStyleBackColor = true;
            this.btnMoveUp10.Click += new System.EventHandler(this.btnMoveUp10_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(113, 19);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(95, 23);
            this.btnMoveUp.TabIndex = 3;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown10
            // 
            this.btnMoveDown10.Location = new System.Drawing.Point(12, 48);
            this.btnMoveDown10.Name = "btnMoveDown10";
            this.btnMoveDown10.Size = new System.Drawing.Size(95, 23);
            this.btnMoveDown10.TabIndex = 2;
            this.btnMoveDown10.Text = "Move Down -10";
            this.btnMoveDown10.UseVisualStyleBackColor = true;
            this.btnMoveDown10.Click += new System.EventHandler(this.btnMoveDown10_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(12, 19);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(95, 23);
            this.btnMoveDown.TabIndex = 1;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(233, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(144, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save ordering and exit";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 79);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(385, 378);
            this.treeView.TabIndex = 1;
            // 
            // btnRestoreOrder
            // 
            this.btnRestoreOrder.Location = new System.Drawing.Point(310, 79);
            this.btnRestoreOrder.Name = "btnRestoreOrder";
            this.btnRestoreOrder.Size = new System.Drawing.Size(55, 20);
            this.btnRestoreOrder.TabIndex = 34;
            this.btnRestoreOrder.Text = "Restore";
            this.btnRestoreOrder.UseVisualStyleBackColor = true;
            this.btnRestoreOrder.Click += new System.EventHandler(this.btnRestoreOrder_Click);
            // 
            // SortMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 457);
            this.ControlBox = false;
            this.Controls.Add(this.btnRestoreOrder);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.groupActions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SortMusic";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SortMusic";
            this.groupActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupActions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMoveUp10;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown10;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnRestoreOrder;
    }
}