namespace JsParserCore.UI
{
    partial class JsParserSettingsControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.edExtensions = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chTrackActiveItem = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.numericUpDownMaxParametersLengthInFunctionChain = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxParametersLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.button13 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.taggedFuncLabel1 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.taggedFuncLabel2 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.taggedFuncLabel3 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.button6 = new System.Windows.Forms.Button();
            this.taggedFuncLabel4 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.taggedFuncLabel5 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button8 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.taggedFuncLabel6 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnForceSendStatistics = new System.Windows.Forms.Button();
            this.chSendStatistics = new System.Windows.Forms.CheckBox();
            this.chCheckForVersionUpdates = new System.Windows.Forms.CheckBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxParametersLengthInFunctionChain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxParametersLength)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(449, 290);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edExtensions);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.chTrackActiveItem);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(431, 257);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // edExtensions
            // 
            this.edExtensions.AcceptsReturn = true;
            this.edExtensions.AcceptsTab = true;
            this.edExtensions.Location = new System.Drawing.Point(17, 37);
            this.edExtensions.Multiline = true;
            this.edExtensions.Name = "edExtensions";
            this.edExtensions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.edExtensions.Size = new System.Drawing.Size(185, 122);
            this.edExtensions.TabIndex = 1;
            this.edExtensions.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Each extension should be on separate line in format .js for example.";
            // 
            // chTrackActiveItem
            // 
            this.chTrackActiveItem.AutoSize = true;
            this.chTrackActiveItem.Location = new System.Drawing.Point(7, 187);
            this.chTrackActiveItem.Name = "chTrackActiveItem";
            this.chTrackActiveItem.Size = new System.Drawing.Size(220, 17);
            this.chTrackActiveItem.TabIndex = 2;
            this.chTrackActiveItem.Text = "Automatically track active function in tree";
            this.chTrackActiveItem.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(377, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File extensions for analysing (If list is empty then all file types will be proce" +
    "ssed):";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.numericUpDownMaxParametersLengthInFunctionChain);
            this.tabPage2.Controls.Add(this.numericUpDownMaxParametersLength);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(441, 264);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "UI";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMaxParametersLengthInFunctionChain
            // 
            this.numericUpDownMaxParametersLengthInFunctionChain.Location = new System.Drawing.Point(229, 214);
            this.numericUpDownMaxParametersLengthInFunctionChain.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownMaxParametersLengthInFunctionChain.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxParametersLengthInFunctionChain.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMaxParametersLengthInFunctionChain.Name = "numericUpDownMaxParametersLengthInFunctionChain";
            this.numericUpDownMaxParametersLengthInFunctionChain.Size = new System.Drawing.Size(44, 20);
            this.numericUpDownMaxParametersLengthInFunctionChain.TabIndex = 28;
            this.numericUpDownMaxParametersLengthInFunctionChain.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericUpDownMaxParametersLength
            // 
            this.numericUpDownMaxParametersLength.Location = new System.Drawing.Point(229, 191);
            this.numericUpDownMaxParametersLength.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownMaxParametersLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxParametersLength.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMaxParametersLength.Name = "numericUpDownMaxParametersLength";
            this.numericUpDownMaxParametersLength.Size = new System.Drawing.Size(44, 20);
            this.numericUpDownMaxParametersLength.TabIndex = 27;
            this.numericUpDownMaxParametersLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Max. Parameters Length In Function Chain:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 193);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Max. Parameters Length:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox5);
            this.groupBox1.Controls.Add(this.button13);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button14);
            this.groupBox1.Controls.Add(this.taggedFuncLabel1);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.taggedFuncLabel2);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.taggedFuncLabel3);
            this.groupBox1.Controls.Add(this.pictureBox6);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.taggedFuncLabel4);
            this.groupBox1.Controls.Add(this.pictureBox4);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.taggedFuncLabel5);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.taggedFuncLabel6);
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(329, 176);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text style of marked items in tree";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::JsParserCore.Properties.Resources.flag_white;
            this.pictureBox5.Location = new System.Drawing.Point(12, 26);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(18, 20);
            this.pictureBox5.TabIndex = 16;
            this.pictureBox5.TabStop = false;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(252, 141);
            this.button13.Margin = new System.Windows.Forms.Padding(2);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(56, 19);
            this.button13.TabIndex = 23;
            this.button13.Tag = "6";
            this.button13.Text = "Color";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(184, 24);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 19);
            this.button3.TabIndex = 1;
            this.button3.Tag = "1";
            this.button3.Text = "Font";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(252, 118);
            this.button14.Margin = new System.Windows.Forms.Padding(2);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(56, 19);
            this.button14.TabIndex = 22;
            this.button14.Tag = "5";
            this.button14.Text = "Color";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // taggedFuncLabel1
            // 
            this.taggedFuncLabel1.AutoSize = true;
            this.taggedFuncLabel1.Location = new System.Drawing.Point(34, 26);
            this.taggedFuncLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taggedFuncLabel1.Name = "taggedFuncLabel1";
            this.taggedFuncLabel1.Size = new System.Drawing.Size(87, 13);
            this.taggedFuncLabel1.TabIndex = 0;
            this.taggedFuncLabel1.Text = "taggedFunction()";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(252, 94);
            this.button11.Margin = new System.Windows.Forms.Padding(2);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(56, 19);
            this.button11.TabIndex = 21;
            this.button11.Tag = "4";
            this.button11.Text = "Color";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(184, 47);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(56, 19);
            this.button4.TabIndex = 3;
            this.button4.Tag = "2";
            this.button4.Text = "Font";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(252, 71);
            this.button12.Margin = new System.Windows.Forms.Padding(2);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(56, 19);
            this.button12.TabIndex = 20;
            this.button12.Tag = "3";
            this.button12.Text = "Color";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // taggedFuncLabel2
            // 
            this.taggedFuncLabel2.AutoSize = true;
            this.taggedFuncLabel2.Location = new System.Drawing.Point(34, 50);
            this.taggedFuncLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taggedFuncLabel2.Name = "taggedFuncLabel2";
            this.taggedFuncLabel2.Size = new System.Drawing.Size(87, 13);
            this.taggedFuncLabel2.TabIndex = 2;
            this.taggedFuncLabel2.Text = "taggedFunction()";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(252, 47);
            this.button10.Margin = new System.Windows.Forms.Padding(2);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(56, 19);
            this.button10.TabIndex = 19;
            this.button10.Tag = "2";
            this.button10.Text = "Color";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(184, 71);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(56, 19);
            this.button5.TabIndex = 5;
            this.button5.Tag = "3";
            this.button5.Text = "Font";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(252, 24);
            this.button9.Margin = new System.Windows.Forms.Padding(2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(56, 19);
            this.button9.TabIndex = 18;
            this.button9.Tag = "1";
            this.button9.Text = "Color";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // taggedFuncLabel3
            // 
            this.taggedFuncLabel3.AutoSize = true;
            this.taggedFuncLabel3.Location = new System.Drawing.Point(34, 73);
            this.taggedFuncLabel3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taggedFuncLabel3.Name = "taggedFuncLabel3";
            this.taggedFuncLabel3.Size = new System.Drawing.Size(87, 13);
            this.taggedFuncLabel3.TabIndex = 4;
            this.taggedFuncLabel3.Text = "taggedFunction()";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::JsParserCore.Properties.Resources.icon_favourites;
            this.pictureBox6.Location = new System.Drawing.Point(12, 144);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(18, 20);
            this.pictureBox6.TabIndex = 17;
            this.pictureBox6.TabStop = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(184, 94);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(56, 19);
            this.button6.TabIndex = 7;
            this.button6.Tag = "4";
            this.button6.Text = "Font";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // taggedFuncLabel4
            // 
            this.taggedFuncLabel4.AutoSize = true;
            this.taggedFuncLabel4.Location = new System.Drawing.Point(34, 97);
            this.taggedFuncLabel4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taggedFuncLabel4.Name = "taggedFuncLabel4";
            this.taggedFuncLabel4.Size = new System.Drawing.Size(87, 13);
            this.taggedFuncLabel4.TabIndex = 6;
            this.taggedFuncLabel4.Text = "taggedFunction()";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::JsParserCore.Properties.Resources.flag_orange;
            this.pictureBox4.Location = new System.Drawing.Point(12, 97);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(18, 20);
            this.pictureBox4.TabIndex = 15;
            this.pictureBox4.TabStop = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(184, 118);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(56, 19);
            this.button7.TabIndex = 9;
            this.button7.Tag = "5";
            this.button7.Text = "Font";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::JsParserCore.Properties.Resources.flag_red;
            this.pictureBox3.Location = new System.Drawing.Point(12, 120);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(18, 20);
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // taggedFuncLabel5
            // 
            this.taggedFuncLabel5.AutoSize = true;
            this.taggedFuncLabel5.Location = new System.Drawing.Point(34, 120);
            this.taggedFuncLabel5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taggedFuncLabel5.Name = "taggedFuncLabel5";
            this.taggedFuncLabel5.Size = new System.Drawing.Size(87, 13);
            this.taggedFuncLabel5.TabIndex = 8;
            this.taggedFuncLabel5.Text = "taggedFunction()";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::JsParserCore.Properties.Resources.flag_green;
            this.pictureBox2.Location = new System.Drawing.Point(12, 73);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(18, 20);
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(184, 141);
            this.button8.Margin = new System.Windows.Forms.Padding(2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(56, 19);
            this.button8.TabIndex = 11;
            this.button8.Tag = "6";
            this.button8.Text = "Font";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::JsParserCore.Properties.Resources.flag_blue;
            this.pictureBox1.Location = new System.Drawing.Point(12, 50);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 20);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // taggedFuncLabel6
            // 
            this.taggedFuncLabel6.AutoSize = true;
            this.taggedFuncLabel6.Location = new System.Drawing.Point(34, 144);
            this.taggedFuncLabel6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taggedFuncLabel6.Name = "taggedFuncLabel6";
            this.taggedFuncLabel6.Size = new System.Drawing.Size(87, 13);
            this.taggedFuncLabel6.TabIndex = 10;
            this.taggedFuncLabel6.Text = "taggedFunction()";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnForceSendStatistics);
            this.tabPage3.Controls.Add(this.chSendStatistics);
            this.tabPage3.Controls.Add(this.chCheckForVersionUpdates);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(431, 257);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Network";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnForceSendStatistics
            // 
            this.btnForceSendStatistics.Location = new System.Drawing.Point(35, 69);
            this.btnForceSendStatistics.Name = "btnForceSendStatistics";
            this.btnForceSendStatistics.Size = new System.Drawing.Size(75, 23);
            this.btnForceSendStatistics.TabIndex = 2;
            this.btnForceSendStatistics.Text = "Send now";
            this.btnForceSendStatistics.UseVisualStyleBackColor = true;
            this.btnForceSendStatistics.Click += new System.EventHandler(this.btnForceSendStatistics_Click);
            // 
            // chSendStatistics
            // 
            this.chSendStatistics.AutoSize = true;
            this.chSendStatistics.Location = new System.Drawing.Point(21, 46);
            this.chSendStatistics.Name = "chSendStatistics";
            this.chSendStatistics.Size = new System.Drawing.Size(250, 17);
            this.chSendStatistics.TabIndex = 1;
            this.chSendStatistics.Text = "Send anonymous usage statistics. (Once a day)";
            this.chSendStatistics.UseVisualStyleBackColor = true;
            // 
            // chCheckForVersionUpdates
            // 
            this.chCheckForVersionUpdates.AutoSize = true;
            this.chCheckForVersionUpdates.Location = new System.Drawing.Point(21, 22);
            this.chCheckForVersionUpdates.Name = "chCheckForVersionUpdates";
            this.chCheckForVersionUpdates.Size = new System.Drawing.Size(217, 17);
            this.chCheckForVersionUpdates.TabIndex = 0;
            this.chCheckForVersionUpdates.Text = "Check for version updates automatically.";
            this.chCheckForVersionUpdates.UseVisualStyleBackColor = true;
            // 
            // JsParserSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "JsParserSettingsControl";
            this.Size = new System.Drawing.Size(449, 290);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxParametersLengthInFunctionChain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxParametersLength)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox edExtensions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chTrackActiveItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxParametersLengthInFunctionChain;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxParametersLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Label taggedFuncLabel1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Label taggedFuncLabel2;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label taggedFuncLabel3;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label taggedFuncLabel4;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label taggedFuncLabel5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label taggedFuncLabel6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnForceSendStatistics;
        private System.Windows.Forms.CheckBox chSendStatistics;
        private System.Windows.Forms.CheckBox chCheckForVersionUpdates;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
