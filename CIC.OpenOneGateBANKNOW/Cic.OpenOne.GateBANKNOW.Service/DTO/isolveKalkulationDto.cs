using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.solveKalkulation"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.solveKalkulation"/> Methode
    /// </summary>
    public class isolveKalkulationDto
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
