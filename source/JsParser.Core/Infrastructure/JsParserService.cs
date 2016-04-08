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
        private readonly IJavascriptParserSettings _settings;

        public JsParserService(IJavascriptParserSettings settings)
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
            
            var result = (new JavascriptParser(_settings)).Parse(code, docName);
            result.FileName = docName;
            return result;
        }

        private bool CheckExt(string fileName)
        {
            if (_settings.Extensions.Count > 0)
            {
                var currentExt = Path.GetExtension(fileName).SafeTrimStart('.');
                foreach (var ext in _settings.Extensions)
                {
                    if (string.Compare(currentExt, ext, StringComparison.InvariantCultureIgnoreCase) == 0)
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
