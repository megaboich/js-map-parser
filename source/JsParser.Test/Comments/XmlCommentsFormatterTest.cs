using JsParser.UI.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Test.Comments
{
    [TestFixture(Description = "Xml comments formatter")]
    public class XmlCommentsFormatterTest
    {
        [Test]
        public void TestXmlCommentsFormatter_NotXml()
        {
            var testComment = @"not xml";

            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("not xml", result.OtherText);
        }

        [Test]
        public void TestXmlCommentsFormatter_Summary()
        {
            var testComment = @"
/// <summary>
///     This is function to do something
/// </summary>
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("This is function to do something", result.Summary, "Summary not match");
        }

        [Test]
        public void TestXmlCommentsFormatter_Params()
        {
            var testComment = @"
/// <summary>
///     This is function to do something
/// </summary>
/// <param name=""par1"">
///     Param # 1
/// </param>
/// <param name=""par2"">
///     Param # 2
/// </param>
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("This is function to do something", result.Summary, "Summary not match");

            Assert.AreEqual(2, result.Parameters.Count, "Parameters count not match");
            Assert.AreEqual("par1", result.Parameters[0].Key, "Parameter not match");
            Assert.AreEqual("Param # 1", result.Parameters[0].Value, "Parameter not match");
            Assert.AreEqual("par2", result.Parameters[1].Key, "Parameter not match");
            Assert.AreEqual("Param # 2", result.Parameters[1].Value, "Parameter not match");
        }

        [Test]
        public void TestXmlCommentsFormatter_Returns()
        {
            var testComment = @"
/// <summary>
///     This is function to do something
/// </summary>
/// <returns>
/// Some result
/// </returns>
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("This is function to do something", result.Summary, "Summary not match");
            Assert.AreEqual("Some result", result.Returns, "Returns not match");
        }

        [Test]
        public void TestXmlCommentsFormatter_OtherTags()
        {
            var testComment = @"
/// <summary>
///     This is function to do something
/// </summary>
/// <customsummary>
///     This is custom tag
/// </customsummary>
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("This is function to do something", result.Summary, "Summary not match");
            Assert.AreEqual("This is custom tag", result.OtherText, "Other text not match");
        }

        [Test]
        public void TestXmlCommentsFormatter_SeveralOtherTags()
        {
            var testComment = @"
/// <summary>
///     This is function to do something
/// </summary>
/// <customsummary>
///     This is custom tag
/// </customsummary>
/// <customsummary2>
///     This is custom tag 2
/// </customsummary2>
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("This is function to do something", result.Summary, "Summary not match");
            Assert.AreEqual("This is custom tag\r\n\r\nThis is custom tag 2", result.OtherText, "Other text not match");
        }

        [Test]
        public void TestCommentsFormatter_Remove_CommentsSyntaxSimple()
        {
            var testComment = @"
// Simple comment
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("Simple comment", result.OtherText);
        }

        [Test]
        public void TestCommentsFormatter_Remove_CommentsSyntaxMultiline()
        {
            var testComment = @"
// Simple comment
// Second line
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("Simple comment\r\nSecond line", result.OtherText);
        }

        [Test]
        public void TestCommentsFormatter_Remove_CommentsSyntaxMultilineWithExtraSlashes()
        {
            var testComment = @"
/// Simple comment
/// Second line
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("Simple comment\r\nSecond line", result.OtherText);
        }

        [Test]
        public void TestCommentsFormatter_Remove_CommentsSyntaxMultiline2()
        {
            var testComment = @"
/*
 First line
 Second line
*/";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("First line\r\nSecond line", result.OtherText);
        }

        [Test]
        public void TestCommentsFormatter_Remove_CommentsSyntaxMultilineMixed()
        {
            var testComment = @"
/*
 First line
 Second line
*/
/// Third line
/*
    4th line
    5th
*/
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("First line\r\nSecond line\r\nThird line\r\n4th line\r\n5th", result.OtherText);
        }

        [Test]
        public void TestCommentsFormatter_Remove_CommentsSyntaxMultilineBoxStyle()
        {
            var testComment = @"
/************************************************************
*   Box styling comment                                     *
*   Looks like a very solid block                           *
*                                                           *
*                                                           *
*   Author: Person who likes blocks                         *
************************************************************/
";
            var result = CommentTipFormatter.ParseCommentStructure(testComment);
            Assert.AreEqual("Box styling comment\r\nLooks like a very solid block\r\nAuthor: Person who likes blocks", result.OtherText);
        }
    }
}
