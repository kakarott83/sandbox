using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{


    /// <summary>
    /// Buchwerte Input class
    /// </summary>
    public class BWInDto
    {
        public double? mwst { get; set; }
        public double barkaufpreis { get; set; }
        public double zins { get; set; }
        public double restwert { get; set; }
        public int laufzeit { get; set; }
        public double? rate { get; set; }
        public long sysvart { get; set; }
        public double anzahlung { get; set; }
    }

    public class BWOutDto
    {
        public double buchwerte { get; set; }
        public double[] bwarray { get; set; }
        public double[] tilgung { get; set; }
        public string BuchwerteListe { get; set; }
        public string TilgungListe { get; set; }

    }
    public class ELOutDto
    {
        public double LGDp { get; set; }
        public double LGD { get; set; }
        public double ausfallwahrscheinlichkeitP { get; set; }
        public double profitabilitaetp { get; set; }
        public double TR { get; set; }
        public double TRP { get; set; }


        public double SUMME_MW { get; set; }
        public double SUMME_BW { get; set; }
        public double LZ_FAKTOR { get; set; }
        public double BKP_FAKTOR { get; set; }
        public double ALTER_FAKTOR { get; set; }

        public BWInDto INTPUTBUCHWERTE { get; set; }
        public BWOutDto OUTPUTBUCHWERTE { get; set; }


    }
}
