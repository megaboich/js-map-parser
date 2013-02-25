using JsParser.Core.Code;
using JsParser.Core.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JsParser.Core.Infrastructure
{
    public class JsParserService
    {
        private string _loadedCodeHash;
        private string _docName;
        private ISettings _settings;

        public JsParserService(ISettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Gets Code.
        /// </summary>
        public ICodeProvider Code { get; private set; }

        public void InvalidateCash()
        {
            _loadedCodeHash = null;
            _docName = null;
        }

        public JSParserResult Process(ICodeProvider codeProvider, bool skipHashCheck = false)
        {
            Code = codeProvider;
            string docName = Path.Combine(Code.Path, Code.Name);
            
            if (!CheckExt(docName))
            {
                return null;
            }

            var code = Code.LoadCode();
            var hash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.Default.GetBytes(code)));
            if (!skipHashCheck && _loadedCodeHash == hash)
            {
                return new JSParserResult();
            }
            _loadedCodeHash = hash;

            var parserSettings = new JavascriptParserSettings
            {
                MaxParametersLength = _settings.MaxParametersLength,
                MaxParametersLengthInFunctionChain = _settings.MaxParametersLengthInFunctionChain,
                ProcessHierarchy = _settings.HierarchyEnabled,
                SkipAnonymousFuntions = _settings.HideAnonymousFunctions,
                Filename = docName,
                ExtensionsToBeConsideredAsJs = _settings.JSExtensions.OfType<string>().ToArray(),
            };

            var result = (new JavascriptParser(parserSettings)).Parse(code);
            result.FileName = docName;
            return result;
        }

        private bool CheckExt(string fileName)
        {
            if (_settings.Extensions.Count > 0)
            {
                foreach (var ext in _settings.Extensions)
                {
                    if (fileName.ToLower().EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }
    }
}
