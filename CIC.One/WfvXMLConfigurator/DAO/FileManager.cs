using System;
using System.IO;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.Common.Util;
using System.Xml.Linq;
using WfvXmlConfigurator.DTO;
using WfvXmlConfigurator.BO.GUI;

namespace WfvXmlConfigurator.DAO
{
    /// <summary>
    /// Manager for getting the raw data from xml
    /// </summary>
    internal class FileManager : IDataManager
    {
        private string m_FilePath = "";

        /// <summary>
        /// XML file manager
        /// </summary>
        /// <param name="path">Location of xml file</param>
        internal FileManager(string path)
        {
            m_FilePath = path;
        }

        /// <summary>
        /// Read the xml objects
        /// </summary>
        /// <returns>Configuration described by the xml file</returns>
        public WfvConfig ReadData()
        {
            WfvConfig config = null;

            byte[] data = null;
            data = FileUtils.loadData(m_FilePath);
            config = XMLDeserializer.objectFromXml<WfvConfig>(data, ConstantsDto.ENCODING);

            return config;
        }

        /// <summary>
        /// Save data as xml to opened file
        /// </summary>
        /// <param name="data">Configuration data</param>
        public void SaveData (WfvConfig data)
        {
            if (data == null)
            {
                Console.WriteLine("Keine Daten vorhanden.");
                return; //If there is no content, there is nothing to save
            }

            foreach (WfvConfigEntry configentry in data.configentries)
            {
                //Mask xml text value for xml file
                if (configentry.einrichtung != null)
                    configentry.einrichtung = "<![CDATA[" + configentry.einrichtung + "]]>";
            }

            XmlObjectConverter<WfvConfig> converter = new XmlObjectConverter<WfvConfig>();
            string contentText = (string)converter.ConvertTo(data, typeof(string));

            XDocument xmlParser = XDocument.Parse(contentText);
            StreamWriter streamwriter = new StreamWriter(m_FilePath, false);
            streamwriter.Write(xmlParser.ToString());
            streamwriter.Close();
        }

        /// <summary>
        /// There is no open connection to the file, so nothing needs to be done for closing
        /// </summary>
        public void Dispose()
        {
        }
    }
}
