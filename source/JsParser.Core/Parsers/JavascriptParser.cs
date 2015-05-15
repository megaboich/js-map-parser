using System.Linq;
using JsParser.Core.Code;
using System.IO;

namespace JsParser.Core.Parsers
{
    /// <summary>
    /// The js parser.
    /// </summary>
    public class JavascriptParser : IJavascriptParser
    {
        private CommentsAgregator _comments;
        private JavascriptParserSettings _settings;

        public JavascriptParser(JavascriptParserSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Parse javascript
        /// </summary>
        /// <param name="code">string with javascript code</param>
        /// <returns></returns>
        public JSParserResult Parse(string code)
        {
            // Get extension
            var ext = Path.GetExtension(_settings.Filename).ToLower();
            if (ext.StartsWith("."))
            {
                ext = ext.Substring(1);
            }

            code = CodeTransformer.ApplyJSParserSkip(code);
            
            code = CodeTransformer.FixStringScriptBlocks(code);

            if (_settings.FixAspNetTags && _settings.FixAspNetTagsExtensions.Contains(ext))
            {
                code = CodeTransformer.KillAspNetTags(code);
            }
            if (_settings.FixRazorSyntax && _settings.FixRazorSyntaxExtensions.Contains(ext))
            {
                code = CodeTransformer.FixRazorSyntax(code);
            }

            if (_settings.ScriptStripEnabled && _settings.ScriptStripExtensions.Contains(ext))
            {
                var foundScriptBlocks = CodeTransformer.ExtractJsFromSource(ref code);

                if (!foundScriptBlocks) //empty file
                {
                    return ParseInternal(string.Empty);
                }
            }
            
            var result = ParseInternal(code);

            NodesPostProcessor.HideAnonymousFunctions(result.Nodes, _settings);

            return result;
        }

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="script">
        /// The js script.
        /// </param>
        /// <returns>
        /// Hierarhy with code structure.
        /// </returns>
        private JSParserResult ParseInternal(string sourceCode)
        {
            var parser = new JavascriptStructureParserV2(_settings);
            return parser.Parse(sourceCode);
        }
    }
}