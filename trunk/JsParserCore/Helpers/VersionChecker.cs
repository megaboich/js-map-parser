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

namespace JsParcerCore.Helpers
{
	public class VersionChecker
	{
		public static bool CheckVersion()
		{
			try
			{
				int thisVersion = 1;
				int repositoryVersion = 0;
				
				var projectSite = @"http://js-addin.googlecode.com";
				var serverVersionUrl = projectSite + "/svn/Version.xml";

				using (var sr = new StreamReader(WebRequest.Create(serverVersionUrl).GetResponse().GetResponseStream()))
				{
					repositoryVersion = ParseVersion(sr.ReadToEnd());
				}

				if (thisVersion == repositoryVersion)
				{
					return false;
				}

				if (thisVersion > repositoryVersion)
				{
					MessageBox.Show(string.Format("js parser version checker. Your version {0}, repository version {1}. Good luck", thisVersion, repositoryVersion));
				}

				if (thisVersion < repositoryVersion)
				{
					var result = MessageBox.Show(string.Format("A new version of Javascript parser addin is available. Would you like to visit site {0} ?", projectSite),
						"JSparser version checker",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Information);

					if (result == DialogResult.Yes)
					{
						Process.Start(projectSite);
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static int ParseVersion(string versionXml)
		{
			var xmlDoc = new XmlDocument { InnerXml = versionXml };
			var versionNode = xmlDoc.SelectSingleNode("Version");
			if (versionNode != null)
			{
				int version = 0;
				if (int.TryParse(versionNode.InnerText, out version))
				{
					return version;
				}
			}

			return 0;
		}
	}
}
