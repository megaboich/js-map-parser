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

        void FrmGoToLineVisibleChanged(object sender, EventArgs e)
        {
        	if (!Visible)
        	{
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                                  PluginBase._funcItems.Items[PluginBase.idFrmGotToLine]._cmdID, 0);
        	}
        }
    }
}
