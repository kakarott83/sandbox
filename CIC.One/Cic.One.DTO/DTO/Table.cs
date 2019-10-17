using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    public class Table
    {
        [XmlAttribute]
        public String name { get; set; }
        [XmlAttribute]
        public String pkey { get; set; }
        public List<Fkey> fkeys { get; set; } 
    }
}
