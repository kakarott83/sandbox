using AutoMapper;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Serialization;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using XmlConfiguratorBase.BO.ContentLogics;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase.BO.GUI
{
    /// <summary>
    /// Interaction logic for SourceControl.xaml
    /// </summary>
    public partial class SourceControl : UserControl
    {
        /// <summary>
        /// xml representing the object
        /// </summary>
        public string Xml
        {
            get
            {
                return SourceXml.Text;
            }
            set
            {
                SourceXml.Text = value;
            }
        }

        private XmlObjectConverter<object> ObjectConverter = new XmlObjectConverter<object>();
        private XmlObjectConverter<WfvEntry> EntryConverter = new XmlObjectConverter<WfvEntry>();
        private XmlObjectConverter<WfvConfigEntry> ConfigConverter = new XmlObjectConverter<WfvConfigEntry>();


        public SourceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// check if text is valid xml
        /// </summary>
        /// <returns>true: text is valid xml</returns>
        public bool IsValid()
        {
            try
            {
                if (ParseXml() == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// get the parsed xml as object
        /// </summary>
        /// <returns>object described by the xml</returns>
        public object ParseXml()
        {
            if (Xml.Contains("</WfvEntry>"))
                return EntryConverter.ConvertFrom(Xml);
            else if (Xml.Contains("</WfvConfigEntry>"))
            {
                byte[] data = Encoding.UTF8.GetBytes(Xml);
                WfvConfigEntry config = XMLDeserializer.objectFromXml<WfvConfigEntry>(data, ConstantsDto.ENCODING);
                WfvConfigEntryDto configDto = Mapper.Map<WfvConfigEntry, WfvConfigEntryDto>(config);
                configDto.einrichtung = ContentManager.GetEinrichtungObject(config);
                return configDto;
            }
            else
                return null;
        }

        /// <summary>
        /// Set the xml encoded object to the given one
        /// </summary>
        /// <param name="ObjectToEncode">object that is supposed to be turned into xml</param>
        public void SetXmlObject(object ObjectToEncode)
        {
            if (ObjectToEncode is WfvEntry)
                Xml = (string)EntryConverter.ConvertTo(ObjectToEncode, typeof(string));
            else if (ObjectToEncode is WfvConfigEntryDto)
            {
                WfvConfigEntry configentry = Mapper.Map<WfvConfigEntryDto, WfvConfigEntry>((WfvConfigEntryDto)ObjectToEncode);
                configentry.einrichtung = (string)ObjectConverter.ConvertTo(((WfvConfigEntryDto)ObjectToEncode).einrichtung, typeof(string));
                //Mask xml text value for xml file
                if (configentry.einrichtung != null)
                    configentry.einrichtung = "<![CDATA[" + configentry.einrichtung + "]]>";
                Xml = (string)ConfigConverter.ConvertTo(configentry, typeof(string));
            }
            else if (ObjectToEncode is WfvConfigEntry)
            {
                Xml = (string)ConfigConverter.ConvertTo(ObjectToEncode, typeof(string));
            }
            else
                Xml = "";
        }
    }
}
