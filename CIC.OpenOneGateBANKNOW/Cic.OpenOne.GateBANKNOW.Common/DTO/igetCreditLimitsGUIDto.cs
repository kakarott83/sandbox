using Cic.OpenOne.Common.DTO.Prisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class igetCreditLimitsGUIDto 
    {

        public Prprodtype prprodtype { get; set; }

        public long sysperole { get; set; }

        public long sysprbildwelt { get; set; }
     
        public long sysprchannel { get; set; }

        public long syskdtyp { get; set; }
    
        public long sysprusetype { get; set; }

        public DateTime perDate { get; set; }

        public long sysprkgroup { get; set; }

        public long sysprhgroup { get; set; }

        public long sysvttyp { get; set; }

        public string isoCode { get; set; }
        public long sysantrag
       {
            get;
            set;
        }
        public long sysvart
        {
            get;
            set;
        }
        public long syswfuser { get; set; }

    }
}
