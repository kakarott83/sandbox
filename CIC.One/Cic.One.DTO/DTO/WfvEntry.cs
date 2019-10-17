using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Serialization;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    public class WfvEntry
    {
        
        public String syscode { get; set; }
        /// <summary>
        /// Typ der View 0=Liste, 1=Detail, 2=Dashboard mit wfvrefs, 3=Wizard, 4=IFrame
        /// </summary>
        public int entrytype { get; set; }
        public String befehlszeile { get; set; }
        /*[XmlElement(ElementName = "customentry", Type = typeof(CustomEntry))]
        [XmlElement(ElementName = "detailentry", Type = typeof(DetailEntry))]
        [XmlElement(ElementName = "listentry", Type = typeof(ListEntry))]*/
        public CustomEntry customentry { get; set; }
        
        public List<WfvRef> references { get; set; }

        /// <summary>
        /// Permission properties
        /// </summary>
        public String coderfu { get; set; }
        public String codermo { get; set; }

        /// <summary>
        /// Type of Wizard
        /// 0 = WF4
        /// 1 = Panels of Dashboard are displayed as wizard
        /// 2 = BPE Dashboard
        /// </summary>
        public int wizardtype { get; set; }

        //BPE initial event Code
        public String eventCode { get; set; } 
        
    }

    /// <summary>
    /// Used for deserializing from database, must have the same properties as WfvEntry
    /// </summary>
    public class WfvTmpEntry
    {
        public String syscode { get; set; }
        /// <summary>
        /// Typ der View 0=Liste, 1=Detail, 2=Dashboard mit wfvrefs
        /// </summary>
        public int entrytype { get; set; }
        public String befehlszeile { get; set; }
        public String einrichtung { get; set; }

        public String coderfu { get; set; }
        public String codermo { get; set; }

        public WfvEntry getWfvEntry()
        {
            WfvEntry rval = XMLDeserializer.objectFromXml<WfvEntry>(System.Text.Encoding.UTF8.GetBytes(einrichtung), "UTF-8");
            rval.entrytype = entrytype;
            rval.syscode = syscode;
            rval.befehlszeile = befehlszeile;
            if (entrytype == 2)
                rval.befehlszeile = null;
            return rval;
        }
    }
}