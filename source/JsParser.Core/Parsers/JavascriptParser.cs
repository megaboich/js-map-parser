using System.Linq;
using JsParser.Core.Code;
using System.IO;

namespace JsParser.Core.Parsers
{
    /// <summary>
    /// The JavaScript parser.
    /// </summary>
    public class JavascriptParser : IJavascriptParser
    {
        private readonly IJavascriptParserSettings _settings;

        public JavascriptParser(IJavascriptParserSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Parse JavaScript
        /// </summary>
        public JSParserResult Parse(string code, string filename)
        {
            // Get extension
            var ext = Path.GetExtension(filename).ToLower();
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

            if (_settings.HideAnonymousFunctions)
            {
                NodesPostProcessor.HideAnonymousFunctions(result.Nodes);
            }

            return result;
        }

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="sourceCode">
        /// The JavaScript script.
        /// </param>
        /// <returns>
        /// Hierarchy with code structure.
        /// </returns>
        private JSParserResult ParseInternal(string sourceCode)
        {
            var parser = new JavascriptStructureParserV2(_settings);
            return parser.Parse(sourceCode);
        }
    }
}