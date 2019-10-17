using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Rahmenvertragsposition
    /// </summary>
    public class RvtPosDto : EntityDto
    {
        public long sysrvtpos { get; set; }
        public long sysrvt { get; set; }
        public long sysvttyp { get; set; }
        public String gruppe { get; set; }
        public String bezeichnung { get; set; }
        public DateTime? gueltigvon { get; set; }
        public DateTime? gueltigbis { get; set; }
        public double wert { get; set; }
        public double wertp { get; set; }
        public DateTime? wertdatum { get; set; }
        public long wertnum { get; set; }
        public int ppy { get; set; }
        public DateTime? faellig { get; set; }
        public long sysabrregel { get; set; }
        public String code { get; set; }
        public double wert01 { get; set; }
        public double wertp01 { get; set; }
        public DateTime? wertdatum01 { get; set; }
        public long wertnum01 { get; set; }
        public double wert02 { get; set; }
        public double wertp02 { get; set; }
        public DateTime? wertdatum02 { get; set; }
        public long wertnum02 { get; set; }
        public double wert03 { get; set; }
        public double wertp03 { get; set; }
        public DateTime? wertdatum03 { get; set; }
        public long wertnum03 { get; set; }
        public double wert04 { get; set; }
        public double wertp04 { get; set; }
        public DateTime? wertdatum04 { get; set; }
        public long wertnum04 { get; set; }
        public double wert05 { get; set; }
        public double wertp05 { get; set; }
        public DateTime? wertdatum05 { get; set; }
        public long wertnum05 { get; set; }
        public long sysfstyp { get; set; }
        public long syszinstab { get; set; }
        public int flag { get; set; }

        public long sysvstyp { get; set; }//temp field for sysvstyp of rvtvs

        public override long getEntityId()
        {
            return sysrvtpos;
        }
    }
}
