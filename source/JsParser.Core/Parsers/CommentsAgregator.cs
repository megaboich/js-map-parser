using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    internal class CommentsAgregator
    {
        private List<CommentWrapper> _comments;

        public IList<CommentWrapper> Comments
        {
            get
            {
                return _comments.AsReadOnly();
            }
        }

        public void ProcessComments(IEnumerable<CommentWrapper> rawComments)
        {
            _comments = new List<CommentWrapper>();
            CommentWrapper prevCom = null;

            // Combine comments on neighbour lines to comment groups
            foreach(var currentCom in rawComments)
            {
                if (prevCom != null)
                {
                    if (prevCom.EndLine == currentCom.StartLine - 1)
                    {
                        prevCom.Spelling = prevCom.Spelling + Environment.NewLine + currentCom.Spelling;
                        prevCom.EndLine = currentCom.EndLine;
                    }
                    else
                    {
                        _comments.Add(currentCom);
                        prevCom = currentCom;
                    }
                }
                else
                {
                    _comments.Add(currentCom);
                    prevCom = currentCom;
                }
            }
        }

        public string GetComment(int startline, int endline)
        {
            var result = new List<string>();
            foreach (var comment in _comments.Where(c => !c.Processed))
            {
                if (comment.EndLine == startline // The same line
                    || comment.EndLine == startline - 1 // The prev line
                    || comment.StartLine == startline + 1 // The next line
                   )
                {
                    result.Add(comment.Spelling);
                    comment.Processed = true;
                }
            }

            var r = string.Join(Environment.NewLine, result.ToArray());
            if (string.IsNullOrEmpty(r.Trim()))
            {
                return null;
            }

            return r.Trim();
        }
    }
}
