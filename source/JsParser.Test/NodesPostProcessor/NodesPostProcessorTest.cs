namespace JsParserTest.NodesPostProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using JsParser.Core.Helpers;
    using JsParser.Core.Code;
    using JsParser.Core.Parsers;
    using JsParserTest.Helpers;

    /// <summary>
    /// Test NodesPostProcessor class.
    /// </summary>
    [TestFixture]
    public class NodesPostProcessorTest
    {
        [Test]
        public void JustHeavyLoadTest_JQuery()
        {
            var source = TestsHelper.GetEmbeddedText("JsParser.Test.NodesPostProcessor.TestsScripts.jquery-1.7.1.js");

            for (var i = 0; i < 10; i++)
            {
                var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source);
                Assert.IsNotNull(actualResult);
            }
        }
    }
}
