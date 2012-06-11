using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Properties;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace JsParser.Core.Helpers
{
    public class StatisticsManager
    {
        private static StatisticsManager _statMan;
        public Statistics Statistics { get; private set; }

        static StatisticsManager()
        {
            _statMan = new StatisticsManager();
        }

        private StatisticsManager()
        {
            if (string.IsNullOrEmpty(Settings.Default.Statistics))
            {
                Statistics = new Statistics();
                Statistics.FirstInitialize();
            }
            else
            {
                try
                {
                    Statistics = new JavaScriptSerializer().Deserialize<Statistics>(Settings.Default.Statistics);
                }
                catch
                {
                    Statistics = new Statistics();
                    Statistics.FirstInitialize();
                }
            }

            Statistics.UpdateStatisticsOnStart();
        }

        public static StatisticsManager Instance
        {
            get
            {
                return _statMan;
            }
        }

        public void UpdateSettingsWithStatistics()
        {
            Settings.Default.Statistics = new JavaScriptSerializer().Serialize(Statistics);
        }

        public void Save()
        {
            UpdateSettingsWithStatistics();
            Settings.Default.Save();
        }

        public void IgnoreSendingStatistics()
        {
            Statistics.LastSubmittedTime = DateTime.UtcNow;
            Save();
        }

        public void SubmitStatisticsToServer(bool force = false)
        {
            if (force || Statistics.LastSubmittedTime.AddDays(1) < DateTime.UtcNow)
            {
                Save();

                ThreadPool.QueueUserWorkItem(state => {
                    try
                    {
                        //This code is copypasted from http://msdn.microsoft.com/en-us/library/debx8sh9.aspx

                        // Create a request using a URL that can receive a post. 
                        WebRequest request = WebRequest.Create(Settings.Default.StatisticsServerUrl);
                        // Set the Method property of the request to POST.
                        request.Method = "POST";
                        // Create POST data and convert it to a byte array.
                        string postData = new JavaScriptSerializer().Serialize(Statistics);
                        
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                        // Set the ContentType property of the WebRequest.
                        request.ContentType = "application/json";
                        // Set the ContentLength property of the WebRequest.
                        request.ContentLength = byteArray.Length;
                        // Get the request stream.
                        Stream dataStream = request.GetRequestStream();
                        // Write the data to the request stream.
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        // Close the Stream object.
                        dataStream.Close();
                        // Get the response.
                        WebResponse response = request.GetResponse();
                        // Display the status.
                        Trace.WriteLine(((HttpWebResponse)response).StatusDescription);
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Trace.WriteLine(responseFromServer);
                        // Clean up the streams.
                        reader.Close();
                        dataStream.Close();
                        response.Close();

                        var responseData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(responseFromServer);
                        if ((string) responseData["processed"] == "Ok")
                        {
                            Statistics.LastSubmittedTime = DateTime.UtcNow;
                            Save();
                        }
                    }
                    catch
                    {
                    }
                });
            }
        }
    }
}
