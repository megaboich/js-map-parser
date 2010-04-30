using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParserCore.Code
{
	public static class CodeTransformer
	{
		public static string KillAspNetTags(string source)
		{
			return source.Replace("<%=", string.Empty)
				.Replace("<%", string.Empty)
				.Replace("%>", string.Empty);
		}

		public static string KillNonJavascript(string source)
		{
			var i = 0;
			var beginTagStart = -1;
			var beginTagFinish = -1;
			var endTagStart = -1;
			var endTagFinish = -1;
			var chunks = new List<KeyValuePair<int, int>>();

			while (FindTag(source, "<script", i, out beginTagStart, out beginTagFinish))
			{
				var isFinish = FindTag(source, "</script", beginTagStart, out endTagStart, out endTagFinish);
				if (isFinish)
				{
					chunks.Add(new KeyValuePair<int, int>(i, beginTagFinish));
					chunks.Add(new KeyValuePair<int, int>(endTagStart, endTagFinish));
					i = endTagFinish;
				}
				else
				{
					break;
				}
			}

			if (chunks.Count > 0)
			{
				StringBuilder sb = new StringBuilder(source.Length, source.Length);

				for (int ch = 0; ch < chunks.Count; ch+=2)
				{
					var bsi = chunks[ch].Key;
					var bfi = chunks[ch].Value;
					var esi = chunks[ch + 1].Key;
					var efi = chunks[ch + 1].Value;

					sb.Append(GetSpacedChunk(source.Substring(bsi, bfi - bsi)));
					sb.Append(source.Substring(bfi, esi - bfi));
					sb.Append(GetSpacedChunk(source.Substring(esi, efi - esi)));
				}

				return sb.ToString();
			}

			return string.Empty;
		}

		private static string GetSpacedChunk(string source)
		{
			var res = new StringBuilder(source.Length, source.Length);
			res.Append(source.Where(ch => (ch == '\r' || ch == '\n')).ToArray());
			return res.ToString();
		}

		private static bool FindTag(string source, string tag, int startSearchIndex, out int tagStart, out int tagFinish)
		{
			var si = source.IndexOf(tag, startSearchIndex, StringComparison.InvariantCultureIgnoreCase);
			if (si >= 0)
			{
				var afterTagSymbol = source[si + tag.Length];
				if (afterTagSymbol != ' ' && afterTagSymbol != '>') //Avoid tag like "<Scripts>"
				{
					return FindTag(source, tag, si + 1, out tagStart, out tagFinish);
				}
				else
				{
					var ei = source.IndexOf(">", si);

					if (ei >= 0)
					{
						tagStart = si;
						tagFinish = ei + 1;
						return true;
					}
				}
			}

			tagStart = -1;
			tagFinish = -1;
			return false;
		}
	}
}
