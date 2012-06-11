using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Properties;
using JsParser.Core.UI;

namespace JsParser.Core.Helpers
{
    public static class StatisticsSender
    {
        public static void Send()
        {
            if (Settings.Default.SendStatistics && Settings.Default.SendStatisticsPolitic == "Approved")
            {
                //all is OK - just send it
                StatisticsManager.Instance.SubmitStatisticsToServer();
                return;
            }

            //Ask user if it is normal to send statistics
            if (Settings.Default.SendStatisticsPolitic == "Ask"
             && StatisticsManager.Instance.Statistics.LastSubmittedTime.AddDays(3) < DateTime.UtcNow
             && StatisticsManager.Instance.Statistics.NavigateFromFunctionsTreeCount > 10)
            {
                var askForm = new Form_SendStatisticsConfirmation();
                var result = askForm.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //set OK settings
                    Settings.Default.SendStatisticsPolitic = "Approved";
                    Settings.Default.SendStatistics = true;
                    Settings.Default.Save();

                    //call self recursively
                    Send();
                    return;
                }

                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    //no - user dont want this shit
                    Settings.Default.SendStatisticsPolitic = "Not allowed";
                    Settings.Default.SendStatistics = false;
                    Settings.Default.Save();

                    return;
                }

                if (result == System.Windows.Forms.DialogResult.Ignore)
                {
                    //just do nothing - only update Last submitting time
                    StatisticsManager.Instance.Statistics.LastSubmittedTime = DateTime.UtcNow;
                    Settings.Default.Save();
                    return;
                }
            }
        }
    }
}
