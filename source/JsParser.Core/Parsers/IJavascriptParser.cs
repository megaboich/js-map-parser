namespace JsParser.Core.Parsers
{
    public interface IJavascriptParser
    {
        /// <summary>
        /// Parse javascript
        /// </summary>
        /// <param name="code">string with javascript code</param>
        /// <returns></returns>
        JSParserResult Parse(string code);
    }
}