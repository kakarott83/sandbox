using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Parameterklasse für Angebot
    /// </summary>
    public class BNAngebotDto : Cic.One.DTO.AngAntDto
    {
        public override long getEntityId()
        {
            return sysid;
        }
        public override string getEntityBezeichnung()
        {
            return angebot;
        }
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
        public BNKundeDto kunde { get; set; }

        /// <summary>
        /// Mitantragssteller
        /// </summary>
        public BNKundeDto mitantragsteller { get; set; }

        /// <summary>
        /// Händler
        /// </summary>
        public BNKundeDto haendler { get; set; }

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

        /// <summary>
        /// Produktinformationen aus der Kalkulation
        /// </summary>
        public ProduktInfoDto produkt { get; set; }
        
        /// <summary>
        /// Mitantragsteller sysperson
        /// </summary>
        public long sysMa { get; set; }
        /// <summary>
        /// Mitantragsteller sysit
        /// </summary>
        public long sysItMa { get; set; }




    }
}
