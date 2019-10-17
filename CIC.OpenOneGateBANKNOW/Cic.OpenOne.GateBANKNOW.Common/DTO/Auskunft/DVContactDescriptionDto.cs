using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// DV Contact Description Data Transfer Object
    /// </summary>
    public class DVContactDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Contact type
        /// </summary>
        public int ContactType { get; set; }

        /// <summary>
        /// Getter/Setter Contact Details
        /// </summary>
        public string ContactDetails { get; set; }

        /// <summary>
        /// Getter/Setter Date of last verification
        /// </summary>
        public string DateLastVerified { get; set; }
    }
}
