using NUnit.Framework;

namespace JsParser.Test.Parser
{
    [TestFixture]
    public class AutoTester
    {
        [Test]
        public void Test1()
        {
            TestRunner.RunTest("Test1.js", "Test1.txt");
        }

        [Test]
        public void Test2()
        {
            TestRunner.RunTest("Test2.js", "Test2.txt");
        }

        [Test]
        public void Test3()
        {
            TestRunner.RunTest("Test3.js", "Test3.txt");
        }

        [Test]
        public void Test4()
        {
            TestRunner.RunTest("Test4.js", "Test4.txt");
        }

        [Test]
        public void Test4_2()
        {
            TestRunner.RunTest("Test4_2.js", "Test4_2.txt");
        }

        [Test]
        public void Test5()
        {
            TestRunner.RunTest("Test5.js", "Test5.txt");
        }

        [Test]
        public void Test51()
        {
            TestRunner.RunTest("Test5_1.js", "Test5_1.txt");
        }

        [Test]
        public void Test_FunctionWithPlainObject()
        {
            TestRunner.RunTest("Test_FunctionWithPlainObject.js", "Test_FunctionWithPlainObject.txt");
        }

        [Test]
        public void Test_Functions_In_CASE_Statement()
        {
            TestRunner.RunTest("Test_Functions_In_CASE_Statement.js", "Test_Functions_In_CASE_Statement.txt");
        }

        [Test]
        public void HtmlScriptBlocks()
        {
            TestRunner.RunTest("HtmlScriptBlocks.htm", "HtmlScriptBlocks.txt");
        }

        [Test]
        public void Test_Functions_In_IF_Statement()
        {
            TestRunner.RunTest("Test_Functions_In_IF_Statement.js", "Test_Functions_In_IF_Statement.txt");
        }

        [Test]
        public void Test_Functions_In_TryCatch_Statement()
        {
            TestRunner.RunTest("Test_Functions_In_TryCatch_Statement.js", "Test_Functions_In_TryCatch_Statement.txt");
        }

        [Test]
        public void Test_JsonObject_StringPropNames()
        {
            TestRunner.RunTest("Test_JsonObject_StringPropNames.js", "Test_JsonObject_StringPropNames.txt");
        }

        [Test]
        public void Test_StringContinuationCharacter()
        {
            TestRunner.RunTest("Test_StringContinuationCharacter.js", "Test_StringContinuationCharacter.txt");
        }

        [Test]
        public void Test_Construct_Object_In_Return_Statement()
        {
            TestRunner.RunTest("Test_Construct_Object_In_Return_Statement.js", "Test_Construct_Object_In_Return_Statement.txt");
        }

        [Test]
        public void Test_Anonimous_In_Return_Statement()
        {
            TestRunner.RunTest("Test_Anonimous_In_Return_Statement.js", "Test_Anonimous_In_Return_Statement.txt");
        }

        [Test]
        public void Test_DoubleAssign()
        {
            TestRunner.RunTest("Test_DoubleAssign.js", "Test_DoubleAssign.txt");
        }

        [Test]
        public void Test_AnonymousSelfExecBlock()
        {
            TestRunner.RunTest("Test_AnonymousSelfExecBlock.js", "Test_AnonymousSelfExecBlock.txt");
        }

        [Test]
        public void Test_ReservedWords_Goto()
        {
            TestRunner.RunTest("Test_ReservedWords_Goto.js", "Test_ReservedWords_Goto.txt");
        }

        [Test]
        public void Test_NewStatements()
        {
            TestRunner.RunTest("Test_NewStatements.js", "Test_NewStatements.txt");
        }

        [Test]
        public void Test_StringScriptBlock()
        {
            TestRunner.RunTest("Test_StringScriptBlock.js", "Test_StringScriptBlock.txt");
        }

        [Test]
        public void Test_AspTagsReplace()
        {
            var result = TestRunner.RunTest("Test_AspTagsReplace.aspx", "Test_AspTagsReplace.txt");
            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void Test_RazorSyntax()
        {
            var result = TestRunner.RunTest("Test_RazorSyntax.cshtml", "Test_RazorSyntax.txt");
            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void Test_AspScriptCombiner()
        {
            var result = TestRunner.RunTest("Test_AspScriptCombiner.aspx", "Test_AspScriptCombiner.txt");
            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void Test_SkipJSParser()
        {
            var result = TestRunner.RunTest("Test_JSParserSkipHandling.js", "Test_JSParserSkipHandling.txt");
            Assert.AreEqual(0, result.Errors.Count);
        }
    }
}
