using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    public static class JsParserExtensions
    {
        public static string SafeTrimStart(this string s, char symbol)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return s.TrimStart(symbol);
        }

        public static string Shortenize(this String s, int targetLength)
        {
            if (targetLength <= 0)
            {
                return string.Empty;
            }

            if (s.Length > targetLength)
            {
                var si = (int) 2*(targetLength/3);
                s = s.Substring(0, si) + '\x2026' + s.Substring(s.Length + 1 - targetLength + si);
            }

            return s;
        }

        /// <summary>
        /// Transform input string by adding spaces where words should be separeated.
        /// Example: "ThisStringShouldBeSeparated" => "This String Shoud Be Separated"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string SplitWordsByCamelCase(this String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var chars = s.ToList();
            for (int charIndex = 0; charIndex < chars.Count; charIndex++)
            {
                if (char.IsUpper(chars[charIndex]))
                {
                    chars.Insert(charIndex, ' ');
                    charIndex++;
                }
            }

            return new string(chars.ToArray()).Trim();
        }

        public static StringCollection ToStringCollection(this string[] strings)
        {
            var sc = new StringCollection();
            sc.AddRange(strings);
            return sc;
        }

        public static IEnumerable<string> GetEnumerable(this StringCollection sc)
        {
            foreach (var s in sc)
            {
                yield return s;
            }
        }
    }
}
