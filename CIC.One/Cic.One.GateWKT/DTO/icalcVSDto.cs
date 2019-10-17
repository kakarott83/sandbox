using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for calculating insurances
    /// </summary>
    public class icalcVSDto
    {
        /// <summary>
        /// Vehicle code
        /// </summary>
        public long sysobtyp { get; set; }
        /// <summary>
        /// kw verbrennung (bzw. Primärantriebs-Kw)
        /// </summary>
        public long kw { get; set; }

        /// <summary>
        /// Hubraum
        /// </summary>
        public long ccm { get; set; }

        /// <summary>
        /// kw emotor
        /// </summary>
        public long kwe { get; set; }

        /// <summary>
        /// Antriebsart
        /// </summary>
        public long? actuation { get; set; }

        /// <summary>
        /// Berechnungsdatum
        /// </summary>
        public DateTime? lieferdatum { get; set; }
    }
}