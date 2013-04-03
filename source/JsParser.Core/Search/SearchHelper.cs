using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Search
{
    public static class SearchHelper
    {
        static char[] _separators = new[] { '.', '|', '^', '?', '!', '$', '(', ')', '\'', '"', ',', ';', ' ', '-', '>', '<' };

        public static IEnumerable<string> SplitFunctionName(string fname)
        {
            var res = fname
                .Split(_separators, StringSplitOptions.RemoveEmptyEntries)
                .Where(ch => !string.IsNullOrEmpty(ch.Trim()));

            return res
                .SelectMany(r => SplitByUpperCaseWording(r))
                .Where(s => !string.IsNullOrEmpty(s.Trim()));
        }

        private static IEnumerable<string> SplitByUpperCaseWording(string fname)
        {
            var lastUpperIndex = 0;
            var counter = 0;
            for (int index = 0; index < fname.Length; ++index)
            {
                if (char.IsUpper(fname[index]) ||
                    char.IsDigit(fname[index]))
                {
                    var tl = lastUpperIndex;
                    lastUpperIndex = index;
                    ++counter;
                    yield return fname.Substring(tl, index - tl);
                }
            }

            yield return fname.Substring(lastUpperIndex, fname.Length - lastUpperIndex);
        }

        public static IEnumerable<T> GetMatches<T>(IEnumerable<T> source, Func<T, string> nameDelegate, string input)
        {
            var inputSet = SearchHelper.SplitFunctionName(input).ToList();

            var res = source
                .Select(item => new {
                    item = item,
                    count = CompareEntities(inputSet, SearchHelper.SplitFunctionName(nameDelegate(item)).ToList())
                })
                .Where(i => i.count > 0)
                .OrderByDescending(i => i.count)
                .Select(i => i.item);

            return res;
        }

        public static int CompareEntities(IList<string> input, IList<string> test)
        {
            int encounters = 0;
            foreach (var inputPart in input)
            {
                bool hasMatch = false;
                foreach (var testPart in test)
                {
                    if (testPart.StartsWith(inputPart, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ++encounters;
                        hasMatch = true;
                    }
                }

                if (!hasMatch)
                {
                    // all input tokens must be present in test
                    return 0;
                }
            }

            return encounters;
        }
    }
}
