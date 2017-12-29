using System.Collections.Specialized;

namespace JsParser.Core.Parsers
{
    public interface IJavascriptParserSettings
    {
        int MaxParametersLengthInFunctionChain { get; set; }
        int MaxParametersLength { get; set; }

        /// <summary>
        /// Gets or sets flag indicating to skip anonymous functions
        /// </summary>
        bool HideAnonymousFunctions { get; set; }

        bool ScriptStripEnabled { get; set; }

        /// <summary>
        /// List of extensions that will be analyzed by parser.
        /// </summary>
        StringCollection Extensions { get; set; }

        /// <summary>
        /// List of extensions that are considered as for script blocks stripping.
        /// </summary>
        StringCollection ScriptStripExtensions { get; set; }

        bool FixAspNetTags { get; set; }
        StringCollection FixAspNetTagsExtensions { get; set; }
        bool FixRazorSyntax { get; set; }
        StringCollection FixRazorSyntaxExtensions { get; set; }
        StringCollection ToDoKeywords { get; set; }
    }

    /// <summary>
    /// The js parser settings.
    /// </summary>
    public class JavascriptParserSettings : IJavascriptParserSettings
    {
        public int MaxParametersLengthInFunctionChain { get; set; }

        public int MaxParametersLength { get; set; }

        /// <summary>
        /// Gets or sets flag indicating to skip anonymous functions
        /// </summary>
        public bool HideAnonymousFunctions { get; set; }

        public bool ScriptStripEnabled { get; set; }

        /// <summary>
        /// List of extensions that are considered as for script blocks stripping.
        /// </summary>
        public StringCollection ScriptStripExtensions { get; set; }

        public StringCollection Extensions { get; set; }

        public bool FixAspNetTags { get; set; }

        public StringCollection FixAspNetTagsExtensions { get; set; }

        public bool FixRazorSyntax { get; set; }

        public StringCollection FixRazorSyntaxExtensions { get; set; }

        public StringCollection ToDoKeywords { get; set; }

        public JavascriptParserSettings()
        {
            MaxParametersLengthInFunctionChain = 25;
            MaxParametersLength = 25;
            HideAnonymousFunctions = false;
            ScriptStripEnabled = true;
            ScriptStripExtensions = new[] { "htm", "html", "aspx", "asp", "ascx", "master", "cshtml" }.ToStringCollection();
            FixAspNetTags = true;
            FixAspNetTagsExtensions = new[] { "asp", "aspx", "ascx", "master" }.ToStringCollection();
            FixRazorSyntax = true;
            FixRazorSyntaxExtensions = new[] { "cshtml" }.ToStringCollection();
            ToDoKeywords = new[] { "todo:", "to do:" }.ToStringCollection();
            Extensions = new[] { "js" }.ToStringCollection();
        }
    }
}