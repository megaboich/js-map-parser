using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NUnit.Framework;
using JsParserCore.Parsers;
using JsParserCore.Helpers;
using JsParserCore.Code;

namespace UnitTests
{
	[TestFixture]
	public class AutoTester
	{
		public static string GetEmbeddedText(string resourceName)
		{
			var stream = Assembly.GetAssembly(typeof(AutoTester)).GetManifestResourceStream(resourceName);
			Assert.IsNotNull(stream);
			using (var sr = new StreamReader(stream))
			{
				return sr.ReadToEnd();
			}
		}

		public XmlDocument GetEmbeddedXml(string resourceName)
		{
			return new XmlDocument { InnerXml = GetEmbeddedText(resourceName) };
		}

		private void ProcessTemplate(string name)
		{
			var source = GetEmbeddedText("JsParserTest.UnitTests.Source." + name + ".js");
			source = CodeTransformer.KillAspNetTags(source);
			var codeChunks = CodeTransformer.ExtractJsFromSource(source);
			var result = (new JavascriptParser()).Parse(codeChunks);

			Directory.CreateDirectory("C:\\outxml");

			XmlDocument xml = new XmlDocument() {InnerXml = result.Serialize()};
			xml.Save("C:\\outxml\\" + name + ".xml");

			var resxml = GetEmbeddedText("JsParserTest.UnitTests.Result." + name + ".xml");

			var expectedresult = SerializedEntity.Deserialize<Hierachy<CodeNode>>(resxml);

			Assert.IsTrue(result.Equals(expectedresult));
		}

		[Test]
		public void Test1()
		{
			ProcessTemplate("Test1");
		}

		[Test]
		public void Test1_1()
		{
			ProcessTemplate("Test1_1");
		}


		[Test]
		public void Test2()
		{
			ProcessTemplate("Test2");
		}

		[Test]
		public void Test3()
		{
			ProcessTemplate("Test3");
		}

		[Test]
		public void Test4()
		{
			ProcessTemplate("Test4");
		}

		[Test]
		public void Test4_1()
		{
			ProcessTemplate("Test4_1");
		}

		[Test]
		public void Test4_2()
		{
			ProcessTemplate("Test4_2");
		}

		[Test]
		public void Test5()
		{
			ProcessTemplate("Test5");
		}

		[Test]
		public void Test51()
		{
			ProcessTemplate("Test5_1");
		}

		[Test]
		public void Test6()
		{
			ProcessTemplate("Test6");
		}

		[Test]
		public void Test7()
		{
			ProcessTemplate("Test7");
		}

		[Test]
		public void Test8()
		{
			ProcessTemplate("Test8");
		}
	}
}
