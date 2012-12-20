using JsParser.Core.Code;
using JsParser.Core.Parsers;
using JsParser.UI.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JsParser.UI.Services
{
    public class JsParserService
    {
        private string _loadedCodeHash;
        private string _docName;

        /// <summary>
        /// Settings instance
        /// </summary>
        public static Settings UISettings
        {
            get { return Settings.Default; }
        }

        /// <summary>
        /// Gets Code.
        /// </summary>
        public ICodeProvider Code { get; private set; }


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
                MaxParametersLength = UISettings.MaxParametersLength,
                MaxParametersLengthInFunctionChain = UISettings.MaxParametersLengthInFunctionChain,
                ProcessHierarchy = UISettings.HierarchyEnabled,
                SkipAnonymousFuntions = UISettings.HideAnonymousFunctions
            };

            var result = (new JavascriptParser(parserSettings)).Parse(code);
            result.FileName = docName;
            return result;
        }

        private bool CheckExt(string fileName)
        {
            if (UISettings.Extensions.Count > 0)
            {
                foreach (var ext in UISettings.Extensions)
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
