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

namespace UnitTests
{
	[TestFixture]
	public class AutoTester
	{
		private void ProcessTemplate(string sourceName, string resultName)
		{
			var source = TestsHelper.GetEmbeddedText("JsParser.Test.UnitTests.Source." + sourceName);
			var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source);

			Directory.CreateDirectory("C:\\outxml");
			//Save actual hierarchy xml
			XmlDocument xml = new XmlDocument() {InnerXml = actualResult.Nodes.Serialize()};
			xml.Save("C:\\outxml\\" + resultName);

			var expectedresultXml = TestsHelper.GetEmbeddedText("JsParser.Test.UnitTests.Result." + resultName);
			var expectedresult = SerializedEntity.Deserialize<Hierachy<CodeNode>>(expectedresultXml);
			//Save expected hierarchy xml
			File.WriteAllText("C:\\outxml\\" + resultName + ".ex", expectedresultXml);

			Assert.IsTrue(actualResult.Nodes.Equals(expectedresult));
		}

		[Test]
		public void Test1()
		{
			ProcessTemplate("Test1.js", "Test1.xml");
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
        public void TestHtmlScriptBlocks_HeavyLoad()
        {
            //var source = TestsHelper.GetEmbeddedText("JsParser.Test.UnitTests.Source.HtmlScriptBlocks.htm");
            
            //for (int o = 0; o < 10000; o++)
            //{
            //    var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source);
            //    Assert.Greater(actualResult.Nodes.Childrens.Count, 0);
            //}
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

		[Test]
		public void Test_Construct_Object_In_Return_Statement()
		{
			ProcessTemplate("Test_Construct_Object_In_Return_Statement.js", "Test_Construct_Object_In_Return_Statement.xml");
		}

		[Test]
		public void Test_Anonimous_In_Return_Statement()
		{
			ProcessTemplate("Test_Anonimous_In_Return_Statement.js", "Test_Anonimous_In_Return_Statement.xml");
		}

		[Test]
		public void Test_DoubleAssign()
		{
			ProcessTemplate("Test_DoubleAssign.js", "Test_DoubleAssign.xml");
		}

		[Test]
		public void Test_AnonymousSelfExecBlock()
		{
			ProcessTemplate("Test_AnonymousSelfExecBlock.js", "Test_AnonymousSelfExecBlock.xml");
		}

		[Test]
		public void Test_ReservedWords_Goto()
		{
			ProcessTemplate("Test_ReservedWords_Goto.js", "Test_ReservedWords_Goto.xml");
		}

		[Test]
		public void Test_NewStatements()
		{
			ProcessTemplate("Test_NewStatements.js", "Test_NewStatements.xml");
		}

		[Test]
		public void Test_StringScriptBlock()
		{
			ProcessTemplate("Test_StringScriptBlock.js", "Test_StringScriptBlock.xml");
		}

		[Test]
		public void Test_AspTagsReplace()
		{
			ProcessTemplate("Test_AspTagsReplace.aspx", "Test_AspTagsReplace.xml");
		}
	}
}
