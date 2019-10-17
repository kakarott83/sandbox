using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class AuftragDto : EntityDto
    {
        public long sysauftrag { get; set; }
        public int art { get; set; }
        public String rechnung { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public String kontonr { get; set; }
        public String blz { get; set; }
        public String bic { get; set; }
        public String iban { get; set; }
        public double gbetrag { get; set; }
        public DateTime? valutadatum { get; set; }
        public DateTime? belegdatum { get; set; }

        override public long getEntityId()
        {
            return sysauftrag;
        }
    }
}
