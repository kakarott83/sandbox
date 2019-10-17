using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Xml.Linq;

namespace Cic.One.Utils.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpandoUtil
    {
        /// <summary>
        /// Erzeugt ein Expando aus einem Dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static object GetExpando<T>(Dictionary<string, T> dictionary)
        {
            return GetExpandoFromDictionary(dictionary.ToDictionary(pair => pair.Key, pair => (object)pair.Value));
        }

        /// <summary>
        /// Fügt zwei Dictionaries zu einem Expando zusammen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="dictionary_2"></param>
        /// <returns></returns>
        internal static object GetExpando<T, S>(Dictionary<string, T> dictionary, Dictionary<string, S> dictionary_2)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, T>();
            }
            if (dictionary_2 == null)
            {
                dictionary_2 = new Dictionary<string, S>();
            }
            var expando = (ExpandoObject)GetExpando(dictionary);
            return AddToExpando(expando, dictionary_2.ToDictionary(pair => pair.Key, pair => (object)pair.Value));
        }

        /// <summary>
        /// Fügt einem Expando Objekt ein Dictionary hinzu
        /// </summary>
        /// <param name="dictionary">Dictionary</param>
        /// <returns></returns>
        public static ExpandoObject AddToExpando(ExpandoObject expando, IDictionary<string, object> dictionary)
        {
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = GetExpandoFromDictionary((IDictionary<string, object>)kvp.Value);
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = GetExpandoFromDictionary((IDictionary<string, object>)item);
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }
            return expando;
        }


        /// <summary>
        /// Erzeugt ein Expando aus einem Dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static ExpandoObject GetExpandoFromDictionary(IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            AddToExpando(expando, dictionary);
            return expando;

        }

        /// <summary>
        /// Erzeugt ein dynamisches Objekt anhand einer XML-Datei
        /// </summary>
        /// <param name="xml">XML-Daten</param>
        /// <returns>Generiertes Objekt</returns>
        public static object GetExpandoFromXml(string xml)
        {
            var doc = XDocument.Parse(xml);
            dynamic obj = Process(doc.Root);
            return obj;
        }

        /// <summary>
        /// Erzeugt ein dynamisches Objekt anhand eines XElement
        /// </summary>
        /// <param name="element">Element, für welches das Objekt zurückgeliefert werden soll</param>
        /// <returns>Generiertes Objekt</returns>
        private static object Process(XElement element)
        {
            dynamic result = new ExpandoObject();
            int count = 0;
            foreach (var n in element.Attributes())
            {
                count++;
                (result as IDictionary<String, object>)[n.Name.ToString()] = n.Value;
            }

            IDictionary<String, object> dict = (result as IDictionary<String, object>);

            foreach (var n in element.Elements())
            {
                count++;

                if (dict.ContainsKey(n.Name.ToString()))
                {
                    if (dict[n.Name.ToString()] is List<object>)
                    {
                        (dict[n.Name.ToString()] as List<object>).Add(Process(n));
                    }
                    else
                    {
                        dict[n.Name.ToString()] = new List<object>() { dict[n.Name.ToString()], Process(n) };
                    }
                }
                else
                {
                    dict[n.Name.ToString()] = Process(n);
                }
            }
            if (count == 0)
            {
                result = element.Value.ToString();
            }
            //Falls es nur ein Unterelement gibt und es eine Liste ist, wird es zurückgegeben
            //Bsp: <rows><row>1</row><row>4</row><row>10</row></rows>. Dadurch wird die Liste bei rows gespeichert und nicht bei rows.row
            if (dict.Keys.Count == 1)
            {
                object o = dict.First();
                if (o is List<object>)
                    result = o;
            }
            return result;
        }
    }
}
