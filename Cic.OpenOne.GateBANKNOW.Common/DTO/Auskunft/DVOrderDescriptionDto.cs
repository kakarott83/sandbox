using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// DV Order Description Data Transfer Objects
    /// </summary>
    public class DVOrderDescriptionDto
    {
        /// <summary>
        /// Getter/Setter BA Product ID
        /// </summary>
        public int BAProductId { get; set; }

        /// <summary>
        /// Getter/Setter EWK Product ID
        /// </summary>
        public int EWKProductId { get; set; }

        /// <summary>
        /// Getter/Setter cresura Report ID
        /// </summary>
        public int cresuraReportId { get; set; }
    }
}
