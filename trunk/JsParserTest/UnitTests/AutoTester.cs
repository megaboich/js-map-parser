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

		private void ProcessTemplate(string sourceName, string resultName)
		{
			var source = GetEmbeddedText("JsParserTest.UnitTests.Source." + sourceName);
			source = CodeTransformer.KillAspNetTags(source);
			var codeChunks = CodeTransformer.ExtractJsFromSource(source);
			var result = (new JavascriptParser()).Parse(codeChunks);

			Directory.CreateDirectory("C:\\outxml");

			XmlDocument xml = new XmlDocument() {InnerXml = result.Serialize()};
			xml.Save("C:\\outxml\\" + resultName);

			var resxml = GetEmbeddedText("JsParserTest.UnitTests.Result." + resultName);

			var expectedresult = SerializedEntity.Deserialize<Hierachy<CodeNode>>(resxml);

			Assert.IsTrue(result.Equals(expectedresult));
		}

		[Test]
		public void Test1()
		{
			ProcessTemplate("Test1.js", "Test1.xml");
		}

		[Test]
		public void Test1_1()
		{
			ProcessTemplate("Test1_1.js", "Test1_1.xml");
		}


		[Test]
		public void Test2()
		{
			ProcessTemplate("Test2.js", "Test2.xml");
		}

		[Test]
		public void Test3()
		{
			ProcessTemplate("Test3.js", "Test3.xml");
		}

		[Test]
		public void Test4()
		{
			ProcessTemplate("Test4.js", "Test4.xml");
		}

		[Test]
		public void Test4_1()
		{
			ProcessTemplate("Test4_1.js", "Test4_1.xml");
		}

		[Test]
		public void Test4_2()
		{
			ProcessTemplate("Test4_2.js", "Test4_2.xml");
		}

		[Test]
		public void Test5()
		{
			ProcessTemplate("Test5.js", "Test5.xml");
		}

		[Test]
		public void Test51()
		{
			ProcessTemplate("Test5_1.js", "Test5_1.xml");
		}

		[Test]
		public void Test6()
		{
			ProcessTemplate("Test6.js", "Test6.xml");
		}

		[Test]
		public void Test7()
		{
			ProcessTemplate("Test7.js", "Test7.xml");
		}

		[Test]
		public void Test_Functions_In_CASE_Statement()
		{
			ProcessTemplate("Test_Functions_In_CASE_Statement.js", "Test_Functions_In_CASE_Statement.xml");
		}

		[Test]
		public void TestHtmlScriptBlocks()
		{
			ProcessTemplate("HtmlScriptBlocks.htm", "HtmlScriptBlocks.xml");
		}

		[Test]
		public void Test_Functions_In_IF_Statement()
		{
			ProcessTemplate("Test_Functions_In_IF_Statement.js", "Test_Functions_In_IF_Statement.xml");
		}
	}
}
