using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Restwert DTO Eingang
    /// </summary>
    public class igetRestwertDto
    {
        /// <summary>
        /// Objekt-Typ
        /// </summary>
        public long sysobtyp { get; set; }

        /// <summary>
        /// Laufzeit
        /// </summary>
        public long Alter { get; set; }

        /// <summary>
        /// Laufleistung
        /// </summary>
        public long Laufleistung { get; set; }
    }
}