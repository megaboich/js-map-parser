using System;
using System.Collections.Generic;
using System.Linq;
using JsParser.Core.Code;
using JsParser.Core.Helpers;
using System.Reflection;
using System.Diagnostics;

namespace JsParser.Core.Parsers
{
    /// <summary>
    /// The js parser settings.
    /// </summary>
    public class JavascriptParserSettings
    {
        public int MaxParametersLengthInFunctionChain { get; set; }

        public int MaxParametersLength { get; set; }

        /// <summary>
        /// Gets or sets flag indicating to skip anonymous functions
        /// </summary>
        public bool SkipAnonymousFuntions { get; set; }

        public string Filename { get; set; }

        public bool ScriptStripEnabled { get; set; }

        /// <summary>
        /// List of extensions that are considered as for script blocks stripping.
        /// </summary>
        public string[] ScriptStripExtensions { get; set; }

        public bool FixAspNetTags { get; set; }

        public string[] FixAspNetTagsExtensions { get; set; }

        public bool FixRazorSyntax { get; set; }

        public string[] FixRazorSyntaxExtensions { get; set; }

        public string[] ToDoKeyWords { get; set; }

        public bool IgnoreDebuggerKeyword { get; set; }

        public JavascriptParserSettings()
        {
            MaxParametersLengthInFunctionChain = 25;
            MaxParametersLength = 25;
            SkipAnonymousFuntions = false;
            Filename = "fakefilename.html";
            ScriptStripEnabled = true;
            ScriptStripExtensions = new[] { "htm", "html", "aspx", "asp", "ascx", "master", "cshtml" };
            FixAspNetTags = true;
            FixAspNetTagsExtensions = new[] { "asp", "aspx", "ascx", "master" };
            FixRazorSyntax = true;
            FixRazorSyntaxExtensions = new[] { "cshtml" };
            ToDoKeyWords = new[] { "todo:", "to do:" };
            IgnoreDebuggerKeyword = false;
        }
    }
}