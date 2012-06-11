using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JsParser.Core.Helpers
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
			return Serializer.Deserialize(serializedSchema, type);
		}

		/// <summary>
		/// The serialize.
		/// </summary>
		/// <returns>
		/// The serialized string.
		/// </returns>
		public string Serialize()
		{
			return Serializer.Serialize(this);
		}
	}
}