namespace NppPluginNET
{
    partial class frmParserUiContainer
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
            this.navigationTreeView1 = new JsParser.UI.UI.NavigationTreeView();
            this.SuspendLayout();
            // 
            // navigationTreeView1
            // 
            this.navigationTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationTreeView1.Location = new System.Drawing.Point(0, 0);
            this.navigationTreeView1.Name = "navigationTreeView1";
            this.navigationTreeView1.Size = new System.Drawing.Size(197, 420);
            this.navigationTreeView1.TabIndex = 0;
            // 
            // frmParserUiContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 420);
            this.Controls.Add(this.navigationTreeView1);
            this.Name = "frmParserUiContainer";
            this.Text = "NppDockableForm";
            this.VisibleChanged += new System.EventHandler(this.FrmGoToLineVisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private JsParser.UI.UI.NavigationTreeView navigationTreeView1;






    }
}