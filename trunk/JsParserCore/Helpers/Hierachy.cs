using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace JsParserCore.Helpers
{
	/// <summary>
	/// Represents a hierarchy template.
	/// </summary>
	/// <typeparam name="T">
	/// Type of objects used in hierarchy.
	/// </typeparam>
	[Serializable]
	[XmlRoot("Hierachy")]
	public class Hierachy<T> : SerializedEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Hierachy{T}"/> class.
		/// </summary>
		/// <param name="item">
		/// The item to be stored.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		public Hierachy(T item)
		{
			Item = item;
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="Hierachy{T}"/> class from being created.
		/// </summary>
		private Hierachy()
		{
		}

		/// <summary>
		/// Gets or sets Item.
		/// </summary>
		public T Item { get; set; }

		/// <summary>
		/// Gets or sets Childrens.
		/// </summary>
		[XmlArrayItem(ElementName = "Hierachy", IsNullable = true)]
		public List<Hierachy<T>> Childrens { get; set; }

		/// <summary>
		/// Gets a value indicating whether HasChildrens.
		/// </summary>
		public bool HasChildrens
		{
			get
			{
				return Childrens != null && Childrens.Count > 0;
			}
		}

		/// <summary>
		/// Adds set of childrens to current hierarchy node.
		/// </summary>
		/// <param name="childs">
		/// The childs.
		/// </param>
		public void Add(IEnumerable<T> childs)
		{
			foreach (T child in childs)
			{
				Add(child);
			}
		}

		/// <summary>
		/// Adds one child to current hierarchy node.
		/// </summary>
		/// <param name="child">
		/// The child.
		/// </param>
		/// <returns>
		/// The add children.
		/// </returns>
		public Hierachy<T> Add(T child)
		{
			var h = new Hierachy<T>(child);

			if (Childrens == null)
			{
				Childrens = new List<Hierachy<T>>();
			}

			Childrens.Add(h);
			return h;
		}

		/// <summary>
		/// Equals custom implementation.
		/// </summary>
		/// <param name="obj">
		/// The obj parameter.
		/// </param>
		/// <returns>
		/// Bool result.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			if (!(obj is Hierachy<T>))
			{
				return false;
			}

			var h = (Hierachy<T>) obj;

			if (!Item.Equals(h.Item))
			{
				return false;
			}

			int mycount = Childrens != null ? Childrens.Count : 0;
			int hiscount = h.Childrens != null ? h.Childrens.Count : 0;

			if (mycount + hiscount == 0)
			{
				return true;
			}

			if (mycount != hiscount)
			{
				return false;
			}

			for (int index = 0; index < Childrens.Count; ++index)
			{
				if (!(Childrens[index].Equals(h.Childrens[index])))
				{
					return false;
				}
			}

			return true;
		}
	}
}