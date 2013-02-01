using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using JsParser.UI.Properties;

namespace JsParser.UI.Helpers
{
    public class Statistics
    {
        public Guid ClientId { get; set; }

        #region Counters

        public int TotalRunCount { get; set; }

        public int FindFeatureShowCount { get; set; }

        public int FindFeatureUsedCount { get; set; }

        public int ToggleHierachyOptionCount { get; set; }

        public int ExpandAllCommandExecutedCount { get; set; }

        public int CollapseAllCommandExecutedCount { get; set; }

        public int SettingsDialogShowedCount { get; set; }

        public int ToggleShowLineNumbersCount { get; set; }

        public int NavigateFromToDoListCount { get; set; }

        public int NavigateFromErrorListCount { get; set; }

        public int NavigateFromFunctionsTreeCount { get; set; }

        public int FilterByMarksUsedCount { get; set; }

        public int HideAnonymousFunctionsUsedCount { get; set; }

        public int SortingUsedCount { get; set; }

        public int TreeContextMenuExecutedCount { get; set; }

        public int SetMarkExecutedCount { get; set; }

        #endregion

        #region Settings

        public bool IsAutoTrackItemEnabled { get; set; }

        public bool IsHierachyShowedOption { get; set; }

        public bool IsAutoExpandAllEnabled { get; set; }

        public bool IsShowLineNumbersEnabled { get; set; }

        public bool IsSortingEnabled { get; set; }

        public bool IsFilterByMarksEnabled { get; set; }

        public bool IsHideAnonymousFunctionsEnabled { get; set; }
        #endregion

        
        public DateTime FirstDateUse { get; set; }

        public DateTime LastSubmittedTime { get; set; }

        public string Version {get; set;}

        /// <summary>
        /// String identifies container which runs a javascript parser - VS2010, VS2011, VS2008, or other.
        /// </summary>
        public string Container { get; set; }

        public Statistics()
        {
            Version = new AssemblyName(typeof(Statistics).Assembly.FullName).Version.ToString();
        }

        public void FirstInitialize()
        {
            ClientId = Guid.NewGuid();
            FirstDateUse = DateTime.UtcNow;
        }

        public void UpdateStatisticsOnStart()
        {
            ++TotalRunCount;
        }

        public void UpdateStatisticsFromSettings()
        {
            IsAutoTrackItemEnabled = Settings.Default.TrackActiveItem;
            IsHierachyShowedOption = Settings.Default.HierarchyEnabled;
            IsAutoExpandAllEnabled = Settings.Default.AutoExpandAll;
            IsShowLineNumbersEnabled = Settings.Default.ShowLineNumbersEnabled;
            IsSortingEnabled = Settings.Default.SortingEnabled;
            IsFilterByMarksEnabled = Settings.Default.FilterByMarksEnabled;
            IsHideAnonymousFunctionsEnabled = Settings.Default.HideAnonymousFunctions;
        }
    }
}
