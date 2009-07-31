using System;
using System.Collections.Generic;
using System.Text;

namespace JS_addin.Addin.Code
{
	/// <summary>
	/// Represents a hierarchy template.
	/// </summary>
	/// <typeparam name="T">
	/// Type of objects used in hierarchy.
	/// </typeparam>
	public class Hierachy<T>
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
		public Hierachy(T item, Hierachy<T> parent)
		{
			Item = item;
			Parent = parent;
		}

		/// <summary>
		/// Gets or sets Item.
		/// </summary>
		public T Item { get; set; }

		/// <summary>
		/// Gets or sets Parent.
		/// </summary>
		public Hierachy<T> Parent { get; set; }

		/// <summary>
		/// Gets Childrens.
		/// </summary>
		public List<Hierachy<T>> Childrens { get; private set; }

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
			var h = new Hierachy<T>(child, this);

			if (Childrens == null)
			{
				Childrens = new List<Hierachy<T>>();
			}

			Childrens.Add(h);
			return h;
		}
	}
}
