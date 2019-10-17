using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Dynamic;

namespace Cic.OpenLease.Service.Services.DdOl.BO
{
    public class XMLToObjectBo
    {
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
                if(o is List<object>)
                    result = o;
            }
            return result;
        }

    }
}