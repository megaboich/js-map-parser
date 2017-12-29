namespace JsMapParser.NppPlugin.NppPluginBaseInfrastructure
{
    internal abstract partial class NppPluginBase
    {
        private static NppPluginBase GetPluginInstance()
        {
            return new JsMapParserPlugin();
        }
    }
}