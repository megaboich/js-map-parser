using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Code
{
	public static class CodeTransformer
	{
		public static string FixContinueStringLiterals(string source)
		{
			var codeParts = source.Split(new[] { "\\\r\n", "\\\r", "\\\n" }, StringSplitOptions.None);
			var multilineMultiplicator = 1;
			if (codeParts.Length > 0)
			{
				//Insert new lines to compensate removed ones. This is necessary to match line numbers of original file.
				for (var i = 1; i < codeParts.Length; ++i)
				{
					var index = codeParts[i].IndexOfAny(new[] { '\r', '\n' });
					if (index >= 0)
					{
						codeParts[i] = codeParts[i].Insert(index, new String('\n', multilineMultiplicator));
						multilineMultiplicator = 1;
					}
					else
					{
						++multilineMultiplicator;
					}
				}

				return string.Join(string.Empty, codeParts);
			}

			return source;
		}

		public static string KillAspNetTags(string source)
		{
			return source.Replace("<%=", string.Empty)
				.Replace("<%", string.Empty)
				.Replace("%>", string.Empty);
		}

		internal static string FixStringScriptBlocks(string code)
		{
			return code.Replace("\"<script", "\"<sxript")
				.Replace("'<script", "'<sxript")
				.Replace("script>\"", "sxript>\"")
				.Replace("script>'", "sxript>'");
		}

		public static IEnumerable<CodeChunk> ExtractJsFromSource(string source)
		{
			var resultList = new List<CodeChunk>();

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
				for (int ch = 0; ch < chunks.Count; ch += 2)
				{
					var bsi = chunks[ch].Key;
					var bfi = chunks[ch].Value;
					var esi = chunks[ch + 1].Key;
					var efi = chunks[ch + 1].Value;

					StringBuilder sb = new StringBuilder(source.Length, source.Length);

					sb.Append(GetSpacedChunk(source.Substring(0, bfi)));
					sb.Append(source.Substring(bfi, esi - bfi));
					sb.Append(GetSpacedChunk(source.Substring(esi, source.Length - esi)));

					resultList.Add(new CodeChunk { Code = sb.ToString() });
				}
			}
			else
			{
				resultList.Add(new CodeChunk { Code = source });
			}

			return resultList;
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
