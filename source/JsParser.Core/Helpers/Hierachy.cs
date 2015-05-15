using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace JsParser.Core.Helpers
{
    /// <summary>
    /// Represents a hierarchy template.
    /// </summary>
    /// <typeparam name="T">
    /// Type of objects used in hierarchy.
    /// </typeparam>
    [Serializable]
    [XmlRoot("Hierachy")]
    public class Hierarchy<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hierarchy{T}"/> class.
        /// </summary>
        /// <param name="item">
        /// The item to be stored.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        public Hierarchy(T item)
        {
            Item = item;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Hierarchy{T}"/> class from being created.
        /// </summary>
        private Hierarchy()
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
        public List<Hierarchy<T>> Children { get; set; }

        /// <summary>
        /// Gets a value indicating whether HasChildren.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return Children != null && Children.Count > 0;
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
        public Hierarchy<T> Add(T child)
        {
            var h = new Hierarchy<T>(child);

            if (Children == null)
            {
                Children = new List<Hierarchy<T>>();
            }

            Children.Add(h);
            return h;
        }
    }
}