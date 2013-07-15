using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JsParser.UI.Helpers
{
    public static class CommentTipFormatter
    {
        public class CommentStructure
        {
            public string Summary;
            public List<KeyValuePair<String, String>> Parameters = new List<KeyValuePair<string, string>>();
            public string OtherText;
            public string Returns;
        }

        public static String FormatPlainTextComment(String originalComment)
        {
            if (string.IsNullOrEmpty(originalComment))
            {
                return originalComment;
            }

            var structure = ParseCommentStructure(originalComment);

            if (structure == null)
            {
                return originalComment;
            }

            StringBuilder toolTipText = new StringBuilder();
            if (!string.IsNullOrEmpty(structure.Summary))
            {
                toolTipText.AppendLine(structure.Summary);
                toolTipText.AppendLine("");
            }
            if (structure.Parameters.Count > 0)
            {
                toolTipText.AppendLine("Parameters: ");
                foreach (var p in structure.Parameters)
                {
                    toolTipText.AppendLine("         " + p.Key + ": " + p.Value);
                }
            }
            toolTipText.AppendLine(structure.OtherText);
            if (!string.IsNullOrEmpty(structure.Returns))
            {
                toolTipText.AppendLine("Returns: " + structure.Returns);
            }

            return toolTipText.ToString();
        }

        /// <summary>
        /// Parse XML comment and returns structurized info
        /// </summary>
        /// <param name="originalComment"></param>
        /// <returns></returns>
        public static CommentStructure ParseCommentStructure(String originalComment)
        {
            if(String.IsNullOrEmpty(originalComment))
            {
                return null;
            }

            // Remove // and /**/ from comments to improve their readability
            var lines = originalComment
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim(new[] { '*', '/', '|', ' ' }))
                .Where(s => !String.IsNullOrEmpty(s))
                .ToArray();

            String resultStr = string.Join(Environment.NewLine, lines);

            CommentStructure result = new CommentStructure();

            //Try to load comments as XML
            if (resultStr.Contains("</"))
            {
                try
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml("<root>" + resultStr + "</root>");
                    var summaryNode = xmlDoc.SelectSingleNode("//summary");
                    if (summaryNode != null)
                    {
                        result.Summary = summaryNode.InnerText.Trim();
                        summaryNode.InnerXml = string.Empty;
                    }

                    var returnsNode = xmlDoc.SelectSingleNode("//returns");
                    if (returnsNode != null)
                    {
                        result.Returns = returnsNode.InnerText.Trim();
                        returnsNode.InnerXml = string.Empty;
                    }

                    var paramNodes = xmlDoc.SelectNodes("//param");
                    if (paramNodes != null)
                    {
                        foreach (XmlElement paramElt in paramNodes)
                        {
                            result.Parameters.Add(new KeyValuePair<string, string>(paramElt.GetAttribute("name").Trim(), paramElt.InnerText.Trim()));
                            paramElt.InnerXml = string.Empty;
                        }
                    }

                    result.OtherText = xmlDoc.InnerText.Trim();
                    return result;
                }
                catch(Exception ex) {
                    //no luck
                }
            }

            result.OtherText = resultStr;
            return result;
        }
    }
}
