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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigationTreeView));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.setLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetAllLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.showHierarhyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllByDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSortToggle = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnShowLineNumbers = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFilterByMarks = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnErrorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btnErrorDiagnosis = new System.Windows.Forms.ToolStripDropDownButton();
            this.lbDocName = new System.Windows.Forms.ToolStripLabel();
            this.panelLinesNumbers = new System.Windows.Forms.Panel();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.taskListListView = new System.Windows.Forms.ListView();
            this.indexToDoListColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textToDoListColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lineNoToDoListColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new JsParserCore.UI.CustomTreeView();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStripMenuItem6,
            this.toolStripMenuItem5,
            this.toolStripMenuItem4,
            this.toolStripMenuItem3,
            this.toolStripMenuItem2,
            this.setLabelToolStripMenuItem,
            this.toolStripMenuItem1,
            this.resetLabelToolStripMenuItem,
            this.resetAllLabelsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 186);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem6.Image = global::JsParserCore.Properties.Resources.flag_white;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem6.Text = "White mark";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem5.Image = global::JsParserCore.Properties.Resources.flag_green;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem5.Text = "Green mark";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem4.Image = global::JsParserCore.Properties.Resources.flag_blue;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem4.Text = "Blue mark";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem3.Image = global::JsParserCore.Properties.Resources.flag_orange;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "Orange mark";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem2.Image = global::JsParserCore.Properties.Resources.flag_red;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Red mark";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // setLabelToolStripMenuItem
            // 
            this.setLabelToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.setLabelToolStripMenuItem.Image = global::JsParserCore.Properties.Resources.icon_favourites;
            this.setLabelToolStripMenuItem.Name = "setLabelToolStripMenuItem";
            this.setLabelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setLabelToolStripMenuItem.Text = "Star mark";
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
            this.resetLabelToolStripMenuItem.Text = "Reset mark";
            this.resetLabelToolStripMenuItem.Click += new System.EventHandler(this.resetLabelToolStripMenuItem_Click);
            // 
            // resetAllLabelsToolStripMenuItem
            // 
            this.resetAllLabelsToolStripMenuItem.Name = "resetAllLabelsToolStripMenuItem";
            this.resetAllLabelsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetAllLabelsToolStripMenuItem.Text = "Reset all marks";
            this.resetAllLabelsToolStripMenuItem.Click += new System.EventHandler(this.resetAllLabelsToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton2,
            this.btnSortToggle,
            this.toolStripSeparator1,
            this.toolStripDropDownButton1,
            this.toolStripSeparator3,
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.btnRefresh,
            this.btnErrorSeparator,
            this.btnErrorDiagnosis,
            this.lbDocName});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(246, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.AutoToolTip = false;
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHierarhyToolStripMenuItem,
            this.expandAllByDefaultToolStripMenuItem,
            this.toolStripMenuItem7,
            this.expandAllToolStripMenuItem,
            this.collapseAllNodesToolStripMenuItem});
            this.toolStripDropDownButton2.Image = global::JsParserCore.Properties.Resources.tree;
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton2.Text = "toolStripDropDownButton2";
            // 
            // showHierarhyToolStripMenuItem
            // 
            this.showHierarhyToolStripMenuItem.CheckOnClick = true;
            this.showHierarhyToolStripMenuItem.Name = "showHierarhyToolStripMenuItem";
            this.showHierarhyToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.showHierarhyToolStripMenuItem.Text = "Show hierarchy";
            this.showHierarhyToolStripMenuItem.Click += new System.EventHandler(this.showHierarhyToolStripMenuItem_Click);
            // 
            // expandAllByDefaultToolStripMenuItem
            // 
            this.expandAllByDefaultToolStripMenuItem.CheckOnClick = true;
            this.expandAllByDefaultToolStripMenuItem.Name = "expandAllByDefaultToolStripMenuItem";
            this.expandAllByDefaultToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.expandAllByDefaultToolStripMenuItem.Text = "Expand all by default";
            this.expandAllByDefaultToolStripMenuItem.Click += new System.EventHandler(this.expandAllByDefaultToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(180, 6);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.expandAllToolStripMenuItem.Text = "Expand all nodes";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllNodesToolStripMenuItem
            // 
            this.collapseAllNodesToolStripMenuItem.Name = "collapseAllNodesToolStripMenuItem";
            this.collapseAllNodesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.collapseAllNodesToolStripMenuItem.Text = "Collapse all nodes";
            this.collapseAllNodesToolStripMenuItem.Click += new System.EventHandler(this.collapseAllNodesToolStripMenuItem_Click);
            // 
            // btnSortToggle
            // 
            this.btnSortToggle.CheckOnClick = true;
            this.btnSortToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSortToggle.Image = global::JsParserCore.Properties.Resources.SortAZ;
            this.btnSortToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSortToggle.Name = "btnSortToggle";
            this.btnSortToggle.Size = new System.Drawing.Size(23, 22);
            this.btnSortToggle.Text = "Sort";
            this.btnSortToggle.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoToolTip = false;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnShowLineNumbers,
            this.btnFilterByMarks,
            this.settingsToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::JsParserCore.Properties.Resources.list_settings;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "View tools";
            // 
            // btnShowLineNumbers
            // 
            this.btnShowLineNumbers.CheckOnClick = true;
            this.btnShowLineNumbers.Name = "btnShowLineNumbers";
            this.btnShowLineNumbers.Size = new System.Drawing.Size(175, 22);
            this.btnShowLineNumbers.Text = "Show line numbers";
            this.btnShowLineNumbers.Click += new System.EventHandler(this.showLineNumbersToolStripMenuItem_Click);
            // 
            // btnFilterByMarks
            // 
            this.btnFilterByMarks.CheckOnClick = true;
            this.btnFilterByMarks.Name = "btnFilterByMarks";
            this.btnFilterByMarks.Size = new System.Drawing.Size(175, 22);
            this.btnFilterByMarks.Text = "Filter by marks";
            this.btnFilterByMarks.Click += new System.EventHandler(this.btnFilterByMarks_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::JsParserCore.Properties.Resources.Find_icon;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Find function";
            this.toolStripButton1.Click += new System.EventHandler(this.toolFindButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::JsParserCore.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnErrorSeparator
            // 
            this.btnErrorSeparator.Name = "btnErrorSeparator";
            this.btnErrorSeparator.Size = new System.Drawing.Size(6, 25);
            this.btnErrorSeparator.Visible = false;
            // 
            // btnErrorDiagnosis
            // 
            this.btnErrorDiagnosis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnErrorDiagnosis.Image = global::JsParserCore.Properties.Resources.error_icon;
            this.btnErrorDiagnosis.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnErrorDiagnosis.Name = "btnErrorDiagnosis";
            this.btnErrorDiagnosis.Size = new System.Drawing.Size(29, 22);
            this.btnErrorDiagnosis.Text = "Errors";
            this.btnErrorDiagnosis.Visible = false;
            // 
            // lbDocName
            // 
            this.lbDocName.Name = "lbDocName";
            this.lbDocName.Size = new System.Drawing.Size(25, 22);
            this.lbDocName.Text = "      ";
            // 
            // panelLinesNumbers
            // 
            this.panelLinesNumbers.Location = new System.Drawing.Point(12, 22);
            this.panelLinesNumbers.Name = "panelLinesNumbers";
            this.panelLinesNumbers.Size = new System.Drawing.Size(25, 119);
            this.panelLinesNumbers.TabIndex = 4;
            this.panelLinesNumbers.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLinesNumbers_Paint);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick_1);
            // 
            // taskListListView
            // 
            this.taskListListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskListListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.indexToDoListColumn,
            this.textToDoListColumn,
            this.lineNoToDoListColumn});
            this.taskListListView.FullRowSelect = true;
            this.taskListListView.GridLines = true;
            this.taskListListView.Location = new System.Drawing.Point(0, 20);
            this.taskListListView.Margin = new System.Windows.Forms.Padding(0);
            this.taskListListView.Name = "taskListListView";
            this.taskListListView.ShowItemToolTips = true;
            this.taskListListView.Size = new System.Drawing.Size(200, 68);
            this.taskListListView.TabIndex = 6;
            this.taskListListView.UseCompatibleStateImageBehavior = false;
            this.taskListListView.View = System.Windows.Forms.View.Details;
            this.taskListListView.DoubleClick += new System.EventHandler(this.TaskListItemClick);
            // 
            // indexToDoListColumn
            // 
            this.indexToDoListColumn.Text = "#";
            this.indexToDoListColumn.Width = 20;
            // 
            // textToDoListColumn
            // 
            this.textToDoListColumn.Text = "Description";
            this.textToDoListColumn.Width = 160;
            // 
            // lineNoToDoListColumn
            // 
            this.lineNoToDoListColumn.Text = "Line#";
            this.lineNoToDoListColumn.Width = 40;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(17, 36);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelLinesNumbers);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Resize += new System.EventHandler(this.splitContainer1_Panel1_Resize);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.taskListListView);
            this.splitContainer1.Size = new System.Drawing.Size(200, 331);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Image = global::JsParserCore.Properties.Resources.task_List;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(-2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "      Task List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeView1
            // 
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(43, 22);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(145, 119);
            this.treeView1.StateImageList = this.imageList1;
            this.treeView1.TabIndex = 2;
            this.treeView1.OnScroll += new System.EventHandler(this.treeView1_OnScroll);
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCollapse);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView1_DrawNode);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treeView1_KeyPress);
            // 
            // NavigationTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip2);
            this.DoubleBuffered = true;
            this.Name = "NavigationTreeView";
            this.Size = new System.Drawing.Size(246, 381);
            this.Resize += new System.EventHandler(this.NavigationTreeView_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

		private CustomTreeView treeView1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem resetLabelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setLabelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem resetAllLabelsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton btnSortToggle;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnRefresh;
		private System.Windows.Forms.ToolStripLabel lbDocName;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem btnShowLineNumbers;
		private System.Windows.Forms.ToolStripMenuItem btnFilterByMarks;
		private System.Windows.Forms.Panel panelLinesNumbers;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
		private System.Windows.Forms.ToolStripMenuItem showHierarhyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem expandAllByDefaultToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton btnErrorDiagnosis;
        private System.Windows.Forms.ToolStripSeparator btnErrorSeparator;
        private System.Windows.Forms.ListView taskListListView;
        private System.Windows.Forms.ColumnHeader indexToDoListColumn;
        private System.Windows.Forms.ColumnHeader textToDoListColumn;
        private System.Windows.Forms.ColumnHeader lineNoToDoListColumn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
    }
}
