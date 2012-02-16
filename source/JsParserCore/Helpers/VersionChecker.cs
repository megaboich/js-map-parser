using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using JsParserCore.Properties;
using System.Threading;

namespace JsParserCore.Helpers
{
	public class VersionChecker
	{
		private static bool _versionChecked = false;

		public static void CheckVersion(bool force = false)
		{
			if (Settings.Default.CheckForVersionUpdates || force)
			{
				ThreadPool.QueueUserWorkItem((object state) =>
				{
					try
					{
						VersionChecker._CheckVersion(force);
					}
					catch
					{
					}
				});
				
			}
		}

		private static bool _CheckVersion(bool force = false)
		{
			if (force || !_versionChecked)
			{
				try
				{
					_versionChecked = true;

					float thisVersion = 3.5f;
					float repositoryVersion = 0;

					var projectSite = @"http://js-addin.googlecode.com";
					var serverVersionUrl = projectSite + "/svn/Version.xml";
					//var serverVersionUrl = @"http://localhost/Version.xml";
					var releaseInfo = string.Empty;

					using (var sr = new StreamReader(WebRequest.Create(serverVersionUrl).GetResponse().GetResponseStream()))
					{
						repositoryVersion = ParseVersion(thisVersion, sr.ReadToEnd(), out releaseInfo);
					}

					if (thisVersion >= repositoryVersion)
					{
						return false;
					}

					var result = MessageBox.Show(string.Format("A new version of Javascript parser addin is available.\r\n\r\n{1}\r\n\r\n Would you like to visit site {0} ?", projectSite, releaseInfo),
						"JSparser version checker",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Information);

					if (result == DialogResult.Yes)
					{
						Process.Start(projectSite);
					}

					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}

			return false;
		}

		public static float ParseVersion(float thisVersion, string versionXml, out string releaseInfo)
		{
			releaseInfo = string.Empty;
			var xmlDoc = new XmlDocument { InnerXml = versionXml };
			var versionNode = xmlDoc.SelectSingleNode("Version");
			if (versionNode != null)
			{
				float version = 0;
				if (float.TryParse(versionNode.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out version))
				{
					var versionInfos = versionNode.SelectNodes("//ReleaseInfo");
					if (versionInfos != null)
					{
						var newReleasesInfo = versionInfos.OfType<XmlElement>()
							.Where(e => float.Parse(e.GetAttribute("version")) > thisVersion)
							.SelectMany(e => e.SelectNodes("self::*//info").OfType<XmlElement>())
							.Select(e => e.GetAttribute("text"))
							.ToArray();
						releaseInfo = String.Join("\r\n", newReleasesInfo);
					}
					return version;
				}
			}

			return 0;
		}
	}
}
