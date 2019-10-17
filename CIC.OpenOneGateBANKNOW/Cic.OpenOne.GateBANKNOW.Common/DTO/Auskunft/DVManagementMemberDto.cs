using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// DV Management Member Data Transfer Object
    /// </summary>
    public class DVManagementMemberDto
    {
        /// <summary>
        /// Getter/Setter Address
        /// </summary>
        public DVAddressDescriptionDto Address { get; set; }

        /// <summary>
        /// Getter/Setter Function Name
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Getter/Setter Start Date
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Getter/Setter End Date
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Getter/Setter Signature Right
        /// </summary>
        public int SignatureRight { get; set; }

        /// <summary>
        /// Getter/Setter Heimatort
        /// </summary>
        public string Hometown { get; set; }
    }
}
