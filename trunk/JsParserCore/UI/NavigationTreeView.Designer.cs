using System.Resources;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.ComponentModel;
namespace JsParserCore.UI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigationTreeView));
			this.topBar = new System.Windows.Forms.Panel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnTreeToggle = new System.Windows.Forms.ToolStripButton();
			this.btnSortToggle = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnRefresh = new System.Windows.Forms.ToolStripButton();
			this.lbDocName = new System.Windows.Forms.ToolStripLabel();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
			this.setLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.resetLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetAllLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.topBar.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// topBar
			// 
			this.topBar.Controls.Add(this.toolStrip1);
			this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.topBar.Location = new System.Drawing.Point(0, 0);
			this.topBar.Name = "topBar";
			this.topBar.Size = new System.Drawing.Size(234, 26);
			this.topBar.TabIndex = 1;
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTreeToggle,
            this.btnSortToggle,
            this.toolStripSeparator1,
            this.btnRefresh,
            this.lbDocName});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(234, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnTreeToggle
			// 
			this.btnTreeToggle.Checked = true;
			this.btnTreeToggle.CheckOnClick = true;
			this.btnTreeToggle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnTreeToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnTreeToggle.Image = global::JsParserCore.Properties.Resources.tree;
			this.btnTreeToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnTreeToggle.Name = "btnTreeToggle";
			this.btnTreeToggle.Size = new System.Drawing.Size(23, 22);
			this.btnTreeToggle.Text = "toolStripButton1";
			this.btnTreeToggle.ToolTipText = "Hierarchical";
			this.btnTreeToggle.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnSortToggle
			// 
			this.btnSortToggle.CheckOnClick = true;
			this.btnSortToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSortToggle.Image = global::JsParserCore.Properties.Resources.SortAZ;
			this.btnSortToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSortToggle.Name = "btnSortToggle";
			this.btnSortToggle.Size = new System.Drawing.Size(23, 22);
			this.btnSortToggle.Text = "toolStripButton2";
			this.btnSortToggle.ToolTipText = "Alphabetical";
			this.btnSortToggle.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnRefresh
			// 
			this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRefresh.Image = global::JsParserCore.Properties.Resources.refresh;
			this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(23, 22);
			this.btnRefresh.Text = "toolStripButton4";
			this.btnRefresh.ToolTipText = "Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// lbDocName
			// 
			this.lbDocName.Name = "lbDocName";
			this.lbDocName.Size = new System.Drawing.Size(58, 22);
			this.lbDocName.Text = "< None >";
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.FullRowSelect = true;
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(0, 26);
			this.treeView1.Name = "treeView1";
			this.treeView1.ShowNodeToolTips = true;
			this.treeView1.Size = new System.Drawing.Size(234, 274);
			this.treeView1.StateImageList = this.imageList1;
			this.treeView1.TabIndex = 2;
			this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "function.png");
			this.imageList1.Images.SetKeyName(1, "type.png");
			this.imageList1.Images.SetKeyName(2, "undefined.png");
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setLabelToolStripMenuItem,
            this.toolStripMenuItem1,
            this.resetLabelToolStripMenuItem,
            this.resetAllLabelsToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 98);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// setLabelToolStripMenuItem
			// 
			this.setLabelToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
			this.setLabelToolStripMenuItem.Name = "setLabelToolStripMenuItem";
			this.setLabelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.setLabelToolStripMenuItem.Text = "Set label";
			this.setLabelToolStripMenuItem.Click += new System.EventHandler(this.setLabelToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// resetLabelToolStripMenuItem
			// 
			this.resetLabelToolStripMenuItem.Name = "resetLabelToolStripMenuItem";
			this.resetLabelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.resetLabelToolStripMenuItem.Text = "Reset label";
			this.resetLabelToolStripMenuItem.Click += new System.EventHandler(this.resetLabelToolStripMenuItem_Click);
			// 
			// resetAllLabelsToolStripMenuItem
			// 
			this.resetAllLabelsToolStripMenuItem.Name = "resetAllLabelsToolStripMenuItem";
			this.resetAllLabelsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.resetAllLabelsToolStripMenuItem.Text = "Reset all labels";
			this.resetAllLabelsToolStripMenuItem.Click += new System.EventHandler(this.resetAllLabelsToolStripMenuItem_Click);
			// 
			// NavigationTreeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.topBar);
			this.Name = "NavigationTreeView";
			this.Size = new System.Drawing.Size(234, 300);
			this.Load += new System.EventHandler(this.NavigationTreeView_Load);
			this.topBar.ResumeLayout(false);
			this.topBar.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

        }



        #endregion

		private System.Windows.Forms.Panel topBar;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnTreeToggle;
		private System.Windows.Forms.ToolStripButton btnSortToggle;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripLabel lbDocName;
		private System.Windows.Forms.ToolStripButton btnRefresh;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem resetLabelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setLabelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem resetAllLabelsToolStripMenuItem;
    }
}
