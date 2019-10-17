using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    public class Fkey
    {
        [XmlAttribute]
        public String column { get;set;}
        [XmlAttribute]
        public String foreign { get; set; }
    }
}
