using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Helpers
{
    public static class HierarchyComparer
    {
        /// <summary>
        /// Compares two hierarchies
        /// </summary>
        /// <typeparam name="T">Type of hierarchy item</typeparam>
        /// <param name="one">One hierarchy</param>
        /// <param name="other">Other hierarchy</param>
        /// <returns>True if equals, false otherwise</returns>
        public static bool Compare<T>(Hierarchy<T> one, Hierarchy<T> other, IComparer<T> comparer)
        {
            if (one == null || other == null)
            {
                return false;
            }

            if (comparer.Compare(one.Item, other.Item) != 0)
            {
                return false;
            }

            int oneCount = one.Children != null ? one.Children.Count : 0;
            int otherCount = other.Children != null ? other.Children.Count : 0;

            if (oneCount == 0 && otherCount == 0)
            {
                return true;
            }

            if (oneCount != otherCount)
            {
                return false;
            }

            for (int index = 0; index < oneCount; ++index)
            {
                if (!HierarchyComparer.Compare(one.Children[index], other.Children[index], comparer))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
