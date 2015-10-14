using System;
using System.Windows.Forms;

namespace NppPluginNET
{
    partial class frmParserUiContainer : Form
    {
        public frmParserUiContainer()
        {
            InitializeComponent();
        }

        void FormVisibleChanged(object sender, EventArgs e)
        {
        	if (!Visible)
        	{
                PluginBase.UncheckMenuItem(PluginBase.idMenuItemParserUi);
        	    PluginBase.Settings.ShowToolWindow = false;
        	}
        }
    }
}
