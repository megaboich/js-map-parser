namespace JsParserTest.NodesPostProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using JsParserCore.Helpers;
    using JsParserCore.Code;
    using JsParserCore.Parsers;
    using JsParserTest.Helpers;

    /// <summary>
    /// Test NodesPostProcessor class.
    /// </summary>
    [TestFixture]
    public class NodesPostProcessorTest
    {
        [Test]
        public void TestGroupNodesByVariableDeclaration1()
        {
            var testSet = new Hierachy<CodeNode>(new CodeNode { Alias = "root" });
            testSet.Childrens = new List<Hierachy<CodeNode>>();
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "_this", Opcode = "Variable" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "_this.method1()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "_this.method2()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "_this.method3()" }));

            NodesPostProcessor.GroupNodesByVariableDeclaration(testSet);

            Assert.AreEqual(1, testSet.Childrens.Count);
            Assert.AreEqual("_this", testSet.Childrens[0].Item.Alias);
            var subSet = testSet.Childrens[0];
            Assert.AreEqual(3, subSet.Childrens.Count);
            var subItemsAliases = subSet.Childrens.Select(c => c.Item.Alias).ToList();
            Assert.Contains("method1()", subItemsAliases);
            Assert.Contains("method2()", subItemsAliases);
            Assert.Contains("method3()", subItemsAliases);
        }

        [Test]
        public void TestGroupNodesByVariableDeclaration2()
        {
            var testSet = new Hierachy<CodeNode>(new CodeNode { Alias = "root" });
            testSet.Childrens = new List<Hierachy<CodeNode>>();
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1.level2()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1.level2.level3()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1.level2.level3.level4()" }));

            NodesPostProcessor.GroupNodesByVariableDeclaration(testSet);

            var set = testSet.Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level1()", set[0].Item.Alias);

            set = set[0].Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level2()", set[0].Item.Alias);

            set = set[0].Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level3()", set[0].Item.Alias);

            set = set[0].Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level4()", set[0].Item.Alias);
        }

        [Test]
        public void TestGroupNodesByVariableDeclaration3()
        {
            var testSet = new Hierachy<CodeNode>(new CodeNode { Alias = "root" });
            testSet.Childrens = new List<Hierachy<CodeNode>>();
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1.level2.level3.level4()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1.level2.level3()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1.level2()" }));
            testSet.Childrens.Add(new Hierachy<CodeNode>(new CodeNode { Alias = "level1()" }));

            NodesPostProcessor.GroupNodesByVariableDeclaration(testSet);

            var set = testSet.Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level1()", set[0].Item.Alias);

            set = set[0].Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level2()", set[0].Item.Alias);

            set = set[0].Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level3()", set[0].Item.Alias);

            set = set[0].Childrens;
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual("level4()", set[0].Item.Alias);
        }

        [Test]
        public void JustHeavyLoadTest_JQuery()
        {
            var source = TestsHelper.GetEmbeddedText("JsParserTest.NodesPostProcessor.TestsScripts.jquery-1.7.1.js");

            for (var i = 0; i < 10; i++)
            {
                var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source);
                Assert.IsNotNull(actualResult);
            }
        }
    }
}
