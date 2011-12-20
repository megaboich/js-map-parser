using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using NUnit.Framework;
using System.Xml;

namespace JsParserTest.Helpers
{
    public static class TestsHelper
    {
        public static string GetEmbeddedText(string resourceName)
        {
            var stream = Assembly.GetAssembly(typeof(TestsHelper)).GetManifestResourceStream(resourceName);
            Assert.IsNotNull(stream);
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static XmlDocument GetEmbeddedXml(string resourceName)
        {
            return new XmlDocument { InnerXml = GetEmbeddedText(resourceName) };
        }
    }
}
