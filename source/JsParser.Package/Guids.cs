// Guids.cs
// MUST match guids.h
using System;

namespace JsParser.Package
{
    static class GuidList
    {
        public const string guidJsParserPackagePkgString = "85fcde11-d0ea-4b3e-b03a-79b16c2379f7";
        public const string guidJsParserPackageCmdSetString = "a770d5ac-aede-4255-84a2-c3fc9d3e8e96";
        public const string guidToolWindowPersistanceString = "5c1947b9-a2ea-42cd-8299-f2603a9c033d";
        public const string guidJsParserTreeToolWindow = "8AB2BB0C-DF78-4967-A6D6-91FC9E847125";

        public static readonly Guid guidJsParserPackageCmdSet = new Guid(guidJsParserPackageCmdSetString);
    };
}