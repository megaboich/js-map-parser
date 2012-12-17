using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using JsParser.UI.UI;
using System.Diagnostics;

namespace JsParser.Package
{
	/// <summary>
	/// Extends a standard dialog functionality for implementing ToolsOptions pages, 
	/// with support for the Visual Studio automation model, Windows Forms, and state 
	/// persistence through the Visual Studio settings mechanism.
	/// </summary>
	[Guid("C5B955B9-6220-4DC9-9B46-C3ECE0D66B69")]
	public class OptionsPageCustom : DialogPage
	{
		#region Fields

		private JsParserSettingsControl _settingsUI;
		private bool _needReinitOnActivate = false;

		#endregion Fields

		#region Properties
		/// <summary>
		/// Gets the window an instance of DialogPage that it uses as its user interface.
		/// </summary>
		/// <devdoc>
		/// The window this dialog page will use for its UI.
		/// This window handle must be constant, so if you are
		/// returning a Windows Forms control you must make sure
		/// it does not recreate its handle.  If the window object
		/// implements IComponent it will be sited by the 
		/// dialog page so it can get access to global services.
		/// </devdoc>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected override IWin32Window Window
		{
			get
			{
				return SettingsUI;
			}
		}

		protected JsParserSettingsControl SettingsUI
		{
			get
			{
				if (_settingsUI == null)
				{
					_settingsUI = new JsParser.UI.UI.JsParserSettingsControl();
					_settingsUI.Location = new Point(0, 0);
					_settingsUI.Dock = DockStyle.Fill;
					_settingsUI.InitSettings();
				}

				return _settingsUI;
			}
		}

		protected override void OnApply(PageApplyEventArgs e)
		{
			SettingsUI.SaveSettings();
			_needReinitOnActivate = true;
			base.OnApply(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			SettingsUI.InitSettings();
			_needReinitOnActivate = true;
			base.OnClosed(e);
		}

		protected override void OnActivate(CancelEventArgs e)
		{
			if (_needReinitOnActivate)
			{
				SettingsUI.InitSettings();
			}

			_needReinitOnActivate = false;
			base.OnActivate(e);
		}

		protected override void OnDeactivate(CancelEventArgs e)
		{
			base.OnDeactivate(e);
		}

		#endregion Properties
	}
}
