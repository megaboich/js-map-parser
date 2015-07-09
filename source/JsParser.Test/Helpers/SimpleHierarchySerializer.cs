using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using JsParser.Core.Helpers;

namespace JsParser.Test.Helpers
{
    public static class SimpleHierarchySerializer
    {
        private class DeserializedRawItem<T>
        {
            public T Item { get; set; }
            public int Level { get; set; }
        }

        public static string Serialize<T>(Hierarchy<T> hierarchy)
            where T : IEquatable<T>
        {
            using (var tw = new StringWriter())
            {
                SerializeCurrent(hierarchy, 0, tw);
                return tw.GetStringBuilder().ToString();
            }
        }

        private static void SerializeCurrent<T>(Hierarchy<T> hierarchy, int nestLevel, TextWriter writer)
            where T : IEquatable<T>
        {
            var props = hierarchy.Item.GetType().GetProperties();
            foreach (var p in props)
            {
                var ignoreAttrs = p.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                var isIgnored = ignoreAttrs != null && ignoreAttrs.Any();
                if (!isIgnored)
                {
                    var val = p.GetValue(hierarchy.Item, null);
                    if (val != null && !(val is string && string.IsNullOrEmpty((string)val)))
                    {
                        var escaped = System.Web.HttpUtility.HtmlEncode(val.ToString())
                            .Replace("\r", "&#D;")
                            .Replace("\n", "&#A;")
                            .Replace("\t", "&#9;");

                        writer.WriteLine(new String('\t', nestLevel) + p.Name + ": " + escaped);
                    }
                }
            }
            writer.WriteLine();
            if (hierarchy.HasChildren)
            {
                foreach (var ch in hierarchy.Children)
                {
                    SerializeCurrent(ch, nestLevel + 1, writer);
                }
            }
        }

        public static Hierarchy<T> Deserialize<T>(string serialized)
            where T : IEquatable<T>, new()
        {
            var rawItems = new List<DeserializedRawItem<T>>();
            using (var tr = new StringReader(serialized))
            {
                string line;
                DeserializedRawItem<T> currentItem = null;
                int lineNum = 0;
                while ((line = tr.ReadLine()) != null)
                {
                    lineNum++;
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        currentItem = null;
                        continue;
                    }

                    if (currentItem == null)
                    {
                        // Parse the line level
                        int lineLevel = 0;
                        for (; lineLevel < line.Length && line[lineLevel] == '\t'; ++lineLevel) ;

                        currentItem = new DeserializedRawItem<T>()
                        {
                            Item = new T(),
                            Level = lineLevel
                        };
                        rawItems.Add(currentItem);
                    }

                    // Parse the property name and value
                    line = line.Trim();
                    var propLength = line.IndexOf(":");
                    if (propLength <= 0)
                    {
                        throw new Exception(string.Format("Line {0}: Missed property name separated by ':' symbol", lineNum));
                    }

                    var propName = line.Substring(0, propLength);
                    var propValue = line.Substring(propLength + 1).Trim();
                    propValue = System.Web.HttpUtility.HtmlDecode(propValue.Replace("&#D;", "\r").Replace("&#A;", "\n"));

                    var type = currentItem.Item.GetType();
                    var prop = type.GetProperty(propName);
                    if (prop == null)
                    {
                        throw new Exception(string.Format("Line {0}: Wrong property name: '{1}'", lineNum, propName));
                    }

                    object newPropValue;
                    if (prop.PropertyType.IsEnum)
                    {
                        try
                        {
                            newPropValue = Enum.Parse(prop.PropertyType, propValue, true);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Line {0}: Wrong property value: '{1}'", lineNum, propValue), ex);
                        }
                    }
                    else
                    {
                        newPropValue = Convert.ChangeType(propValue, prop.PropertyType, CultureInfo.InvariantCulture);
                    }
                    
                    prop.SetValue(currentItem.Item, newPropValue, null);
                }
            }

            return BuildHierarchy(rawItems);
        }

        private static Hierarchy<T> BuildHierarchy<T>(List<DeserializedRawItem<T>> rawItems)
            where T : IEquatable<T>, new()
        {
            if (rawItems == null || rawItems.Count == 0)
            {
                return null;
            }

            var firstItem = rawItems[0];
            if (firstItem.Level != 0)
            {
                throw new Exception("First item should be placed without indentation");
            }

            var parentsStack = new Stack<Hierarchy<T>>();
            var levelsStack = new Stack<int>();
            var rootHierarhy = new Hierarchy<T>(firstItem.Item);
            parentsStack.Push(rootHierarhy);
            levelsStack.Push(0);
            
            for (var i = 1; i < rawItems.Count; ++i)
            {
                var currentNode = parentsStack.Peek();
                var nodeLevel = levelsStack.Peek();
                var currentRawItem = rawItems[i];
                var currentLevel = currentRawItem.Level;

                while (currentLevel <= nodeLevel)
                {
                    parentsStack.Pop();
                    levelsStack.Pop();

                    currentNode = parentsStack.Peek();
                    nodeLevel = levelsStack.Peek();
                }

                parentsStack.Push(currentNode.Add(currentRawItem.Item));
                levelsStack.Push(currentLevel);
                continue;
            }

            return rootHierarhy;
        }
    }
}
