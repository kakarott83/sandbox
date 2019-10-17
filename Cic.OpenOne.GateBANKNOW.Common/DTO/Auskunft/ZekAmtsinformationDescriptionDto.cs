using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Amtsinformation Description Data Transfer Object
    /// </summary>
    public class ZekAmtsinformationDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Amts Code
        /// </summary>
        public int AmtsCode { get; set; }

        /// <summary>
        /// Getter/Setter Date Source
        /// </summary>
        public string DatumQuelle { get; set; }

        /// <summary>
        /// Getter/Setter Date Petition term
        /// </summary>
        public string DatumEingabefrist { get; set; }

        /// <summary>
        /// Getter/Setter Source Kanton
        /// </summary>
        public string QuellenKanton { get; set; }

        /// <summary>
        /// Getter/Setter PLZ Office
        /// </summary>
        public string PlzAmt { get; set; }
    }
}
