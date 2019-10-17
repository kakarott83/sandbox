using Cic.OpenOne.Common.DTO.Prisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    public class isolveBNKalkulationDto
    {
        /// <summary>
        /// Persistenzobjekt Kalkulation
        /// </summary>
       public KalkulationDto kalkulation
        {
            get;
            set;
        }

        /// <summary>
        /// Produktkontext zur Zinsermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }

        /// <summary>
        /// Kalkulationskontext (für Berechnung notwendige Daten)
        /// </summary>
        public kalkKontext kalkKontext { get; set; }


    }
}
