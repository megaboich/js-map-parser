using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JsParser.Core.Code;
using JsParser.Core.Helpers;
using JsParser.Core.Parsers;
using JsParser.Test.Helpers;
using JsParserTest.Helpers;
using NUnit.Framework;

namespace JsParser.Test.Parser
{
    public static class TestRunner
    {
        /// <summary>
        /// The comparer for code node.
        /// </summary>
        public class CodeNodeAssertComparer : Comparer<CodeNode>
        {
            public override int Compare(CodeNode x, CodeNode y)
            {
                Assert.IsNotNull(x);
                Assert.IsNotNull(y);

                Assert.AreEqual(x.StartLine, y.StartLine);
                Assert.AreEqual(x.EndLine, y.EndLine);
                Assert.AreEqual(x.Alias, y.Alias);
                Assert.AreEqual(x.Comment, y.Comment);
                Assert.AreEqual(x.NodeType, y.NodeType);
                
                return 0;
            }
        }

        public static JSParserResult RunTest(string sourceName, string resultName)
        {
            var source = TestsHelper.GetEmbeddedText("JsParser.Test.Parser.Source." + sourceName);
            var settings = new JavascriptParserSettings()
            {
                Filename = sourceName,
            };
            var actualResult = (new JavascriptParser(settings)).Parse(source);

            var outDir = "C:\\js_parser_units_output";
            Directory.CreateDirectory(outDir);

            // Save actual hierarchy xml
            var serialized = SimpleHierarchySerializer.Serialize(actualResult.Nodes);
            File.WriteAllText(outDir + "\\" + resultName, serialized);

            // Load test data
            var resName = "JsParser.Test.Parser.ExpectedResult." + resultName;

            var passed = false;
            if (TestsHelper.CheckEmbeddedRes(resName))
            {
                File.WriteAllText(outDir + "\\" + resultName, SimpleHierarchySerializer.Serialize(actualResult.Nodes));

                var expectedresultSerialized = TestsHelper.GetEmbeddedText(resName);
                var expectedresult = SimpleHierarchySerializer.Deserialize<CodeNode>(expectedresultSerialized);

                // Save expected hierarchy serialized
                File.WriteAllText(outDir + "\\" + resultName + ".ex", expectedresultSerialized);

                if (HierarchyComparer.Compare(actualResult.Nodes, expectedresult, new CodeNodeAssertComparer()))
                {
                    passed = true;
                }
            }

            Assert.IsTrue(passed);

            return actualResult;
        }
    }
}
