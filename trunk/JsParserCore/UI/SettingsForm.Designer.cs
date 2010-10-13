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
			this.edExtensions.Location = new System.Drawing.Point(28, 29);
			this.edExtensions.Multiline = true;
			this.edExtensions.Name = "edExtensions";
			this.edExtensions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.edExtensions.Size = new System.Drawing.Size(185, 122);
			this.edExtensions.TabIndex = 1;
			this.edExtensions.WordWrap = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(325, 276);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// chTrackActiveItem
			// 
			this.chTrackActiveItem.AutoSize = true;
			this.chTrackActiveItem.Location = new System.Drawing.Point(16, 169);
			this.chTrackActiveItem.Name = "chTrackActiveItem";
			this.chTrackActiveItem.Size = new System.Drawing.Size(220, 17);
			this.chTrackActiveItem.TabIndex = 3;
			this.chTrackActiveItem.Text = "Automatically track active function in tree";
			this.chTrackActiveItem.UseVisualStyleBackColor = true;
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 311);
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
	}
}