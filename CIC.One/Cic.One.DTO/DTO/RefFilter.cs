using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    public class RefFilter
    {
        /// <summary>
        /// SQL Filter field
        /// </summary>
        [XmlAttribute]
        public String field { get; set; }

        /// <summary>
        /// Filter Source WFVREF id of same dashboard
        /// </summary>
        [XmlAttribute]
        public String wfvref { get; set; }

        /// <summary>
        /// Dto-Field of WFVREF Entity
        /// </summary>
        [XmlAttribute]
        public String reffield { get; set; }
    }
}
