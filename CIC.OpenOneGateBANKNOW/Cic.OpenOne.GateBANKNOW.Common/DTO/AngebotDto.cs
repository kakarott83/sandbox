using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Parameterklasse für Angebot
    /// </summary>
    public class AngebotDto : AngAntDto
    {
        /// <summary>
        /// Primary Angebot Key
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Angebotsnummer
        /// </summary>
        public String angebot { get; set; }

        /// <summary>
        /// Kunde
        /// </summary>
        public KundeDto kunde { get; set; }

        /// <summary>
        /// Mitantragssteller
        /// </summary>
        public KundeDto mitantragsteller { get; set; }
        
        /// <summary>
        /// Angebotsvarianten (inkl. Kalkulationsdaten)
        /// </summary>
        public List<AngAntVarDto> angAntVars { get; set; }

        /// <summary>
        /// Drucksperre
        /// </summary>
        public int Drucksperre { get; set; }

        /// <summary>
        /// Liste mit Fehlermeldungen
        /// </summary>    
        public List<string> errortext { get; set; }


    }
}
