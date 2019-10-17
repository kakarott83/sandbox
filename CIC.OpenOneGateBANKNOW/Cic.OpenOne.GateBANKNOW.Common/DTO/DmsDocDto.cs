using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class DmsDocDto
    {
        public long sysdmsdoc { get; set; }

        public byte[] inhalt { get; set; }
        
        public String name { get; set; }
        public String dateiname { get; set; }
        public String format { get; set; }
        public String typ { get; set; }
        public String bemerkung { get; set; }
        public String gedrucktvon { get; set; }
        public DateTime? gedrucktam { get; set; }
        public String ungueltigvon { get; set; }
        public DateTime? ungueltigam { get; set; }
        public int ungueltigflag { get; set; }
        public String ungueltigcomment { get; set; }
        public String searchterms { get; set; }
        public long syswftx { get; set; }

    }
}
