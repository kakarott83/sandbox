using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateWKT.DTO
{
    /// <summary>
    /// Input for calculating rate, nova, rw
    /// </summary>
    public class icalcRateDto
    {
        public long sysobtyp { get; set; }
        public long sysangob { get; set; }

        public double vsrate { get; set; }
        public double servicerate { get; set; }

        public double mwst { get; set; }
        public double bgnova { get; set; }
        public double bginternexklNova { get; set; }
        public double bgintern { get; set; }
        public double provision { get; set; }
        public double rw { get; set; }
        public double rwp { get; set; }
        public double zins { get; set; }
        public double zinsaufab { get; set; }
        public long syszinstab { get; set; }//when set, will fetch the zins, when zinsfield is zero
        public double depot { get; set; }
        public double depotZins { get; set; }
        public double sonder { get; set; }
        public double grund { get; set; }//fahrzeug grundpreis
        public double sarv { get; set; }//rw relevante sa
        public double novap { get; set; }//nova prozentsatz
        public double rwfzaufabp { get; set; }//rw auf/abschlag prozent
        public double kaskoRate { get; set; }
        public double hpRate { get; set; }
        public int syskalktyp { get; set; }
        public int lz { get; set; }
        public long ll { get; set; }
        public long kmstand { get; set; }
        public int rggtyp { get; set; }
        //parameters needed for nova bonus malus
        public int krafststoff { get; set; }
        public double co2 { get; set; }
        public double nox { get; set; }
        public double particleCount { get; set; }
        public DateTime perDate { get; set; }
        public DateTime erstZul { get; set; }
        public int actuation { get; set; }
    }
}