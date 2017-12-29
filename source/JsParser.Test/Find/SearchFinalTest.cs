using JsParser.Core.Search;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace JsParserTest.UnitTests
{
    [TestFixture]
    public class SearchFinalTest
    {
        private static string[] _source = {   //first goes the test case index - to simplify test cases
                                            "0|this.Method1()",
                                            "1|this.Method2()",
                                            "2|this.longMethodDescription()",
                                            "3|this.withParam('btn1')",
                                            "4|this.withParamChain('btn1', 'btn2', 'btn3')",
                                            "5|ABBR.bigLetters.EAContainer()"
                                        };

        private static IEnumerable<KeyValuePair<int, string>> TransformSource(string[] source)
        {
            return _source.Select(i =>
            {
                var d = i.Split(new char[] { '|' });
                return new KeyValuePair<int, string>(int.Parse(d[0]), d[1]);
            });
        }

        private void AssertIndexes(KeyValuePair<int, string>[] data, int[] indexes) 
        {
            var dic = data.ToDictionary(k => k.Key);
            Assert.AreEqual(indexes.Length, data.Length, "Found wrong amount");
            for (int i = 0; i < data.Length; ++i)
            {
                Assert.AreEqual(indexes[i], dic[indexes[i]].Key, "Wrong index found at position " + i);
            }
        }

        [Test]
        public void CheckBaseSearch()
        {
            var src = TransformSource(_source).ToArray();
            var matches = SearchHelper.GetMatches(src, i => i.Value, "method").ToArray();
            AssertIndexes(matches, new[] { 0, 1, 2 });

            matches = SearchHelper.GetMatches(src, i => i.Value, "btn1").ToArray();
            AssertIndexes(matches, new[] { 3, 4 });

            matches = SearchHelper.GetMatches(src, i => i.Value, "btn2").ToArray();
            AssertIndexes(matches, new[] { 4 });

            matches = SearchHelper.GetMatches(src, i => i.Value, "Chain").ToArray();
            AssertIndexes(matches, new[] { 4 });

            matches = SearchHelper.GetMatches(src, i => i.Value, "ABBR").ToArray();
            AssertIndexes(matches, new[] { 5 });

            matches = SearchHelper.GetMatches(src, i => i.Value, "EAContainer").ToArray();
            AssertIndexes(matches, new[] { 5 });

            matches = SearchHelper.GetMatches(src, i => i.Value, "EACont").ToArray();
            AssertIndexes(matches, new[] { 5 });
        }
    }
}
