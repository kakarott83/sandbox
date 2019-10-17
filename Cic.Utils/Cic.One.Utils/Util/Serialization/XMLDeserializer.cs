using System.IO;
using System.Xml.Serialization;
using System;
using System.Text;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.Util.Serialization
{
    /// <summary>
    /// XMLDeserializer-Klasse
    /// </summary>
    public class XMLDeserializer
    {
       
        /// <summary>
        /// Creates the Object-Representation of the given byte-Array containing XML-Data
        /// </summary>
        /// <typeparam name="T">The Type the data will be converted to</typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T deserializeFromXML<T>(byte[] param)
        {
            
           using( Stream byteStream = new MemoryStream(param)){
                XmlSerializer calcSerializer = XMLSerializer.getSerializer(typeof(T));

                TextReader reader = new StreamReader(byteStream);
                T request = (T)calcSerializer.Deserialize(reader);
                reader.Close();
                return request;
            }
        }

        /// <summary>
        /// Convert a byte stream to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T objectFromXml<T>(byte[] data, String encoding)
        {
            using (Stream byteStream = new MemoryStream(data))
            {
                XmlSerializer calcSerializer = XMLSerializer.getSerializer(typeof(T));

                TextReader reader = new StreamReader(byteStream, Encoding.GetEncoding(encoding));
                return (T)calcSerializer.Deserialize(reader);
            }

        }
       
    }
}