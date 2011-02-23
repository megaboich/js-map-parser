namespace JsParserCore.UI
{
	partial class SettingsForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.edExtensions = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.chTrackActiveItem = new System.Windows.Forms.CheckBox();
			this.chShowHideAutomatically = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(377, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "File extensions for analysing (If list is empty then all file types will be proce" +
				"ssed):";
			// 
			// edExtensions
			// 
			this.edExtensions.Location = new System.Drawing.Point(28, 50);
			this.edExtensions.Multiline = true;
			this.edExtensions.Name = "edExtensions";
			this.edExtensions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.edExtensions.Size = new System.Drawing.Size(185, 122);
			this.edExtensions.TabIndex = 1;
			this.edExtensions.WordWrap = false;
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(247, 276);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// chTrackActiveItem
			// 
			this.chTrackActiveItem.AutoSize = true;
			this.chTrackActiveItem.Location = new System.Drawing.Point(16, 197);
			this.chTrackActiveItem.Name = "chTrackActiveItem";
			this.chTrackActiveItem.Size = new System.Drawing.Size(220, 17);
			this.chTrackActiveItem.TabIndex = 2;
			this.chTrackActiveItem.Text = "Automatically track active function in tree";
			this.chTrackActiveItem.UseVisualStyleBackColor = true;
			// 
			// chShowHideAutomatically
			// 
			this.chShowHideAutomatically.AutoSize = true;
			this.chShowHideAutomatically.Location = new System.Drawing.Point(16, 220);
			this.chShowHideAutomatically.Name = "chShowHideAutomatically";
			this.chShowHideAutomatically.Size = new System.Drawing.Size(249, 17);
			this.chShowHideAutomatically.TabIndex = 3;
			this.chShowHideAutomatically.Text = "Automatically Show/Hide js-parser tool window.";
			this.chShowHideAutomatically.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(327, 276);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(323, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Each extension should be on separate line in format .js for example.";
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(414, 311);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.chShowHideAutomatically);
			this.Controls.Add(this.chTrackActiveItem);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.edExtensions);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Javascript Parser Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox edExtensions;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chTrackActiveItem;
		private System.Windows.Forms.CheckBox chShowHideAutomatically;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label2;
	}
}