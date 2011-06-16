using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.JScript.Compiler;

namespace JsParserCore.Parsers
{
	public static class JSParserExtensions
	{
		public static IEnumerable<ElementType> GetEnumerable<ElementType, ParentType>(this DList<ElementType, ParentType> list)
		{
			var iterator = new DList<ElementType, ParentType>.Iterator(list);
			while (iterator.Element != null)
			{
				yield return iterator.Element;
				iterator.Advance();
			}
		}

		public static string Shortenize(this String s, int targetLength)
		{
			if (targetLength <= 0)
			{
				return string.Empty;
			}

			if (s.Length > targetLength)
			{
				var si = (int)2 * (targetLength / 3);
				s = s.Substring(0, si) + '\x2026' + s.Substring(s.Length + 1 - targetLength + si);
			}

			return s;
		}
	}
}
