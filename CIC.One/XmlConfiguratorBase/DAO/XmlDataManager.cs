using System;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Serialization;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase.DAO
{
    public class XmlDataManager : IDataManager
    {
        private string Xml { get; set; }
        private byte[] XmlBytes
        {
            get
            {
                return System.Text.Encoding.UTF8.GetBytes(Xml);
            }
        }

        public XmlDataManager(string xml)
        {
            Xml = xml;
        }

        public WfvConfig ReadData()
        {
            WfvConfig config = new WfvConfig();
            config.entries = new System.Collections.Generic.List<WfvEntry>();
            config.configentries = new System.Collections.Generic.List<WfvConfigEntry>();

            if (Xml.Contains("</WfvEntry>"))
                config.entries.Add(XMLDeserializer.objectFromXml<WfvEntry>(XmlBytes, ConstantsDto.ENCODING));
            else if (Xml.Contains("</WfvConfigEntry>"))
                config.configentries.Add(XMLDeserializer.objectFromXml<WfvConfigEntry>(XmlBytes, ConstantsDto.ENCODING));
            else
                throw new FormatException("The xml must contain a WfvEntry or a WfvConfigEntry");

            return config;
        }

        public void SaveData(WfvConfig data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
