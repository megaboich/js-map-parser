using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;
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
        /// Gets or sets Flag indicating that parser will additionally process hierarchy.
        /// </summary>
        public bool ProcessHierarchy { get; set; }

        /// <summary>
        /// Gets or sets flag indicating to skip anonymous functions
        /// </summary>
        public bool SkipAnonymousFuntions { get; set; }

        public string Filename { get; set; }

        /// <summary>
        /// List of extensions that are considered as JS files. No Script blocks will be parsed.
        /// </summary>
        public string[] ExtensionsToBeConsideredAsJs { get; set; }

        public bool FixAspNetTags { get; set; }

        public bool FixRazorSyntax { get; set; }

        public JavascriptParserSettings()
        {
            MaxParametersLengthInFunctionChain = 25;
            MaxParametersLength = 25;
            ProcessHierarchy = true;
            SkipAnonymousFuntions = false;
            Filename = "fakefilename.html";
            ExtensionsToBeConsideredAsJs = new[] { "js" };
            FixAspNetTags = true;
            FixRazorSyntax = true;
        }
    }
}