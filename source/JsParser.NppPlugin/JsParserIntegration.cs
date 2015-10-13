using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JsParser.Core.Infrastructure;
using JsParser.Core.Parsers;
using JsParser.UI.Properties;

namespace NppPluginNET
{
    internal class JsParserIntegration
    {
        private frmParserUiContainer _frmParserUiContainer = null;
        private JsParser.Core.Infrastructure.JsParserService _jsParserService = new JsParserService(Settings.Default);

        public void InitUi(frmParserUiContainer uiContainer)
        {
            if (_frmParserUiContainer == null)
            {
                _frmParserUiContainer = uiContainer;
            }
        }

        public void UpdateTree(string fileName, bool ignoreCache = false)
        {
            var codeProvider = new NppCodeProvider(fileName);

            var result = _jsParserService.Process(codeProvider, ignoreCache);
            if (result == null)
            {
                //not JS case
                _jsParserService.InvalidateCash();
                if (_frmParserUiContainer != null)
                {
                    _frmParserUiContainer.navigationTreeView1.Clear();
                }

                return;
            }

            if (!result.IsEmpty)
            {
                if (_frmParserUiContainer != null)
                {
                    _frmParserUiContainer.navigationTreeView1.UpdateTree(result, codeProvider);
                }
            }
        }
    }
}
