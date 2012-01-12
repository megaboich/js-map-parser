/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using JsParserCore.UI;

namespace JsParser_package
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
					_settingsUI = new JsParserCore.UI.JsParserSettingsControl();
					_settingsUI.Location = new Point(0, 0);
					_settingsUI.InitSettings();
				}

				return _settingsUI;
			}
		}

		protected override void OnApply(PageApplyEventArgs e)
		{
			if (e.ApplyBehavior == ApplyKind.Apply)
			{
				SettingsUI.SaveSettings();
			}
		}

		#endregion Properties
	}
}
