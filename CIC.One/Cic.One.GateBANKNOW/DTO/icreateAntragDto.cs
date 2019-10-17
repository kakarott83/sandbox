using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateBANKNOW.DTO
{
    /// <summary>
    /// Input for Service-Method createAntrag
    /// </summary>
    public class icreateAntragDto  
    {
        /// <summary>
        /// complete set of offer data
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto  antrag { get; set; }
        /// <summary>
        /// External User Id
        /// </summary>
        public String extuserid { get; set; }
        /// <summary>
        /// External Dealer Id
        /// </summary>
        public String extdealerid { get; set; }

        /// <summary>
        /// Sprachcode
        /// </summary>
        public String ISOLanguageCode { get; set; }
    }
}