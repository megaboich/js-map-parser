// Guids.cs
// MUST match guids.h
using System;

namespace JsParser.VsExtension
{
    static class GuidList
    {
        public const string guidJsParserPackagePkgString = "85fcde11-d0ea-4b3e-b03a-79b16c2379f7";
        public const string guidJsParserPackageCmdSetString = "a770d5ac-aede-4255-84a2-c3fc9d3e8e96";
        public const string guidToolWindowPersistanceString = "5c1947b9-a2ea-42cd-8299-f2603a9c033d";

        public static readonly Guid guidJsParserPackageCmdSet = new Guid(guidJsParserPackageCmdSetString);
    };
}