using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Code;
using JsParser.Core.Helpers;
using JsParser.Test.Helpers;
using NUnit.Framework;

namespace JsParser.Test.SimpleHierarchySerializerTest
{
    [TestFixture]
    public class SimpleHierarchySerializerTest
    {
        [Test]
        public void SimpleHierarchySerializer_Test()
        {
            var testData = new Hierarchy<CodeNode>(new CodeNode("Node1", 0, 100, "Comment1"))
            {
                Children = new List<Hierarchy<CodeNode>>
                {
                    new Hierarchy<CodeNode>(new CodeNode("S1", 1, 2)),
                    new Hierarchy<CodeNode>(new CodeNode("S2", 2, 3))
                }
            };

            var serialized = SimpleHierarchySerializer.Serialize(testData);
            var deserialised = SimpleHierarchySerializer.Deserialize<CodeNode>(serialized);
            var isEqual = HierarchyComparer.Compare(testData, deserialised, CodeNode.GetDefaultComparer());
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void SimpleHierarchySerializer_Test2()
        {
            var testData = new Hierarchy<CodeNode>(new CodeNode("Node1", 0, 100, "Comment1\r\nHere the second line"))
            {
                Children = new List<Hierarchy<CodeNode>>
                {
                    new Hierarchy<CodeNode>(new CodeNode("S1", 1, 20, "{}\\/+-_=!@#$%^&*;:'<>")),
                    new Hierarchy<CodeNode>(new CodeNode("S2", 20, 40, "Or some xml encoded &#A;&#D; &amp;&gt;"))
                    {
                        Children = new List<Hierarchy<CodeNode>>
                        {
                            new Hierarchy<CodeNode>(new CodeNode("D1", 20, 21)),
                            new Hierarchy<CodeNode>(new CodeNode("D2", 30, 31))
                        }
                    },
                    new Hierarchy<CodeNode>(new CodeNode("S3", 40, 60))
                    {
                        Children = new List<Hierarchy<CodeNode>>
                        {
                            new Hierarchy<CodeNode>(new CodeNode("D3", 40, 41)),
                            new Hierarchy<CodeNode>(new CodeNode("D4", 50, 55))
                            {
                                Children = new List<Hierarchy<CodeNode>>
                                {
                                    new Hierarchy<CodeNode>(new CodeNode("L1", 51, 52))
                                }
                            }
                        }
                    },
                    new Hierarchy<CodeNode>(new CodeNode("S4", 60, 70))
                }
            };

            var serialized = SimpleHierarchySerializer.Serialize(testData);
            var deserialised = SimpleHierarchySerializer.Deserialize<CodeNode>(serialized);
            var isEqual = HierarchyComparer.Compare(testData, deserialised, CodeNode.GetDefaultComparer());
            Assert.IsTrue(isEqual);
        }
    }
}
