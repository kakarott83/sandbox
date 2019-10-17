using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{

    /// <summary>
    /// Output for Kontrollinhaber Auskunft Service
    /// </summary>
    public class ogetControlPersonBusinessDto 
    {
        /// <summary>
        /// State of the request
        /// </summary>
        public KontrollinhaberStatus status { get; set; }

        /// <summary>
        /// Feststellungspflicht für Kunde
        /// </summary>
        public int? feststellungsPflicht { get; set; }

        /// <summary>
        /// Trefferliste
        /// </summary>
        public List<AdresseDto> adresslist { get; set; }

        /// <summary>
        /// Kreditnehmereinheiten
        /// </summary>
        public List<KneDto> kne { get; set; }
        
    }
}