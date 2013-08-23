using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

		public static string FixDebuggerKeyword(string source)
		{
			var regExp = new Regex(@"([\W])(debugger)([\W])", RegexOptions.Compiled);
			return regExp.Replace(source, "$1Debugger$3");
		}

		public static string KillAspNetTags(string source)
		{
			//Replace value-evaluated blocks like <%= ...%> or <%: ...%> with numeric-letter contents
			var regExp = new Regex("(<%=.*?%>)|(<%:.*?%>)", RegexOptions.Singleline | RegexOptions.Compiled);
			source = regExp.Replace(source, (match =>
			{
				return new String(match.Value.Where(c => Char.IsLetterOrDigit(c)).ToArray());
			}));

			//Replace <%%> blocks with newlines to preserve correct line numbering
			var regExp2 = new Regex("(<%.*?%>)", RegexOptions.Singleline | RegexOptions.Compiled);
			source = regExp2.Replace(source, (match =>
			{
				return GetSpacedChunk(match.Value);
			}));

			return source;
		}

		public static string ApplyJSParserSkip(string source)
		{
			//Replace matched blocks with newlines to preserve correct line numbering
			var regExp2 = new Regex("(jsparser:off.*?jsparser:on)", RegexOptions.Singleline | RegexOptions.Compiled);
			source = regExp2.Replace(source, (match =>
			{
				return GetSpacedChunk(match.Value);
			}));

			return source;
		}

		public static string FixRazorSyntax(string source)
		{
			return source.Replace("@", string.Empty);
		}

		public static string FixStringScriptBlocks(string code)
		{
			return code.Replace("\"<script", "\"<sxript")
				.Replace("'<script", "'<sxript")
				.Replace("script>\"", "sxript>\"")
				.Replace("script>'", "sxript>'");
		}

		/// <summary>
		/// Modifies argument
		/// </summary>
		/// <param name="source"></param>
		/// <returns>True if found <script> blocks </returns>
		public static bool ExtractJsFromSource(ref string source)
		{
			var regEx = new Regex(@"(<script [\s\S]*?>[\s\S]*?</script>)|(<script>[\s\S]*?</script>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			var matches = regEx.Split(source);
			if (matches.Length > 1)
			{
				var sb = new StringBuilder();

				foreach (var m in matches)
				{
					if (m.StartsWith("<script", StringComparison.OrdinalIgnoreCase))
					{
						//process script block
						var script = ProcessScriptBlock(m);
						if (!string.IsNullOrEmpty(script))
						{
							sb.Append(script);
						}
						else
						{
							sb.Append(GetSpacedChunk(m));
						}
					}
					else
					{
						//strip all symbols except line breaks
						sb.Append(GetSpacedChunk(m));
					}
				}

				source = sb.ToString();
				return true;
			}

			return false;
		}

		private static string ProcessScriptBlock(string scriptBlock)
		{
			//get beginning tag <script type="text/javascript>
			var beginTagEndIndex = scriptBlock.IndexOf('>');
			if (beginTagEndIndex < 0)
			{
				return null;
			}

			var beginTag = scriptBlock.Substring(0, beginTagEndIndex + 1);

			//decide if this is javascript declaration?
			var typeBeginIndex = beginTag.IndexOf("type=", StringComparison.OrdinalIgnoreCase);
			if (typeBeginIndex > 0)
			{
				var validJavascriptTypes = new[] {
					"text/javascript",
					"text/ecmascript",
					"application/ecmascript",
					"application/javascript",
				};

				if (!validJavascriptTypes.Any(type => (beginTag.IndexOf(type, StringComparison.OrdinalIgnoreCase) > 0)))
				{
					return null;
				}
			}

			//Skip asp.net runat="server" script blocks
			if (beginTag.Contains("runat") && beginTag.Contains("server"))
			{
				return null;
			}

			//get end tag </script>
			var endTagBeginIndex = scriptBlock.LastIndexOf('<');
			if (endTagBeginIndex < 0)
			{
				return null;
			}

			var endTag = scriptBlock.Substring(endTagBeginIndex);

			var script = scriptBlock.Substring(beginTagEndIndex + 1, endTagBeginIndex - beginTagEndIndex - 1);

			return script;
		}

		private static string GetSpacedChunk(string source)
		{
			return new String(source.Where(ch => (ch == '\r' || ch == '\n')).ToArray());
		}
	}
}
