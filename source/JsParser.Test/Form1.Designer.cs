namespace JsParser.Test
{
	partial class Form1
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.navigationTreeView1 = new JsParser.UI.UI.NavigationTreeView();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.txtSource = new System.Windows.Forms.RichTextBox();
			this.tvTests = new System.Windows.Forms.TreeView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.scanDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(16, 33);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.navigationTreeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(1393, 659);
			this.splitContainer1.SplitterDistance = 313;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 2;
			// 
			// navigationTreeView1
			// 
			this.navigationTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.navigationTreeView1.Location = new System.Drawing.Point(0, 0);
			this.navigationTreeView1.Margin = new System.Windows.Forms.Padding(5);
			this.navigationTreeView1.Name = "navigationTreeView1";
			this.navigationTreeView1.Size = new System.Drawing.Size(313, 659);
			this.navigationTreeView1.TabIndex = 1;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.txtSource);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.tvTests);
			this.splitContainer2.Size = new System.Drawing.Size(1075, 659);
			this.splitContainer2.SplitterDistance = 512;
			this.splitContainer2.TabIndex = 4;
			// 
			// txtSource
			// 
			this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSource.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSource.HideSelection = false;
			this.txtSource.Location = new System.Drawing.Point(0, 0);
			this.txtSource.Name = "txtSource";
			this.txtSource.Size = new System.Drawing.Size(512, 659);
			this.txtSource.TabIndex = 0;
			this.txtSource.Text = "";
			this.txtSource.WordWrap = false;
			this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
			// 
			// tvTests
			// 
			this.tvTests.BackColor = System.Drawing.SystemColors.Window;
			this.tvTests.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvTests.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvTests.ForeColor = System.Drawing.SystemColors.WindowText;
			this.tvTests.FullRowSelect = true;
			this.tvTests.HideSelection = false;
			this.tvTests.Location = new System.Drawing.Point(0, 0);
			this.tvTests.Name = "tvTests";
			this.tvTests.ShowRootLines = false;
			this.tvTests.Size = new System.Drawing.Size(559, 659);
			this.tvTests.TabIndex = 4;
			this.tvTests.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTests_AfterSelect);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(1425, 28);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.scanDirectoryToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(183, 24);
			this.openToolStripMenuItem.Text = "Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// scanDirectoryToolStripMenuItem
			// 
			this.scanDirectoryToolStripMenuItem.Name = "scanDirectoryToolStripMenuItem";
			this.scanDirectoryToolStripMenuItem.Size = new System.Drawing.Size(183, 24);
			this.scanDirectoryToolStripMenuItem.Text = "Scan Directory...";
			this.scanDirectoryToolStripMenuItem.Click += new System.EventHandler(this.scanDirectoryToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "JS Files|*.js|All Files|*.*";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1425, 702);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "JS Parser Test Application";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private JsParser.UI.UI.NavigationTreeView navigationTreeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem scanDirectoryToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView tvTests;
		private System.Windows.Forms.RichTextBox txtSource;
	}
}

