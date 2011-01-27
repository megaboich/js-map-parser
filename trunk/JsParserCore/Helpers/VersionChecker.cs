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

namespace JsParserCore.Helpers
{
	public class VersionChecker
	{
		public static bool CheckVersion()
		{
			try
			{
				float thisVersion = 3.1f;
				float repositoryVersion = 0;
				
				var projectSite = @"http://js-addin.googlecode.com";
				//var serverVersionUrl = projectSite + "/svn/Version.xml";
				var serverVersionUrl = @"http://localhost/Version.xml";
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
			catch (Exception ex)
			{
				return false;
			}
		}

		public static float ParseVersion(float thisVersion, string versionXml, out string releaseInfo)
		{
			releaseInfo = string.Empty;
			var xmlDoc = new XmlDocument { InnerXml = versionXml };
			var versionNode = xmlDoc.SelectSingleNode("Version");
			if (versionNode != null)
			{
				float version = 0;
				if (float.TryParse(versionNode.InnerText, out version))
				{
					var versionInfos = versionNode.SelectNodes("//ReleaseInfo");
					if (versionInfos != null)
					{
						var newReleasesInfo = versionInfos.OfType<XmlElement>()
							.Where(e => float.Parse(e.GetAttribute("version")) >= thisVersion)
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
