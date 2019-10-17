using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.Util.Serialization
{

    public static class XmlSerializerHelper<T>
    {
        private static volatile XmlSerializer instance;
        public static XmlSerializer Serializer
        {
            get
            {
                if (instance == null)
                {
                    lock (LOCK)
                    {
                        if (instance == null)
                            instance = new XmlSerializer(typeof(T));
                    }
                }

                return instance;
            }
        }
        private static string LOCK = "LOCK";
    }
    /// <summary>
    /// XMLSerializer-Klasse
    /// </summary>
    public static class XMLSerializer
    {

        #region Methods
        private static CacheDictionary<Type, XmlSerializer> serializerCache = CacheFactory<Type, XmlSerializer>.getInstance().createCache(-1);
        private static  XmlSerializerNamespaces  namespaces = null;
        
        
        public static XmlSerializer getSerializer(Type t)
        {
            if (!serializerCache.ContainsKey(t))
                serializerCache[t] = new XmlSerializer(t);
            return serializerCache[t];
        }

        /// <summary>
        /// Convert the object to an xml-string as byte array
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] objectToXml(Object data, String encoding)
        {
            try
            {
               if(data==null) return null;
                
                XmlSerializer calcSerializer = XMLSerializer.getSerializer(data.GetType());
                
                System.IO.StringWriter stWrite = new StringWriterEncoded(Encoding.GetEncoding(encoding));
                calcSerializer.Serialize(stWrite, data);
                stWrite.Close();

                return Encoding.GetEncoding(encoding).GetBytes(stWrite.ToString());
            }
            catch (Exception ex)
            {
                String err = "XML-Serialization-Error: " + ((ex.InnerException != null) ? ex.InnerException.Message : "");
                Console.WriteLine("XML-Serialization-Error: ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Write an object to a xml string given the encoding
        /// </summary>
        /// <param name="xmlObject"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Serialize(object xmlObject, String encoding)
        {
            if (xmlObject == null) return null;
            XmlSerializer calcSerializer = XMLSerializer.getSerializer(xmlObject.GetType());
            System.IO.StringWriter stWrite = new StringWriterEncoded(Encoding.GetEncoding(encoding));
            calcSerializer.Serialize(stWrite, xmlObject);
            stWrite.Close();
            return stWrite.ToString();
        }

        //  "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
        /// <summary>
        /// SerializeUTF8FormattedNoDeclaration
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
        public static string SerializeUTF8FormattedNoDeclaration(object value)
        {
            try
            {
                // My
                return MySerializeUTF8(value, true, false);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
        /// <summary>
        /// SerializeUTF8WithoutNamespace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
        public static string SerializeUTF8WithoutNamespace(object value)
        {
            try
            {
                // My
                return MySerializeUTF8(value, false, true);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
        /// <summary>
        /// SerializeUTF8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
        public static string SerializeUTF8(object value)
        {
            try
            {
                // My
                return MySerializeUTF8(value, false, false);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        /// <summary>
        /// DeserializeUTF8
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
        public static object DeserializeUTF8(string value, System.Type type)
        {
            try
            {
                // My
                return MyDeserializeUTF8(value, type);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

        #region My methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
        private static string MySerializeUTF8(object value, bool formattedNoDeclaration, bool withoutNamespace)
        {
            // Check value
            if (value == null)
                return null;
            System.Text.UTF8Encoding UTF8Encoding;
            System.IO.MemoryStream MemoryStream;
            System.Xml.Serialization.XmlSerializer XmlSerializer;
            System.Xml.XmlTextWriter XmlTextWriter;
            
            string Result = null;


            // New memory stream
            using (MemoryStream = new System.IO.MemoryStream())
            {
                try
                {
                    // New serializer
                    XmlSerializer = XMLSerializer.getSerializer(value.GetType());
                    XmlSerializer = serializerCache[value.GetType()];
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }

                // New encoding
                UTF8Encoding = new System.Text.UTF8Encoding();

                // Check state
                if (formattedNoDeclaration)
                {
                    // New xml writer
                    XmlTextWriter = new XmlTextWriterFormattedNoDeclaration(MemoryStream, UTF8Encoding);
                }
                else
                {
                    // New xml writer
                    XmlTextWriter = new System.Xml.XmlTextWriter(MemoryStream, UTF8Encoding);
                }

                try
                {
                    // Check state
                    if (withoutNamespace)
                    {
                        if(namespaces==null){
                                            namespaces =  new System.Xml.Serialization.XmlSerializerNamespaces();
                                             // Add empty namspace
                                            namespaces.Add("", "");
                            }
       
                        // Serialize
                        XmlSerializer.Serialize(XmlTextWriter, value, namespaces);
                    }
                    else
                    {
                        // Serialize
                        XmlSerializer.Serialize(XmlTextWriter, value);
                    }
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }

                // Get string
                Result = UTF8Encoding.GetString(MemoryStream.ToArray());
                // Close stream
                MemoryStream.Close();
            }

            return Result;
        }

        /// <summary>
        /// MyDeserializeUTF8
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
        private static object MyDeserializeUTF8(string value, System.Type type)
        {
            System.Text.UTF8Encoding UTF8Encoding;
            System.IO.MemoryStream MemoryStream;
            System.Xml.Serialization.XmlSerializer XmlSerializer;
            object Result = null;

            // Check value
            if (value != null)
            {
                // New encoding
                UTF8Encoding = new System.Text.UTF8Encoding();
                // New memory stream
                using (MemoryStream = new System.IO.MemoryStream(UTF8Encoding.GetBytes(value)))
                {
                    // New serializer
                    XmlSerializer = XMLSerializer.getSerializer(type);
                    // Deserialize
                    Result = XmlSerializer.Deserialize(MemoryStream);
                    // Close stream
                    MemoryStream.Close();
                }
            }
            return Result;
        }
        #endregion
    }

    internal sealed class XmlTextWriterFormattedNoDeclaration : System.Xml.XmlTextWriter
    {
        #region Constructors

        internal XmlTextWriterFormattedNoDeclaration(System.IO.Stream stream, System.Text.Encoding encoding)
            : base(stream, encoding)
        {
            // Set formatting
            this.Formatting = System.Xml.Formatting.Indented;
        }
        #endregion

        public override void WriteStartDocument()
        {
            // Do nothing
        }
    }
}