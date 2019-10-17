using Cic.OpenOne.Common.Util.Serialization;
using Cic.One.DTO;
using Cic.One.Workflow.Activities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WfvXmlConfigurator.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace WfvXmlConfigurator.BO.GUI
{
    /// <summary>
    /// Converts an object of generic type to a xml-string containing the data or the other way around.
    /// </summary>
    /// <typeparam name="T">Type of the object that shall be convertet to/from an xml-string</typeparam>
    public class XmlObjectConverter<T> : ExpandableObjectConverter
    {
        /// <summary>
        /// Log exceptions
        /// </summary>
        private static readonly ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Data types the converter can convert from
        /// </summary>
        /// <param name="sourceType">data type of the object that shall be converted</param>
        /// <returns>whether the given source can be converted to the object</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(T))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Get a xml-string and convert it to the object it's describing
        /// </summary>
        /// <param name="value">xml-string to be converted</param>
        /// <returns>object with data from given xml-string</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is string))
                return base.ConvertFrom(context, culture, value);

            return GetObject((string)value);
        }

        
        /// <summary>
        /// Get a object of the generic type and create a corresponding xml-string
        /// </summary>
        /// <param name="value">object of generic data type</param>
        /// <param name="destinationType">xml-string</param>
        /// <returns>xml-string containing the object data</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(T))
                return GetObject((string)value);

            if (destinationType == typeof(string))
                return GetXml((T)value);

            return base.ConvertTo(context, culture, value, destinationType);

        }

        /// <summary>
        /// create object from xml description
        /// </summary>
        /// <param name="xml">xml string containing object data</param>
        /// <returns>object data</returns>
        private T GetObject(string xml)
        {
            xml = UnmaskSpecialCharacters(xml);
            byte[] data = UTF8Encoding.UTF8.GetBytes(xml);
            try
            {
                T entry = XMLDeserializer.objectFromXml<T>(data, ConstantsDto.ENCODING);
                return entry;
            }
            catch (Exception e)
            {
                string errorinfo = "Fehler beim Umwandeln eines xml-Textes in eine Datenstruktur (" + typeof(T).ToString() + ", " + xml + ")";
                log.Error(errorinfo, e);

                ArgumentException exceptionForUser = new ArgumentException(errorinfo, e);
                throw exceptionForUser;
            }
        }

        /// <summary>
        /// create xml description from object
        /// </summary>
        /// <param name="obj">object data</param>
        /// <returns>xml string containing object data</returns>
        private string GetXml(T obj)
        {
            byte[] data = obj == null ? new byte[0] : XMLSerializer.objectToXml(obj, ConstantsDto.ENCODING);
            string xml = UTF8Encoding.UTF8.GetString(data);
            return UnmaskSpecialCharacters(xml);
        }

        /// <summary>
        /// Turn character codes (e.g. &lt;) to the actual character (e.g. <)
        /// </summary>
        /// <param name="text">text containing character codes</param>
        /// <returns>text with special characters instead of character codes</returns>
        private static string UnmaskSpecialCharacters(string text)
        {
            int indexEnde = text.IndexOf('\0');
            if (indexEnde >= 0)
                text = text.Substring(0, indexEnde);

            if (text.StartsWith("<![CDATA[") && text.EndsWith("]]>"))
            {
                text = text.Substring(9, text.Length - 9 - 3);
            }

            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            text = text.Replace("&apos;", "\'");
            return text;
        }
    }
}
