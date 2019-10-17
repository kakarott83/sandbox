using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// DV Address Correction Data Transfer Object
    /// </summary>
    public class DVAddressCorrectionDto
    {
        /// <summary>
        /// Getter/Setter City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Getter/Setter Corrected City
        /// </summary>
        public int CorrCity { get; set; }

        /// <summary>
        /// Getter/Setter Corrected House Number
        /// </summary>
        public int CorrHousenumber { get; set; }

        /// <summary>
        /// Getter/Setter Corrected Street
        /// </summary>
        public int CorrStreet { get; set; }

        /// <summary>
        /// Getter/Setter Corrected ZIP
        /// </summary>
        public int CorrZip { get; set; }

        /// <summary>
        /// Getter/Setter House number
        /// </summary>
        public string Housenumber { get; set; }

        /// <summary>
        /// Getter/Setter Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Getter/Setter ZIP
        /// </summary>
        public string Zip { get; set; }

    }
}
