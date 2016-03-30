namespace JsParser.UI.UI
{
    partial class ThemeEditor
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtThemeName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cpLineNumbersText = new JsParser.UI.UI.ColorPicker();
            this.cpGridLines = new JsParser.UI.UI.ColorPicker();
            this.cpHighlightInactiveBkground = new JsParser.UI.UI.ColorPicker();
            this.cpHighlightInactiveText = new JsParser.UI.UI.ColorPicker();
            this.cpHighlightBackground = new JsParser.UI.UI.ColorPicker();
            this.cpMenuBackground = new JsParser.UI.UI.ColorPicker();
            this.cpTabText = new JsParser.UI.UI.ColorPicker();
            this.cpWindowText = new JsParser.UI.UI.ColorPicker();
            this.cpWindowBackground = new JsParser.UI.UI.ColorPicker();
            this.cpHighlightText = new JsParser.UI.UI.ColorPicker();
            this.cpControlText = new JsParser.UI.UI.ColorPicker();
            this.cpControlBackground = new JsParser.UI.UI.ColorPicker();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(210, 471);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(291, 471);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Theme Name:";
            // 
            // txtThemeName
            // 
            this.txtThemeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtThemeName.Location = new System.Drawing.Point(93, 10);
            this.txtThemeName.Name = "txtThemeName";
            this.txtThemeName.Size = new System.Drawing.Size(273, 20);
            this.txtThemeName.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(16, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 397);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Colors";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.cpLineNumbersText);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.cpGridLines);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.cpHighlightInactiveBkground);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.cpHighlightInactiveText);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.cpHighlightBackground);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cpMenuBackground);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.cpTabText);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cpWindowText);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cpWindowBackground);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cpHighlightText);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cpControlText);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cpControlBackground);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 372);
            this.panel1.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 334);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Line Numbers Text";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 305);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Grid Lines";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Highlight Inactive Bkground";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 276);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Highlight Inactive Text";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Highlight Background";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Menu Background";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Tab Text";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Window Text";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Window Background";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Highlight Text";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Control Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Control Background";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Location = new System.Drawing.Point(22, 439);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Location = new System.Drawing.Point(103, 439);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cpLineNumbersText
            // 
            this.cpLineNumbersText.Location = new System.Drawing.Point(153, 331);
            this.cpLineNumbersText.Name = "cpLineNumbersText";
            this.cpLineNumbersText.SelectedColor = System.Drawing.Color.Black;
            this.cpLineNumbersText.Size = new System.Drawing.Size(145, 23);
            this.cpLineNumbersText.TabIndex = 23;
            // 
            // cpGridLines
            // 
            this.cpGridLines.Location = new System.Drawing.Point(153, 302);
            this.cpGridLines.Name = "cpGridLines";
            this.cpGridLines.SelectedColor = System.Drawing.Color.Black;
            this.cpGridLines.Size = new System.Drawing.Size(145, 23);
            this.cpGridLines.TabIndex = 21;
            // 
            // cpHighlightInactiveBkground
            // 
            this.cpHighlightInactiveBkground.Location = new System.Drawing.Point(153, 244);
            this.cpHighlightInactiveBkground.Name = "cpHighlightInactiveBkground";
            this.cpHighlightInactiveBkground.SelectedColor = System.Drawing.Color.Black;
            this.cpHighlightInactiveBkground.Size = new System.Drawing.Size(145, 23);
            this.cpHighlightInactiveBkground.TabIndex = 17;
            // 
            // cpHighlightInactiveText
            // 
            this.cpHighlightInactiveText.Location = new System.Drawing.Point(153, 273);
            this.cpHighlightInactiveText.Name = "cpHighlightInactiveText";
            this.cpHighlightInactiveText.SelectedColor = System.Drawing.Color.OrangeRed;
            this.cpHighlightInactiveText.Size = new System.Drawing.Size(145, 23);
            this.cpHighlightInactiveText.TabIndex = 19;
            // 
            // cpHighlightBackground
            // 
            this.cpHighlightBackground.Location = new System.Drawing.Point(153, 186);
            this.cpHighlightBackground.Name = "cpHighlightBackground";
            this.cpHighlightBackground.SelectedColor = System.Drawing.Color.Black;
            this.cpHighlightBackground.Size = new System.Drawing.Size(145, 23);
            this.cpHighlightBackground.TabIndex = 13;
            // 
            // cpMenuBackground
            // 
            this.cpMenuBackground.Location = new System.Drawing.Point(153, 157);
            this.cpMenuBackground.Name = "cpMenuBackground";
            this.cpMenuBackground.SelectedColor = System.Drawing.Color.Black;
            this.cpMenuBackground.Size = new System.Drawing.Size(145, 23);
            this.cpMenuBackground.TabIndex = 11;
            // 
            // cpTabText
            // 
            this.cpTabText.Location = new System.Drawing.Point(153, 128);
            this.cpTabText.Name = "cpTabText";
            this.cpTabText.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.cpTabText.Size = new System.Drawing.Size(145, 23);
            this.cpTabText.TabIndex = 9;
            // 
            // cpWindowText
            // 
            this.cpWindowText.Location = new System.Drawing.Point(153, 99);
            this.cpWindowText.Name = "cpWindowText";
            this.cpWindowText.SelectedColor = System.Drawing.Color.Black;
            this.cpWindowText.Size = new System.Drawing.Size(145, 23);
            this.cpWindowText.TabIndex = 7;
            // 
            // cpWindowBackground
            // 
            this.cpWindowBackground.Location = new System.Drawing.Point(153, 70);
            this.cpWindowBackground.Name = "cpWindowBackground";
            this.cpWindowBackground.SelectedColor = System.Drawing.Color.Black;
            this.cpWindowBackground.Size = new System.Drawing.Size(145, 23);
            this.cpWindowBackground.TabIndex = 5;
            // 
            // cpHighlightText
            // 
            this.cpHighlightText.Location = new System.Drawing.Point(153, 215);
            this.cpHighlightText.Name = "cpHighlightText";
            this.cpHighlightText.SelectedColor = System.Drawing.Color.Black;
            this.cpHighlightText.Size = new System.Drawing.Size(145, 23);
            this.cpHighlightText.TabIndex = 15;
            // 
            // cpControlText
            // 
            this.cpControlText.Location = new System.Drawing.Point(153, 41);
            this.cpControlText.Name = "cpControlText";
            this.cpControlText.SelectedColor = System.Drawing.Color.Black;
            this.cpControlText.Size = new System.Drawing.Size(145, 23);
            this.cpControlText.TabIndex = 3;
            // 
            // cpControlBackground
            // 
            this.cpControlBackground.Location = new System.Drawing.Point(153, 12);
            this.cpControlBackground.Name = "cpControlBackground";
            this.cpControlBackground.SelectedColor = System.Drawing.Color.Black;
            this.cpControlBackground.Size = new System.Drawing.Size(145, 23);
            this.cpControlBackground.TabIndex = 1;
            // 
            // ThemeEditor
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(378, 510);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtThemeName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ThemeEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Theme Editor";
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtThemeName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private ColorPicker cpControlBackground;
        private ColorPicker cpControlText;
        private System.Windows.Forms.Label label3;
        private ColorPicker cpHighlightText;
        private System.Windows.Forms.Label label4;
        private ColorPicker cpHighlightBackground;
        private System.Windows.Forms.Label label9;
        private ColorPicker cpMenuBackground;
        private System.Windows.Forms.Label label8;
        private ColorPicker cpTabText;
        private System.Windows.Forms.Label label7;
        private ColorPicker cpWindowText;
        private System.Windows.Forms.Label label6;
        private ColorPicker cpWindowBackground;
        private System.Windows.Forms.Label label5;
        private ColorPicker cpHighlightInactiveBkground;
        private System.Windows.Forms.Label label10;
        private ColorPicker cpHighlightInactiveText;
        private System.Windows.Forms.Label label11;
        private ColorPicker cpGridLines;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private ColorPicker cpLineNumbersText;
        private System.Windows.Forms.Label label13;
    }
}