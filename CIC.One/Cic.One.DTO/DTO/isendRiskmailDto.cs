using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class isendRiskmailDto
    {
         
        public long sysid { get; set; }

        public long ISOLanguageCode { get; set; }

        public String betreff { get; set; }
        public String empfaenger { get; set; }
        public String absender { get; set; }
        public String name { get; set; }
        public String vorname { get; set; }
        public String departament { get; set; }
        public String risikobereich { get; set; }
        public String antrag { get; set; }
        public String darlehnsbetrag { get; set; }
        public String vertriebsweg { get; set; }
        public String inhalt { get; set; }

        public bool Send { get; set; }

        [NonSerialized]
        public Message error;

    }
}
