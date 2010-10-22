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
	}
}
