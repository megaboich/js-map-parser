using System.Resources;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.ComponentModel;
namespace JS_addin.Addin.UI
{
    partial class NavigationTreeView
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
			this.topBar = new System.Windows.Forms.Panel();
			this.lbDocName = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.topBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// topBar
			// 
			this.topBar.Controls.Add(this.lbDocName);
			this.topBar.Controls.Add(this.btnRefresh);
			this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.topBar.Location = new System.Drawing.Point(0, 0);
			this.topBar.Name = "topBar";
			this.topBar.Size = new System.Drawing.Size(234, 31);
			this.topBar.TabIndex = 1;
			// 
			// lbDocName
			// 
			this.lbDocName.AutoSize = true;
			this.lbDocName.Location = new System.Drawing.Point(3, 9);
			this.lbDocName.Name = "lbDocName";
			this.lbDocName.Size = new System.Drawing.Size(77, 13);
			this.lbDocName.TabIndex = 1;
			this.lbDocName.Text = "< Empty Doc >";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRefresh.Location = new System.Drawing.Point(156, 3);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 25);
			this.btnRefresh.TabIndex = 0;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 31);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(234, 269);
			this.treeView1.TabIndex = 2;
			this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
			// 
			// NavigationTreeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.topBar);
			this.Name = "NavigationTreeView";
			this.Size = new System.Drawing.Size(234, 300);
			this.topBar.ResumeLayout(false);
			this.topBar.PerformLayout();
			this.ResumeLayout(false);

        }



        #endregion

		private System.Windows.Forms.Panel topBar;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Label lbDocName;
		private System.Windows.Forms.TreeView treeView1;
    }
}
