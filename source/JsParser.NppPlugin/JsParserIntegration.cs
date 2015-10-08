using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JsParser.Core.Infrastructure;
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

        public void Update(string fileName)
        {
            //try
            //{
                var codeProvider = new NppCodeProvider(fileName);

                var result = _jsParserService.Process(codeProvider);

                if (_frmParserUiContainer != null)
                {
                    _frmParserUiContainer.navigationTreeView1.UpdateTree(result, codeProvider);
                }
            //}
            //catch (Exception ex)
            //{
            //    File.AppendAllLines(@"c:\npp_events.log", new[]
            //    {
            //        ex.Message + "\r\n" + ex.StackTrace
            //    });
            //}
        }
    }
}
