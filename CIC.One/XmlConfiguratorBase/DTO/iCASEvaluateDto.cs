//using Cic.OpenOne.Common.DTO;
using System.ComponentModel;
using System.Xml.Serialization;
using XmlConfiguratorBase.BO.GUI;

namespace XmlConfiguratorBase.DTO
{
    /// <summary>
    /// Class for configurating a CAS command object.
    /// The original object cannot be used because sysID is a long array, but in the xml values like "{{$object.areaid}}" need to be stored.
    /// </summary>
    [TypeConverter(typeof(XmlObjectConverter<iCASEvaluateDto>))]
    public class iCASEvaluateDto
    {
        [Description("Bereich")]
        public string area { get; set; }

        [Description("Ausführungs-ID")]
        public long execID { get; set; }

        [Description("CAS-Ausdrücke")]
        [Editor(typeof(StringArrayEditor), typeof(StringArrayEditor))]
        public string[] expression { get; set; }

        [Description("Parameter")]
        [Editor(typeof(StringArrayEditor), typeof(StringArrayEditor))]
        public string[] param { get; set; }

        [Description("System-IDs")]
        [Editor(typeof(StringArrayEditor), typeof(StringArrayEditor))]
        [XmlArrayItem("long")]
        public string[] sysID { get; set; }

        [Description("URL")]
        public string url { get; set; }
    }
}
