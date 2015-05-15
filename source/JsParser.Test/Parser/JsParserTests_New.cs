using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NUnit.Framework;
using JsParser.Core.Parsers;
using JsParser.Core.Helpers;
using JsParser.Core.Code;
using JsParserTest.Helpers;
using JsParser.Test.Helpers;

namespace JsParser.Test.Parser
{
    [TestFixture]
    public class JsParserTests_New
    {
        [Test]
        public void ArrayChain()
        {
            TestRunner.RunTest("New.ArrayChain.js", "New.ArrayChain.txt");
        }

        [Test]
        public void Assignment()
        {
            TestRunner.RunTest("New.Assignment.js", "New.Assignment.txt");
        }

        [Test]
        public void CallbackChain()
        {
            TestRunner.RunTest("New.CallbackChain.js", "New.CallbackChain.txt");
        }

        [Test]
        public void Comments()
        {
            TestRunner.RunTest("New.Comments.js", "New.Comments.txt");
        }

        [Test]
        public void FunctionsHierarchy()
        {
            TestRunner.RunTest("New.FunctionsHierarchy.js", "New.FunctionsHierarchy.txt");
        }

        [Test]
        public void FunctionsHierarchy2()
        {
            TestRunner.RunTest("New.FunctionsHierarchy2.js", "New.FunctionsHierarchy2.txt");
        }

        [Test]
        public void InlineOrDecralation()
        {
            TestRunner.RunTest("New.InlineOrDecralation.js", "New.InlineOrDecralation.txt");
        }

        [Test]
        public void ReturnStatement()
        {
            TestRunner.RunTest("New.ReturnStatement.js", "New.ReturnStatement.txt");
        }
    }
}
