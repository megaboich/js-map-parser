using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JSparser.Helpers
{
	/// <summary>
	/// The serialized entity.
	/// </summary>
	[Serializable]
	public abstract class SerializedEntity
	{
		/// <summary>
		/// The deserialize.
		/// </summary>
		/// <param name="serializedSchema">
		/// The serialized schema.
		/// </param>
		/// <typeparam name="T">
		/// Type of items.
		/// </typeparam>
		/// <returns>
		/// Object of specified type.
		/// </returns>
		public static T Deserialize<T>(string serializedSchema)
		{
			return (T) Deserialize(serializedSchema, typeof(T));
		}

		/// <summary>
		/// The deserialize.
		/// </summary>
		/// <param name="serializedSchema">
		/// The serialized schema.
		/// </param>
		/// <param name="type">
		/// The type param.
		/// </param>
		/// <returns>
		/// The deserialized object.
		/// </returns>
		public static object Deserialize(string serializedSchema, Type type)
		{
			var xs = new XmlSerializer(type, string.Empty);
			return xs.Deserialize(XmlReader.Create(new StringReader(serializedSchema)));
		}

		/// <summary>
		/// The serialize.
		/// </summary>
		/// <returns>
		/// The serialized string.
		/// </returns>
		public string Serialize()
		{
			var xs = new XmlSerializer(GetType(), string.Empty);
			var ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, string.Empty);
			var sw = new StringWriter();
			var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true });
			xs.Serialize(xw, this, ns);
			return sw.ToString();
		}
	}
}