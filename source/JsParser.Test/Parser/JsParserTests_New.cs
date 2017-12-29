using NUnit.Framework;

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
        public void DebuggerKeyword()
        {
            TestRunner.RunTest("New.DebuggerKeyword.js", "New.DebuggerKeyword.txt");
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

        [Test]
        public void TodoList()
        {
            var result = TestRunner.RunTest("New.TodoList.js", "New.TodoList.txt");

            Assert.AreEqual(2, result.TaskList.Count);
            Assert.AreEqual("TODO: write some code here", result.TaskList[0].Description);
            Assert.AreEqual(6, result.TaskList[0].StartLine);

            Assert.AreEqual("to do: Another todo line", result.TaskList[1].Description);
            Assert.AreEqual(9, result.TaskList[1].StartLine);
        }

        [Test]
        public void Es6_GetSet()
        {
            TestRunner.RunTest("New.Es6_GetSet.js", "New.Es6_GetSet.txt");
        }

        [Test]
        public void Es6_GetSet2()
        {
            TestRunner.RunTest("New.Es6_GetSet2.js", "New.Es6_GetSet2.txt");
        }
    }
}
