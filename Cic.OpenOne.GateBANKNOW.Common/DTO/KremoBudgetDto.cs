using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class KremoBudgetDto
    {

        /// <summary>
        /// PKZ-Daten
        /// </summary>
        public PkzDto pkz { get; set; }

        /// <summary>
        /// PLZ
        /// </summary>
        public long plz { get; set; }

        /// <summary>
        /// Kanton
        /// </summary>
        public double Kantoncode { get; set; }

        /// <summary>
        /// Geburtsdatum
        /// </summary>
        public DateTime? GebDatum { get; set; }

        public double getKantoncodeInternal()
        {
            if (Kantoncode == 1) return 0;
            if (Kantoncode == 2) return 2;
            if (Kantoncode == 3) return 1;
            if (Kantoncode == 4) return 5;
            if (Kantoncode == 5) return 4;
            if (Kantoncode == 6) return 3;
            if (Kantoncode == 7) return 6;
            if (Kantoncode == 8) return 7;
            if (Kantoncode == 9) return 8;
            if (Kantoncode == 10) return 9;
            if (Kantoncode == 11) return 10;
            if (Kantoncode == 12) return 11;
            if (Kantoncode == 13) return 12;
            if (Kantoncode == 14) return 13;
            if (Kantoncode == 15) return 14;
            if (Kantoncode == 16) return 18;
            if (Kantoncode == 17) return 15;
            if (Kantoncode == 18) return 17;
            if (Kantoncode == 19) return 16;
            if (Kantoncode == 20) return 19;
            if (Kantoncode == 21) return 20;
            if (Kantoncode == 22) return 21;
            if (Kantoncode == 23) return 23;
            if (Kantoncode == 24) return 22;
            if (Kantoncode == 25) return 24;
            if (Kantoncode == 26) return 25;
            return 26;


        }
    }
}