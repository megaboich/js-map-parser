using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.JScript.Compiler;

namespace JsParser.Core.Parsers
{
	internal class CommentsAgregator
	{
		private List<CustomComment> _comments = new List<CustomComment>();
		private string[] _code;

		public IEnumerable<CustomComment> Comments
		{
			get
			{
				return _comments.AsReadOnly();
			}
		}

		public CommentsAgregator(List<Comment> comments, string[] code)
		{
			_code = code;

			string aggregatedCommentString = string.Empty;
			int start = 0;
			int end = 0;
			for (int index = 0; index < comments.Count; ++index)
			{
				var c1 = comments[index];
				if (!ContainsLiteralOrDigits(c1.Spelling))
				{
					continue;
				}

				var c2 = index < comments.Count - 1 ? comments[index + 1] : null;

				var c1t = c1.Location.StartLine != c1.Location.EndLine;
				if (!c1t)
				{
					c1t = c1.Spelling == _code[c1.Location.StartLine - 1].Trim();
				}

				var c2t = c1.Location.StartLine != c1.Location.EndLine;
				if (!c2t)
				{
					c2t = c2 != null ? c2.Spelling == _code[c2.Location.StartLine - 1].Trim() : false;
				}

				if (c2 != null && c1.Location.EndLine + 1 == c2.Location.StartLine && c1t && c2t)
				{
					// Continuous comment.
					if (string.IsNullOrEmpty(aggregatedCommentString))
					{
						aggregatedCommentString = c1.Spelling + "\r\n" + c2.Spelling;
						start = c1.Location.StartLine;
						end = c2.Location.EndLine;
					}
					else
					{
						aggregatedCommentString += "\r\n" + c2.Spelling;
						end = c2.Location.EndLine;
					}
				}
				else
				{
					// Break.
					if (!string.IsNullOrEmpty(aggregatedCommentString))
					{
						CustomComment c = new CustomComment
						{
							Text = aggregatedCommentString,
							Start = start,
							End = end,
							Solid = true
						};
						_comments.Add(c);

						aggregatedCommentString = string.Empty;
					}
					else
					{
						CustomComment c = new CustomComment
						{
							Text = c1.Spelling,
							Start = c1.Location.StartLine,
							End = c1.Location.EndLine,
							Solid = c1t
						};

						_comments.Add(c);
					}
				}
			}

			// Last case.
			if (!string.IsNullOrEmpty(aggregatedCommentString))
			{
				CustomComment c = new CustomComment
				{
					Text = aggregatedCommentString,
					Start = start,
					End = end,
					Solid = true
				};
				_comments.Add(c);
			}
		}

		public string GetComment(int startline, int endline)
		{
			var result = new List<string>();
			foreach (CustomComment comment in _comments.Where(c => !c.Processed))
			{
				// The same line
				if (comment.End == startline)
				{
					result.Add(comment.Text);
					comment.Processed = true;
					continue;
				}

				if (!comment.Solid)
				{
					continue;
				}

				// The prev line
				if (comment.End == startline - 1)
				{
					result.Add(comment.Text);
					comment.Processed = true;
					continue;
				}

				// The next line
				if (comment.Start == startline + 1)
				{
					result.Add(comment.Text);
					comment.Processed = true;
					continue;
				}
			}

			var r = string.Join("\r\n", result.ToArray());
			if (string.IsNullOrEmpty(r.Trim()))
			{
				return null;
			}

			return r;
		}

		private bool ContainsLiteralOrDigits(string src)
		{
			bool hasDigits = src.Any(c => Char.IsLetterOrDigit(c));
			return hasDigits;
		}
	}
}
