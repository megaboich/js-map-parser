using System;
namespace JsParser.Core.Infrastructure
{
    public interface ISettings
    {
        bool AutoExpandAll { get; set; }
        string BookmarksData { get; set; }
        System.Collections.Specialized.StringCollection Extensions { get; set; }
        bool FilterByMarksEnabled { get; set; }
        bool FixAspNetTags { get; set; }
        bool FixRazorSyntax { get; set; }
        bool HideAnonymousFunctions { get; set; }
        bool HierarchyEnabled { get; set; }
        System.Collections.Specialized.StringCollection JSExtensions { get; set; }
        int MaxParametersLength { get; set; }
        int MaxParametersLengthInFunctionChain { get; set; }
        bool SendStatistics { get; set; }
        string SendStatisticsPolitic { get; set; }
        bool ShowLineNumbersEnabled { get; set; }
        bool SortingEnabled { get; set; }
        string Statistics { get; set; }
        string StatisticsServerUrl { get; set; }
        System.Drawing.Color taggedFunction1Color { get; set; }
        System.Drawing.Font taggedFunction1Font { get; set; }
        System.Drawing.Color taggedFunction2Color { get; set; }
        System.Drawing.Font taggedFunction2Font { get; set; }
        System.Drawing.Color taggedFunction3Color { get; set; }
        System.Drawing.Font taggedFunction3Font { get; set; }
        System.Drawing.Color taggedFunction4Color { get; set; }
        System.Drawing.Font taggedFunction4Font { get; set; }
        System.Drawing.Color taggedFunction5Color { get; set; }
        System.Drawing.Font taggedFunction5Font { get; set; }
        System.Drawing.Color taggedFunction6Color { get; set; }
        System.Drawing.Font taggedFunction6Font { get; set; }
        bool ToDoListCollapsed { get; set; }
        int ToDoListLastHeight { get; set; }
        bool TrackActiveItem { get; set; }
        bool UseVSColorTheme { get; set; }
        bool Visible { get; set; }
        System.Collections.Specialized.StringCollection ToDoKeywords { get; set; }
    }
}
