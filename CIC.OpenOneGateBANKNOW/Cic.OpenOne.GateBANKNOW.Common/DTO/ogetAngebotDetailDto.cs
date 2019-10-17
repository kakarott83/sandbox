using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getAngebotDetail Methode
    /// </summary>
    public class ogetAngebotDetailDto
    {
        /// <summary>
        /// Persistenzobjekt Kunde zu Angebot
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }

        /// <summary>
        /// Persistenzobjekt Objekt zu Angebot
        /// </summary>
        public DTO.ObjektDto objekt
        {
            get;
            set;
        }

        /// <summary>
        /// Liste Persistenzobjekte Kalkulation zu Angebot
        /// </summary>
        public DTO.KalkulationDto[] kalkulationen
        {
            get;
            set;
        }
    }
}
