using NUnit.Framework;
using System.IO;
using System.Reflection;

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

        public static bool CheckEmbeddedRes(string resourceName)
        {
            using (var stream = Assembly.GetAssembly(typeof (TestsHelper)).GetManifestResourceStream(resourceName))
            {
                return stream != null;
            }
        }
    }
}
