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
    public class JsParserTests_Frameworks
    {
        [Test]
        public void Frameworks_JustHeavyLoadTest_JQuery()
        {
            for (var i = 0; i < 10; i++)
            {
                Frameworks_JQuery();
            }
        }

        [Test]
        public void Frameworks_JQuery()
        {
            TestRunner.RunTest("Frameworks.jquery-2.1.4.js", "Frameworks.jquery-2.1.4.txt");
        }

        [Test]
        public void Frameworks_Angular()
        {
            TestRunner.RunTest("Frameworks.angular_v1.4.0-rc.2.js", "Frameworks.angular_v1.4.0-rc.2.txt");
        }

        [Test]
        public void Frameworks_UiBootstrap()
        {
            TestRunner.RunTest("Frameworks.ui-bootstrap-tpls-0.13.0.js", "Frameworks.ui-bootstrap-tpls-0.13.0.txt");
        }

        [Test]
        public void Frameworks_Knockout()
        {
            TestRunner.RunTest("Frameworks.knockout-3.3.0.debug.js", "Frameworks.knockout-3.3.0.debug.txt");
        }

        [Test]
        public void Frameworks_Bootstrap()
        {
            TestRunner.RunTest("Frameworks.bootstrap.js", "Frameworks.bootstrap.txt");
        }

    }
}
