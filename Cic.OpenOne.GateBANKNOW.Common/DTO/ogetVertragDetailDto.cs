using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getVertragDetail Methode
    /// </summary>
    public class ogetVertragDetailDto
    {
        /// <summary>
        /// Persistenzobjekt Kunde zu Vertrag
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }

        /// <summary>
        /// Persistenzobjekt Objekt zu Vertrag
        /// </summary>
        public DTO.ObjektDto objekt
        {
            get;
            set;
        }

        /// <summary>
        /// Persistenzobjekt Kalkulation zu Vertrag
        /// </summary>
        public DTO.KalkulationDto kalkulation
        {
            get;
            set;
        }
    }
}
