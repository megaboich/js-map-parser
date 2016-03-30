using System.Resources;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.ComponentModel;
namespace JsParser.UI.UI
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
            this.contextMenuMarks0Item = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuMarks1Item = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuMarks2Item = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuMarks3Item = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuMarks4Item = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuMarks5Item = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetAllLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.expandAllByDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.sortItemsAlphabeticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLineNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterByMarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAnonymousFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btnErrorDiagnosis = new System.Windows.Forms.ToolStripDropDownButton();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelLinesNumbers = new JsParser.UI.UI.CustomPanel();
            this.treeView1 = new JsParser.UI.UI.CustomTreeView();
            this.taskListDataGrid = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnToDoListToggle = new System.Windows.Forms.Button();
            this.lbTaskList = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskListDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "type.png");
            this.imageList1.Images.SetKeyName(1, "undefined.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuMarks0Item,
            this.contextMenuMarks1Item,
            this.contextMenuMarks2Item,
            this.contextMenuMarks3Item,
            this.contextMenuMarks4Item,
            this.contextMenuMarks5Item,
            this.toolStripMenuItem1,
            this.resetLabelToolStripMenuItem,
            this.resetAllLabelsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 186);
            // 
            // contextMenuMarks0Item
            // 
            this.contextMenuMarks0Item.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuMarks0Item.Image = global::JsParser.UI.Properties.Resources.flag_white;
            this.contextMenuMarks0Item.Name = "contextMenuMarks0Item";
            this.contextMenuMarks0Item.Size = new System.Drawing.Size(152, 22);
            this.contextMenuMarks0Item.Tag = "W";
            this.contextMenuMarks0Item.Text = "White mark";
            this.contextMenuMarks0Item.Click += new System.EventHandler(this.contextMenuMarks0Item_Click);
            // 
            // contextMenuMarks1Item
            // 
            this.contextMenuMarks1Item.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuMarks1Item.Image = global::JsParser.UI.Properties.Resources.flag_blue;
            this.contextMenuMarks1Item.Name = "contextMenuMarks1Item";
            this.contextMenuMarks1Item.Size = new System.Drawing.Size(152, 22);
            this.contextMenuMarks1Item.Tag = "B";
            this.contextMenuMarks1Item.Text = "Blue mark";
            this.contextMenuMarks1Item.Click += new System.EventHandler(this.contextMenuMarks0Item_Click);
            // 
            // contextMenuMarks2Item
            // 
            this.contextMenuMarks2Item.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuMarks2Item.Image = global::JsParser.UI.Properties.Resources.flag_green;
            this.contextMenuMarks2Item.Name = "contextMenuMarks2Item";
            this.contextMenuMarks2Item.Size = new System.Drawing.Size(152, 22);
            this.contextMenuMarks2Item.Tag = "G";
            this.contextMenuMarks2Item.Text = "Green mark";
            this.contextMenuMarks2Item.Click += new System.EventHandler(this.contextMenuMarks0Item_Click);
            // 
            // contextMenuMarks3Item
            // 
            this.contextMenuMarks3Item.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuMarks3Item.Image = global::JsParser.UI.Properties.Resources.flag_orange;
            this.contextMenuMarks3Item.Name = "contextMenuMarks3Item";
            this.contextMenuMarks3Item.Size = new System.Drawing.Size(152, 22);
            this.contextMenuMarks3Item.Tag = "O";
            this.contextMenuMarks3Item.Text = "Orange mark";
            this.contextMenuMarks3Item.Click += new System.EventHandler(this.contextMenuMarks0Item_Click);
            // 
            // contextMenuMarks4Item
            // 
            this.contextMenuMarks4Item.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuMarks4Item.Image = global::JsParser.UI.Properties.Resources.flag_red;
            this.contextMenuMarks4Item.Name = "contextMenuMarks4Item";
            this.contextMenuMarks4Item.Size = new System.Drawing.Size(152, 22);
            this.contextMenuMarks4Item.Tag = "R";
            this.contextMenuMarks4Item.Text = "Red mark";
            this.contextMenuMarks4Item.Click += new System.EventHandler(this.contextMenuMarks0Item_Click);
            // 
            // contextMenuMarks5Item
            // 
            this.contextMenuMarks5Item.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuMarks5Item.Image = global::JsParser.UI.Properties.Resources.icon_favourites;
            this.contextMenuMarks5Item.Name = "contextMenuMarks5Item";
            this.contextMenuMarks5Item.Size = new System.Drawing.Size(152, 22);
            this.contextMenuMarks5Item.Tag = "S";
            this.contextMenuMarks5Item.Text = "Star mark";
            this.contextMenuMarks5Item.Click += new System.EventHandler(this.contextMenuMarks0Item_Click);
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
            // toolStrip2
            // 
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton2,
            this.toolStripButton1,
            this.btnErrorDiagnosis});
            this.toolStrip2.Location = new System.Drawing.Point(0, -8);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1000, 42);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.AutoToolTip = false;
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllByDefaultToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.collapseAllNodesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.sortItemsAlphabeticallyToolStripMenuItem,
            this.showLineNumbersToolStripMenuItem,
            this.filterByMarksToolStripMenuItem,
            this.hideAnonymousFunctionsToolStripMenuItem,
            this.toolStripMenuItem7,
            this.settingsToolStripMenuItem1});
            this.toolStripDropDownButton2.Image = global::JsParser.UI.Properties.Resources.list_settings;
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(37, 22);
            this.toolStripDropDownButton2.Text = "toolStripDropDownButton2";
            // 
            // expandAllByDefaultToolStripMenuItem
            // 
            this.expandAllByDefaultToolStripMenuItem.CheckOnClick = true;
            this.expandAllByDefaultToolStripMenuItem.Name = "expandAllByDefaultToolStripMenuItem";
            this.expandAllByDefaultToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.expandAllByDefaultToolStripMenuItem.Text = "Expand All By Default";
            this.expandAllByDefaultToolStripMenuItem.Click += new System.EventHandler(this.expandAllByDefaultToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All Nodes";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllNodesToolStripMenuItem
            // 
            this.collapseAllNodesToolStripMenuItem.Name = "collapseAllNodesToolStripMenuItem";
            this.collapseAllNodesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.collapseAllNodesToolStripMenuItem.Text = "Collapse All Nodes";
            this.collapseAllNodesToolStripMenuItem.Click += new System.EventHandler(this.collapseAllNodesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(219, 6);
            // 
            // sortItemsAlphabeticallyToolStripMenuItem
            // 
            this.sortItemsAlphabeticallyToolStripMenuItem.CheckOnClick = true;
            this.sortItemsAlphabeticallyToolStripMenuItem.Image = global::JsParser.UI.Properties.Resources.SortAZ;
            this.sortItemsAlphabeticallyToolStripMenuItem.Name = "sortItemsAlphabeticallyToolStripMenuItem";
            this.sortItemsAlphabeticallyToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.sortItemsAlphabeticallyToolStripMenuItem.Text = "Sort Items Alphabetically";
            this.sortItemsAlphabeticallyToolStripMenuItem.Click += new System.EventHandler(this.sortItemsAlphabeticallyToolStripMenuItem_Click);
            // 
            // showLineNumbersToolStripMenuItem
            // 
            this.showLineNumbersToolStripMenuItem.CheckOnClick = true;
            this.showLineNumbersToolStripMenuItem.Name = "showLineNumbersToolStripMenuItem";
            this.showLineNumbersToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.showLineNumbersToolStripMenuItem.Text = "Show Line Numbers";
            this.showLineNumbersToolStripMenuItem.Click += new System.EventHandler(this.showLineNumbersToolStripMenuItem_Click);
            // 
            // filterByMarksToolStripMenuItem
            // 
            this.filterByMarksToolStripMenuItem.CheckOnClick = true;
            this.filterByMarksToolStripMenuItem.Name = "filterByMarksToolStripMenuItem";
            this.filterByMarksToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.filterByMarksToolStripMenuItem.Text = "Filter By Marks";
            this.filterByMarksToolStripMenuItem.Click += new System.EventHandler(this.filterByMarksToolStripMenuItem_Click);
            // 
            // hideAnonymousFunctionsToolStripMenuItem
            // 
            this.hideAnonymousFunctionsToolStripMenuItem.CheckOnClick = true;
            this.hideAnonymousFunctionsToolStripMenuItem.Name = "hideAnonymousFunctionsToolStripMenuItem";
            this.hideAnonymousFunctionsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.hideAnonymousFunctionsToolStripMenuItem.Text = "Hide Anonymous Functions";
            this.hideAnonymousFunctionsToolStripMenuItem.Click += new System.EventHandler(this.hideAnonymousFunctionsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(219, 6);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Image = global::JsParser.UI.Properties.Resources.setttings;
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
            this.settingsToolStripMenuItem1.Text = "Settings...";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::JsParser.UI.Properties.Resources.Find_icon;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.toolStripButton1.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton1.Text = "Find function";
            this.toolStripButton1.Click += new System.EventHandler(this.toolFindButton_Click);
            // 
            // btnErrorDiagnosis
            // 
            this.btnErrorDiagnosis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnErrorDiagnosis.Image = global::JsParser.UI.Properties.Resources.error_icon;
            this.btnErrorDiagnosis.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnErrorDiagnosis.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.btnErrorDiagnosis.Name = "btnErrorDiagnosis";
            this.btnErrorDiagnosis.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnErrorDiagnosis.ShowDropDownArrow = false;
            this.btnErrorDiagnosis.Size = new System.Drawing.Size(28, 22);
            this.btnErrorDiagnosis.Text = "Errors";
            this.btnErrorDiagnosis.Visible = false;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick_1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(17, 36);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
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
            this.splitContainer1.Panel2.Controls.Add(this.taskListDataGrid);
            this.splitContainer1.Panel2.Controls.Add(this.btnToDoListToggle);
            this.splitContainer1.Panel2.Controls.Add(this.lbTaskList);
            this.splitContainer1.Panel2.Click += new System.EventHandler(this.btnToDoListToggle_Click);
            this.splitContainer1.Panel2MinSize = 20;
            this.splitContainer1.Size = new System.Drawing.Size(200, 331);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 7;
            this.splitContainer1.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitContainer1_SplitterMoving);
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // panelLinesNumbers
            // 
            this.panelLinesNumbers.Location = new System.Drawing.Point(12, 22);
            this.panelLinesNumbers.Name = "panelLinesNumbers";
            this.panelLinesNumbers.Size = new System.Drawing.Size(25, 119);
            this.panelLinesNumbers.TabIndex = 4;
            this.panelLinesNumbers.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLinesNumbers_Paint);
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Indent = 10;
            this.treeView1.Location = new System.Drawing.Point(43, 22);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = false;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(145, 119);
            this.treeView1.StateImageList = this.imageList1;
            this.treeView1.TabIndex = 2;
            this.treeView1.OnScroll += new JsParser.UI.UI.CustomTreeView.ScrollEventHandler(this.treeView1_OnScroll);
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
            // taskListDataGrid
            // 
            this.taskListDataGrid.AllowUserToAddRows = false;
            this.taskListDataGrid.AllowUserToDeleteRows = false;
            this.taskListDataGrid.AllowUserToOrderColumns = true;
            this.taskListDataGrid.AllowUserToResizeColumns = false;
            this.taskListDataGrid.AllowUserToResizeRows = false;
            this.taskListDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskListDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.taskListDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.taskListDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskListDataGrid.ColumnHeadersVisible = false;
            this.taskListDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Desc,
            this.Line});
            this.taskListDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.taskListDataGrid.Location = new System.Drawing.Point(3, 24);
            this.taskListDataGrid.MultiSelect = false;
            this.taskListDataGrid.Name = "taskListDataGrid";
            this.taskListDataGrid.ReadOnly = true;
            this.taskListDataGrid.RowHeadersVisible = false;
            this.taskListDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.taskListDataGrid.ShowCellErrors = false;
            this.taskListDataGrid.ShowEditingIcon = false;
            this.taskListDataGrid.ShowRowErrors = false;
            this.taskListDataGrid.Size = new System.Drawing.Size(194, 99);
            this.taskListDataGrid.TabIndex = 8;
            this.taskListDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.taskListDataGrid_CellClick);
            this.taskListDataGrid.Leave += new System.EventHandler(this.taskListDataGrid_Leave);
            // 
            // No
            // 
            this.No.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.No.DataPropertyName = "No";
            this.No.FillWeight = 10.6599F;
            this.No.HeaderText = "No";
            this.No.MinimumWidth = 20;
            this.No.Name = "No";
            this.No.ReadOnly = true;
            // 
            // Desc
            // 
            this.Desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Desc.DataPropertyName = "Desc";
            this.Desc.FillWeight = 75.47208F;
            this.Desc.HeaderText = "Desc";
            this.Desc.Name = "Desc";
            this.Desc.ReadOnly = true;
            // 
            // Line
            // 
            this.Line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Line.DataPropertyName = "Line";
            this.Line.FillWeight = 18.86802F;
            this.Line.HeaderText = "Line";
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            // 
            // btnToDoListToggle
            // 
            this.btnToDoListToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToDoListToggle.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnToDoListToggle.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnToDoListToggle.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnToDoListToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToDoListToggle.Image = global::JsParser.UI.Properties.Resources.navExpandArrow;
            this.btnToDoListToggle.Location = new System.Drawing.Point(181, 2);
            this.btnToDoListToggle.Name = "btnToDoListToggle";
            this.btnToDoListToggle.Size = new System.Drawing.Size(16, 16);
            this.btnToDoListToggle.TabIndex = 8;
            this.btnToDoListToggle.UseVisualStyleBackColor = false;
            this.btnToDoListToggle.Click += new System.EventHandler(this.btnToDoListToggle_Click);
            // 
            // lbTaskList
            // 
            this.lbTaskList.Image = global::JsParser.UI.Properties.Resources.task_List;
            this.lbTaskList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbTaskList.Location = new System.Drawing.Point(-2, 0);
            this.lbTaskList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTaskList.Name = "lbTaskList";
            this.lbTaskList.Size = new System.Drawing.Size(152, 20);
            this.lbTaskList.TabIndex = 7;
            this.lbTaskList.Text = "      Task List";
            this.lbTaskList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbTaskList.Click += new System.EventHandler(this.btnToDoListToggle_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Information";
            // 
            // NavigationTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip2);
            this.DoubleBuffered = true;
            this.Name = "NavigationTreeView";
            this.Size = new System.Drawing.Size(246, 532);
            this.Resize += new System.EventHandler(this.NavigationTreeView_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.taskListDataGrid)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private CustomTreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextMenuMarks5Item;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetAllLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextMenuMarks0Item;
        private System.Windows.Forms.ToolStripMenuItem contextMenuMarks2Item;
        private System.Windows.Forms.ToolStripMenuItem contextMenuMarks1Item;
        private System.Windows.Forms.ToolStripMenuItem contextMenuMarks3Item;
        private System.Windows.Forms.ToolStripMenuItem contextMenuMarks4Item;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private CustomPanel panelLinesNumbers;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem expandAllByDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton btnErrorDiagnosis;
        private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label lbTaskList;
        private System.Windows.Forms.ToolStripMenuItem sortItemsAlphabeticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLineNumbersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterByMarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hideAnonymousFunctionsToolStripMenuItem;
		private System.Windows.Forms.Button btnToDoListToggle;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.DataGridView taskListDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
		private System.Windows.Forms.ToolTip toolTip1;
    }
}
