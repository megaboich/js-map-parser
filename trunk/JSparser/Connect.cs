using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;
using JsParserCore.UI;
using System.Diagnostics;
using JsParserCore.Helpers;
using System.Threading;

namespace JSparser
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		/// <summary>
		/// The navigation tree view.
		/// </summary>
		private NavigationTreeView _navigationTreeView;

		private Window _toolWindow;

		private DTE2 _applicationObject;
		private AddIn _addInInstance;

		private DocumentEvents _documentEvents;
		private SolutionEvents _solutionEvents;
		private WindowEvents _windowEvents;

		private bool _initialize = false;

		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
			//Trace.Listeners.Add(new CustomTraceListener());
			Trace.WriteLine(" ------- Start ------- ");
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			if (!_initialize)
			{
				_applicationObject = (DTE2)application;
				//_addInInstance = (AddIn)addInInst;
				_addInInstance = _applicationObject.AddIns.Item(1);

				object []contextGUIDS = new object[] { };
				Commands2 commands = (Commands2)_applicationObject.Commands;

				//Place the command on the tools menu.
				//Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
				Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"];

				try
				{
					//Find the Tools command bar on the MenuBar command bar:
					CommandBarPopup toolsPopup = (CommandBarPopup)((CommandBarPopup)menuBarCommandBar.Controls["View"]).Controls["Other Windows"];
					
					//Add a command to the Commands collection:
					Command command = commands.AddNamedCommand2(_addInInstance,
						"Show",
						"Javascript Parser",
						"Javascript parser addin",
						true,
						629,
						ref contextGUIDS,
						(int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
						(int)vsCommandStyle.vsCommandStylePictAndText,
						vsCommandControlType.vsCommandControlTypeButton
					);

					//Add a control for the command to the tools menu
					if ((command != null) && (toolsPopup != null))
					{
						command.AddControl(toolsPopup.CommandBar, 1);
					}

					//Add a command to the Commands collection:
					Command commandF = commands.AddNamedCommand2(_addInInstance,
						"Find",
						"Javascript Find",
						"Javascript parser addin 'Find' feature",
						true,
						0,
						ref contextGUIDS,
						(int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
						(int)vsCommandStyle.vsCommandStylePictAndText,
						vsCommandControlType.vsCommandControlTypeButton
					);

					//Add hidden control for the command to the tools menu
					if ((commandF != null) && (toolsPopup != null))
					{
						commandF.Bindings = "Text Editor::SHIFT+ALT+J";
						commandF.AddControl(toolsPopup.CommandBar, 2);
					}
				}
				catch (System.ArgumentException)
				{
					//If we are here, then the exception is probably because a command with that name
					//  already exists. If so there is no need to recreate the command and we can 
					//  safely ignore the exception.
				}

				Events events = _applicationObject.Events;
				_documentEvents = events.get_DocumentEvents(null);
				_solutionEvents = events.SolutionEvents;
				_windowEvents = events.get_WindowEvents(null);

				_documentEvents.DocumentClosing += documentEvents_DocumentClosing;
				_documentEvents.DocumentOpened += documentEvents_DocumentOpened;
				_documentEvents.DocumentSaved += documentEvents_DocumentSaved;
				_solutionEvents.Opened += solutionEvents_Opened;
				_windowEvents.WindowActivated += windowEvents_WindowActivated;
				_initialize = true;

				EnsureWindowCreated();
			}
		}

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			Trace.WriteLine("JSParser: QueryStatus " + commandName);
			if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if (commandName == "JSparser.Connect.Show" || commandName == "JSparser.Connect.Find")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
					return;
				}
			}
		}

		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			Trace.WriteLine("JSParser: Exec " + commandName);
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if(commandName == "JSparser.Connect.Show")
				{
					ShowWindow();
					handled = true;
					return;
				}

				if (commandName == "JSparser.Connect.Find")
				{
					ShowWindow();
					_navigationTreeView.Find();
					handled = true;
					return;
				}
			}
		}

		/// <summary>
		/// Show control.
		/// </summary>
		/// <returns>
		/// The show window.
		/// </returns>
		private bool ShowWindow()
		{
			if (EnsureWindowCreated())
			{
				Trace.WriteLine("JSParser: Set toolwindow visible");

				_toolWindow.Linkable = true;
				_toolWindow.IsFloating = false;
				_toolWindow.Visible = true;

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Creates control.
		/// </summary>
		/// <returns>
		/// The ensure window created.
		/// </returns>
		private bool EnsureWindowCreated()
		{
			if (_navigationTreeView != null)
			{
				//Window is created already
				return true;
			}

			try
			{
				//Create window
				object obj = null;
				string guid = "{E2DA09A9-D3E8-43D2-A277-2043A0D17466}";
				var windows2 = (Windows2) _applicationObject.Windows;
				
				try
				{
					var t = typeof(NavigationTreeView);
					_toolWindow = windows2.CreateToolWindow2(_addInInstance, t.Assembly.Location, t.ToString(),
															  "JavaScript Parser", guid, ref obj);
					_toolWindow.Visible = true;
				}
				catch (Exception ex)
				{
					Trace.WriteLine("js addin: Error while creating tool window");
					MessageBox.Show("Error while creating tool window" + Environment.NewLine + ex.Message + Environment.NewLine + ex.Source, "JSParser");
					return false;
				}

				_navigationTreeView = obj as NavigationTreeView;
				if (_navigationTreeView == null || _toolWindow == null)
				{
					MessageBox.Show("Tool window has not created", "JSParser");
					return false;
				}

				try
				{
					var codeProvider = new VS2008CodeProvider(_applicationObject, null);
					_navigationTreeView.Init(codeProvider);
					_navigationTreeView.LoadFunctionList();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error while initializing JS Parser" + Environment.NewLine + ex.Message + Environment.NewLine + ex.Source, "JSParser");
					return false;
				}

				Trace.WriteLine("js addin: EnsureWindowCreated OK");
				return true;
			}
			catch
			{
				return false;
			}
			finally
			{
			}
		}

		/// <summary>
		/// The document events_ document closing.
		/// </summary>
		/// <param name="doc">
		/// The doc.
		/// </param>
		private void documentEvents_DocumentClosing(Document doc)
		{
			Trace.WriteLine("js addin: documentEvents_DocumentSaved");

			if (_navigationTreeView != null)
			{
				Trace.WriteLine("js addin: _navigationTreeView.Clear");
				_navigationTreeView.Clear();
			}
		}

		/// <summary>
		/// The document events_ document saved.
		/// </summary>
		/// <param name="doc">
		/// The doc.
		/// </param>
		private void documentEvents_DocumentSaved(Document doc)
		{
			Trace.WriteLine("js addin: documentEvents_DocumentSaved");

			if (_navigationTreeView != null)
			{
				Trace.WriteLine("js addin: _navigationTreeView.LoadFunctionList");
				_navigationTreeView.LoadFunctionList();
			}
		}

		/// <summary>
		/// The solution events_ opened.
		/// </summary>
		private void solutionEvents_Opened()
		{
			Trace.WriteLine("js addin: solutionEvents_Opened");
		}

		/// <summary>
		/// The window events_ window activated.
		/// </summary>
		/// <param name="gotFocus">
		/// The got focus.
		/// </param>
		/// <param name="lostFocus">
		/// The lost focus.
		/// </param>
		private void windowEvents_WindowActivated(Window gotFocus, Window lostFocus)
		{
			Trace.WriteLine("js addin: windowEvents_WindowActivated");
			if (gotFocus == null
				|| gotFocus.Kind != "Document"
				|| _navigationTreeView == null
				|| gotFocus.Document == null)
			{
				Trace.WriteLine("js addin: windowEvents_WindowActivated early return");
				return;
			}

			try
			{
				var codeProvider = new VS2008CodeProvider(_applicationObject, gotFocus.Document);
				_navigationTreeView.Init(codeProvider);
				_navigationTreeView.LoadFunctionList();
				Trace.WriteLine("js addin: _navigationTreeView.LoadFunctionList");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + ex.Source);
			}
		}

		/// <summary>
		/// On Document Opened event.
		/// </summary>
		/// <param name="document">Document</param>
		public void documentEvents_DocumentOpened(Document document)
		{
			Trace.WriteLine("js addin: documentEvents_DocumentOpened");
		}
	}
}