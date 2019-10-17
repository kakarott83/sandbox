using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    
    /// <summary>
    /// Object Equipment Item
    /// </summary>
    public class AngAntObAustDto
    {
        public long sysobaust { get; set; }
        public long sysob { get; set; }
        public long sysantob { get; set; }
        public long sysangob { get; set; }
        public String snr { get; set; }
        public String beschreibung { get; set; }
        public double betrag { get; set; }
        public long sysmycalc { get; set; }
        public int flagrwrel { get; set; }
        public int flagpacket { get; set; }
        public String freitext { get; set; }
        public String freitext2 { get; set; }
        public String herstellerdaten { get; set; }
        public double betrag2 { get; set; }
    }
}
