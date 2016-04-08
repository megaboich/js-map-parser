using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JsMapParser.NppPlugin.Forms;
using JsMapParser.NppPlugin.Helpers;
using JsMapParser.NppPlugin.NppPluginBaseInfrastructure;
using JsParser.Core.Infrastructure;
using JsParser.UI.Properties;

namespace JsMapParser.NppPlugin
{
    internal class JsMapParserPlugin: NppPluginBase
    {
        private JsMapParserPluginSettings _settings;
        private readonly bool _isDebugMode;
        private int _uiCommandPanelId = 0;
        private PluginUiPanel _uiPanel;
        private readonly JsParserService _jsParserService = new JsParserService(Settings.Default);

        internal override string PluginName
        {
            get { return "JavaScript Map Parser"; }
        }

        public JsMapParserPlugin()
        {
            _isDebugMode = Environment.GetEnvironmentVariable("npp_js_map_parser_addin_debug_mode") == "true";

            OnInit += JsMapParserPlugin_OnInit;
            OnReady += JsMapParserPlugin_OnReady;
            OnFileChanged += JsMapParserPlugin_OnFileChanged;
            OnFileSaved += JsMapParserPlugin_OnFileSaved;
            OnShutDown += JsMapParserPlugin_OnShutDown;
        }

        internal override ICollection<ToolbarButtonDefinition> GetToolbarButtons()
        {
            return new[]
            {
                new ToolbarButtonDefinition() { Id = _uiCommandPanelId, Image = Resources.Resources.jsparsericon }
            };
        }

        internal override ICollection<MenuItemDefinition> GetMenuItems()
        {
            var items = new List<MenuItemDefinition>(new[]
            {
                new MenuItemDefinition() { Id = _uiCommandPanelId, Text = PluginName, Handler = ShowParserUiPanel },
                new MenuItemDefinition() { Id = 1, Text = "---", Handler = null },
                new MenuItemDefinition() { Id = 2, Text = "About", Handler = About },
            });

            if (_isDebugMode)
            {
                items.Add(new MenuItemDefinition() { Id = 3, Text = "Attach debugger", Handler = AttachDebugger });
            }

            return items;
        }

        private void JsMapParserPlugin_OnInit()
        {
            _settings = JsMapParserPluginSettings.Load(GetConfigFolder());
        }

        private void JsMapParserPlugin_OnReady()
        {
            if (_settings.ShowToolWindow)
            {
                ShowParserUiPanel();

                if (_settings.ToolWindowVisible)
                {
                    //Also register timer to show panel once more with delay - so our panel will be likely shown the last
                    Action showThePanel = () =>
                    {
                        if (!_uiPanel.Visible)
                        {
                            ShowPanel(_uiPanel, true);
                        }
                    };
                    TimerHelper.SetTimeOut(showThePanel, TimeSpan.FromMilliseconds(100));
                    TimerHelper.SetTimeOut(showThePanel, TimeSpan.FromMilliseconds(200));
                    TimerHelper.SetTimeOut(showThePanel, TimeSpan.FromMilliseconds(500));
                    TimerHelper.SetTimeOut(showThePanel, TimeSpan.FromMilliseconds(1000));
                    TimerHelper.SetTimeOut(showThePanel, TimeSpan.FromMilliseconds(1500));
                }
            }
        }
        
        private void JsMapParserPlugin_OnShutDown()
        {
            _settings.ToolWindowVisible = _uiPanel != null && _uiPanel.Visible;
            _settings.Save();
        }

        private void About()
        {
            Process.Start("https://github.com/megaboich/js-map-parser");
        }

        private void AttachDebugger()
        {
            Debugger.Launch();
            Debugger.Break();
        }

        private void ShowParserUiPanel()
        {
            if (_uiPanel == null)
            {
                _uiPanel = new PluginUiPanel();
                RegisterPanel(_uiCommandPanelId, Resources.Resources.jsparsericon, _uiPanel);
                ShowPanel(_uiPanel, true);
                CheckMenuItem(_uiCommandPanelId, true);

                _settings.ShowToolWindow = true;

                UpdateTree(GetCurrentFile(), true);
            }
            else
            {
                if (!_uiPanel.Visible)
                {
                    ShowPanel(_uiPanel, true);
                    CheckMenuItem(_uiCommandPanelId, true);

                    _settings.ShowToolWindow = true;

                    UpdateTree(GetCurrentFile(), true);
                }
                else
                {
                    ShowPanel(_uiPanel, false);
                    CheckMenuItem(_uiCommandPanelId, false);

                    _settings.ShowToolWindow = false;
                }
            }
        }

        private void JsMapParserPlugin_OnFileSaved()
        {
            UpdateTree(GetCurrentFile());
        }

        private void JsMapParserPlugin_OnFileChanged()
        {
            UpdateTree(GetCurrentFile());
        }

        private void UpdateTree(string fileName, bool ignoreCache = false)
        {
            var codeProvider = new NppCodeProvider(this, fileName);

            var result = _jsParserService.Process(codeProvider, ignoreCache);
            if (result == null)
            {
                //not JS case
                _jsParserService.InvalidateCash();
                if (_uiPanel != null)
                {
                    _uiPanel.navigationTreeView1.Clear();
                }

                return;
            }

            if (!result.IsEmpty)
            {
                if (_uiPanel != null)
                {
                    _uiPanel.navigationTreeView1.UpdateTree(result, codeProvider);
                }
            }
        }
    }
}