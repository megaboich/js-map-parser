using System;
namespace JsParser.Core.Infrastructure
{
    public interface ISettings
    {
        System.Collections.Specialized.StringCollection Extensions { get; set; }

        bool FixAspNetTags { get; set; }
        System.Collections.Specialized.StringCollection FixAspNetTagsExtensions { get; set; }
        bool FixRazorSyntax { get; set; }
        System.Collections.Specialized.StringCollection FixRazorSyntaxExtensions { get; set; }
        bool HideAnonymousFunctions { get; set; }
        bool ScriptStripEnabled { get; set; }
        System.Collections.Specialized.StringCollection ScriptStripExtensions { get; set; }
        int MaxParametersLength { get; set; }
        int MaxParametersLengthInFunctionChain { get; set; }

        bool SendStatistics { get; set; }
        string SendStatisticsPolitic { get; set; }
        string Statistics { get; set; }
        string StatisticsServerUrl { get; set; }

        System.Collections.Specialized.StringCollection ToDoKeywords { get; set; }

        bool FilterByMarksEnabled { get; set; }
        string BookmarksData { get; set; }

        bool SortingEnabled { get; set; }
        bool AutoExpandAll { get; set; }
        bool ShowLineNumbersEnabled { get; set; }
        bool ToDoListCollapsed { get; set; }
        int ToDoListLastHeight { get; set; }
        bool TrackActiveItem { get; set; }
        bool Visible { get; set; }
        string ThemeSettingsSerialized { get; set; }

        System.Drawing.Font TreeFont { get; set; }
        System.Drawing.Color taggedFunction2Color { get; set; }
        System.Drawing.Color taggedFunction3Color { get; set; }
        System.Drawing.Color taggedFunction4Color { get; set; }
        System.Drawing.Color taggedFunction5Color { get; set; }
        System.Drawing.Color taggedFunction6Color { get; set; }
    }
}
