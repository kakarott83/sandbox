using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Offer Object Entity
    /// </summary>
    public class AngobDto : ObjektDto 
    {
      
        /// <summary>
        /// Extended Object Info
        /// </summary>
        public AngobIniDto zusatzdaten { get; set; }

        /// <summary>
        /// BSI Paket betrag
        /// </summary>
        public double mitfinb { get; set; }
        public double lpBrutto { get; set; }

        /// <summary>
        /// Brief-Daten
        /// </summary>
        public AngobbriefDto briefdaten { get; set; }
        /// <summary>
        /// Briefnummer
        /// </summary>
        public String brief { get; set; }
      
    }
}