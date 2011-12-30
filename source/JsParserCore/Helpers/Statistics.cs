using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace JsParserCore.Helpers
{
    public class Statistics
    {
        public Guid ClientId { get; set; }

        public int TotalRunCount { get; set; }

        public DateTime FirstDateUse { get; set; }

        public DateTime LastSubmittedTime { get; set; }

        public string Version {get; set;}

        /// <summary>
        /// String identifies container which runs a javascript parser - VS2010, VS2011, VS2008, or other.
        /// </summary>
        public string Container { get; set; }

        public Statistics()
        {
            
        }

        public void FirstInitialize()
        {
            ClientId = Guid.NewGuid();
            FirstDateUse = DateTime.UtcNow;
            Version = new AssemblyName(typeof(Statistics).Assembly.FullName).Version.ToString();
        }

        public void UpdateStatistics()
        {
            ++TotalRunCount;
        }
    }
}
