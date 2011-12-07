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
			var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source);

			Directory.CreateDirectory("C:\\outxml");
			//Save actual hierarchy xml
			XmlDocument xml = new XmlDocument() {InnerXml = actualResult.Nodes.Serialize()};
			xml.Save("C:\\outxml\\" + resultName);

			var expectedresultXml = GetEmbeddedText("JsParserTest.UnitTests.Result." + resultName);
			var expectedresult = SerializedEntity.Deserialize<Hierachy<CodeNode>>(expectedresultXml);
			//Save expected hierarchy xml
			xml = new XmlDocument() { InnerXml = expectedresult.Serialize() };
			xml.Save("C:\\outxml\\" + resultName + ".ex");

			Assert.IsTrue(actualResult.Nodes.Equals(expectedresult));
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
		public void Test_FunctionWithPlainObject()
		{
			ProcessTemplate("Test_FunctionWithPlainObject.js", "Test_FunctionWithPlainObject.xml");
		}

		[Test]
		public void Test_JQueryPlugin()
		{
			ProcessTemplate("Test_JQueryPlugin.js", "Test_JQueryPlugin.xml");
		}

		[Test]
		public void Test_JQueryChain()
		{
			ProcessTemplate("Test_JQueryChain.js", "Test_JQueryChain.xml");
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

		[Test]
		public void Test_Functions_In_TryCatch_Statement()
		{
			ProcessTemplate("Test_Functions_In_TryCatch_Statement.js", "Test_Functions_In_TryCatch_Statement.xml");
		}

		[Test]
		public void Test_JQuery_Selectors()
		{
			ProcessTemplate("Test_JQuery_Selectors.js", "Test_JQuery_Selectors.xml");
		}

		[Test]
		public void Test_JsonObject_StringPropNames()
		{
			ProcessTemplate("Test_JsonObject_StringPropNames.js", "Test_JsonObject_StringPropNames.xml");
		}

		[Test]
		public void Test_StringContinuationCharacter()
		{
			ProcessTemplate("Test_StringContinuationCharacter.js", "Test_StringContinuationCharacter.xml");
		}
	}
}
