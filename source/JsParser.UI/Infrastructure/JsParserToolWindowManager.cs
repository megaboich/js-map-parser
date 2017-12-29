using JsParser.Core.Code;
using JsParser.Core.Infrastructure;
using JsParser.UI.UI;
using System;

namespace JsParser.UI.Infrastructure
{
    public class JsParserToolWindowManager
    {
        string _activeDocFullName;
        JsParserService _jsParserService;
        Func<IJsParserToolWindow> _findWindowDelegate;

        public JsParserToolWindowManager(
            JsParserService jsParserService,
            Func<IJsParserToolWindow> findWindowDelegate)
            
        {
            _jsParserService = jsParserService;
            _findWindowDelegate = findWindowDelegate;
        }

        public string ActiveDocFullName
        {
            get
            {
                return _activeDocFullName;
            }
        }

        public void PerformInitialParsing(NavigationTreeView navTree = null)
        {
            if (navTree == null)
            {
                navTree = _findWindowDelegate().NavigationTreeView;
            }
            if (_jsParserService.Code != null)
            {
                var result = _jsParserService.Process(_jsParserService.Code, skipHashCheck: true);
                if (navTree != null)
                {
                    navTree.UpdateTree(result, _jsParserService.Code);
                }
            }
        }

        public void CallParserForDocument(ICodeProvider codeProvider)
        {
            _activeDocFullName = codeProvider.FullName;

            var result = _jsParserService.Process(codeProvider);

            var toolWindow = _findWindowDelegate();

            if (result == null)
            {
                // Not JS case - need to clean tree
                _jsParserService.InvalidateCash();
                if (toolWindow != null)
                {
                    toolWindow.NavigationTreeView.Clear();
                }

                return;
            }

            if (result.IsEmpty)
            {
                // skip - cached result
                return;
            }

            JsParserEventsBroadcaster.FireActionsForDoc(
                _activeDocFullName,
                new JsParserErrorsNotificationArgs
                {
                    Code = codeProvider,
                    FullFileName = _activeDocFullName,
                    Errors = result.Errors
                });

            if (toolWindow != null)
            {
                toolWindow.NavigationTreeView.UpdateTree(result, codeProvider);
            }
        }
    }
}
