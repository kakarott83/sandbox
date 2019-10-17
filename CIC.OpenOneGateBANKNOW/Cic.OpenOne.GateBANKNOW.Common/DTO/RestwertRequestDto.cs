using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Restwertrequest Dto
    /// </summary>
    public class RestwertRequestDto
    {
        /// <summary>
        /// Objekt-Typ
        /// </summary>
        public long sysobtyp {get; set;}

        /// <summary>
        /// Laufzeit
        /// </summary>
        public string Laufzeit { get; set; }

        /// <summary>
        /// Laufleistung
        /// </summary>
        public string Laufleistung { get; set; }

        /// <summary>
        /// Abfragedatum und Uhrzeit
        /// </summary>
        public DateTime perDate { get; set; }

    }
}
