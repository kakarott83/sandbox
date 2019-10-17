using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Objekttyp Restwert DTO
    /// </summary>
    public class ObtypDataRestwertDto
    {
        /// <summary>
        /// Objekt-Typ
        /// </summary>
        public long Schwacke { get; set; }

        /// <summary>
        /// Objekt-Typ
        /// </summary>
        public long sysobtyp { get; set; }

        /// <summary>
        /// Wertegruppe
        /// </summary>
        public double TotalListPriceOfEquipment { get; set; }

    }
}
