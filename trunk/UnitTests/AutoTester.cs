using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using JS_addin.Addin.Code;
using JS_addin.Addin.Helpers;
using JS_addin.Addin.Parsers;
using NUnit.Framework;

namespace UnitTests
{
	[TestFixture]
	public class AutoTester
	{
		public string GetEmbeddedText(string resourceName)
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
			Assert.IsNotNull(stream);
			using (var sr = new StreamReader(stream))
			{
				return sr.ReadToEnd();
			}
		}

		public XmlDocument GetEmbeddedXml(string resourceName)
		{
			return new XmlDocument { InnerXml = GetEmbeddedText(resourceName) };
		}

		private void ProcessTemplate(string name)
		{
			var source = GetEmbeddedText("UnitTests.Source." + name + ".js");
			var result = (new JavascriptParser()).Parse(source);

			XmlDocument xml = new XmlDocument() {InnerXml = result.Serialize()};
			xml.Save("C:\\outxml\\" + name + ".xml");

			var resxml = GetEmbeddedText("UnitTests.Result." + name + ".xml");

			var expectedresult = SerializedEntity.Deserialize<Hierachy<CodeNode>>(resxml);

			Assert.IsTrue(result.Equals(expectedresult));
		}

		[Test]
		public void Test1()
		{
			ProcessTemplate("Test1");
		}

		[Test]
		public void Test1_1()
		{
			ProcessTemplate("Test1.1");
		}


		[Test]
		public void Test2()
		{
			ProcessTemplate("Test2");
		}

		[Test]
		public void Test3()
		{
			ProcessTemplate("Test3");
		}

		[Test]
		public void Test4()
		{
			ProcessTemplate("Test4");
		}

		[Test]
		public void Test4_1()
		{
			ProcessTemplate("Test4.1");
		}

		[Test]
		public void Test4_2()
		{
			ProcessTemplate("Test4.2");
		}

		[Test]
		public void Test5()
		{
			ProcessTemplate("Test5");
		}

		[Test]
		public void Test51()
		{
			ProcessTemplate("Test5.1");
		}

		[Test]
		public void Test6()
		{
			ProcessTemplate("Test6");
		}

		[Test]
		public void Test7()
		{
			ProcessTemplate("Test7");
		}

		[Test]
		public void Test8()
		{
			ProcessTemplate("Test8");
		}
	}
}
