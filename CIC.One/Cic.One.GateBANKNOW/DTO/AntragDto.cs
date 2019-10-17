using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.GateBANKNOW.DTO
{
    public class AntragDto
    {
        /// <summary>
        /// Kalkulationsdaten
        /// </summary>
        public KalkulationDto kalkulation { get; set; }
        /// <summary>
        /// Kundendaten
        /// </summary>
        public KundeDto kunde { get; set; }
        /// <summary>
        /// Mitantragstellerdaten
        /// </summary>
        public KundeDto mitantragsteller { get; set; }
        /// <summary>
        /// Objektdaten
        /// </summary>
        public ObjektDto objekt { get; set; }
        /// <summary>
        /// Opportunityid
        /// </summary>
        public String extreferenz { get; set; }
        /// <summary>
        /// Verweis zum Produkt
        /// </summary>
        public long? sysprproduct { get; set; }
    }
}
