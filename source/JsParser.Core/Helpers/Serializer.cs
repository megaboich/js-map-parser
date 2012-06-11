using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace JsParser.Core.Helpers
{
	public static class Serializer
	{
		public static string Serialize(object obj)
		{
			var xs = new XmlSerializer(obj.GetType(), string.Empty);
			var ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, string.Empty);
			var sw = new StringWriter();
			var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true });
			xs.Serialize(xw, obj, ns);
			return sw.ToString();
		}

		public static object Deserialize(string serializedSchema, Type type)
		{
			var xs = new XmlSerializer(type, string.Empty);
			return xs.Deserialize(XmlReader.Create(new StringReader(serializedSchema)));
		}

		public static T Deserialize<T>(string serializedSchema)
		{
			return (T)Deserialize(serializedSchema, typeof(T));
		}
	}
}
