using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Angebot Varianten Dto
    /// </summary>
    public class AngAntVarDto
    {
        /// <summary>
        /// PKEY
        /// </summary>
        public long sysangvar { get; set; }

        /// <summary>
        /// Verweis zum Angebot 
        /// </summary>
        public long sysangebot { get; set; }

        /// <summary>
        /// Rang für Zusatz in Angebotsnummer zb 4711/1 
        /// </summary>
        public short rang { get; set; }
        /// <summary>
        /// Freitexteingabe 
        /// </summary>
        public String bezeichnung { get; set; }
        /// <summary>
        /// Gültigkeit Variante (aus Produkt bzw Aktion) 
        /// </summary>
        public DateTime? gueltigBis { get; set; }

        /// <summary>
        /// Definiert ob Variante in Antrag übernommen werden sol
        /// </summary>
        public int inantrag { get; set; }

        /// <summary>
        /// Kalkulation Data Transfer Object
        /// </summary>
        public KalkulationDto kalkulation;

        /// <summary>
        /// Product Code für das jeweilige PrProdukt
        /// </summary>
        public String PrProductCode { get; set; }
        
    }
}
