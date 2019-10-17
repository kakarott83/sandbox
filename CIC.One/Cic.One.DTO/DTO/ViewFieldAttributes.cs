using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    /// <summary>
    /// holds all attributes describing a generic gui input field element
    /// </summary>
    public class ViewFieldAttributes
    {
        public ViewFieldAttributes() { }

        public ViewFieldAttributes clone()
        {
            ViewFieldAttributes rval = new ViewFieldAttributes();
            rval.field = field;
            rval.label = label;
            rval.type = type;
            rval.viewtype = viewtype;
            rval.ro = ro;
            rval.maximum = maximum;
            rval.minimum = minimum;
            rval.pattern = pattern;
            rval.suffix = suffix;
            rval.req = req;
            rval.code = code;
            rval.translate = translate;
            return rval;
        }

        /// <summary>
        /// Query ALIAS Or DB Column
        /// </summary>
        [XmlAttribute]
        public String field { get; set; }

        /// <summary>
        /// GUI Label
        /// </summary>
        [XmlAttribute]
        public String label { get; set; }
        /// <summary>
        /// Internal Type (String, Long, DateTime, Int, Double, Byte)
        /// see enum InternalType in WfvXmlConfigurator
        /// </summary>
        [XmlAttribute]
        public String type { get; set; }
        /// <summary>
        /// GUI Type (text,boolean,time,date,currency,separator,number,xpro,headline,textarea,html)
        /// </summary>
        [XmlAttribute]
        public String viewtype { get; set; }
        /// <summary>
        /// Readonly
        /// </summary>
        [XmlAttribute]
        public int ro { get; set; }
        /// <summary>
        /// Number minimum
        /// </summary>
        [XmlAttribute]
        public double minimum { get; set; }
        /// <summary>
        /// Number Maximum
        /// </summary>
        [XmlAttribute]
        public double maximum { get; set; }
        /// <summary>
        /// Number pattern
        /// </summary>
        [XmlAttribute]
        public String pattern { get; set; }
        /// <summary>
        /// Field Suffix
        /// </summary>
        [XmlAttribute]
        public String suffix { get; set; }
        /// <summary>
        /// Required Field
        /// </summary>
        [XmlAttribute]
        public int req { get; set; }
        /// <summary>
        /// XPRO Code
        /// </summary>
        [XmlAttribute]
        public String code { get; set; }
        /// <summary>
        /// translate
        /// </summary>
        [XmlAttribute]
        public int translate { get; set; }
    }
}
