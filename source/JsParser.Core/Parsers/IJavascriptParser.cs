namespace JsParser.Core.Parsers
{
    public interface IJavascriptParser
    {
        /// <summary>
        /// Parse JavaScript
        /// </summary>
        /// <param name="code">string with JavaScript code</param>
        /// <param name="filename"></param>
        /// <returns></returns>
        JSParserResult Parse(string code, string filename);
    }
}