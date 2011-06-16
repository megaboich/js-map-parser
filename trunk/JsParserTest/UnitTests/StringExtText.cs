using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using JsParserCore.Parsers;
using System.Diagnostics;

namespace JsParserTest.UnitTests
{
    [TestFixture]
    public class StringExtText
    {
        [Test]
        public void TestShortenize()
        {
            RunSample("string", 6, 6);
            RunSample("string", 5, 5);
            RunSample("string", 4, 4);
            RunSample("string", 3, 3);
            RunSample("string", 2, 2);
            RunSample("string", 1, 1);
            RunSample("string", 0, 0);
            RunSample("string", -1, 0);
        }

        private void RunSample(string s, int targetLen, int expectedLen)
        {
            var r = s.Shortenize(targetLen);
            Trace.WriteLine(s + " => " + r);
            Assert.AreEqual(expectedLen, r.Length);
        }
    }
}
