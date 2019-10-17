using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// DV ADress Match Data Transfer Object
    /// </summary>
    public class DVAddressMatchDto
    {
        /// <summary>
        /// Getter/Setter Address
        /// </summary>
        public DVAddressDescriptionDto Address { get; set; }

        /// <summary>
        /// Getter/Setter Address ID
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Getter/Setter Character
        /// </summary>
        public int Character { get; set; }

        /// <summary>
        /// Getter/Setter Confidence
        /// </summary>
        public int Confidence { get; set; }

        /// <summary>
        /// Getter/Setter Correction
        /// </summary>
        public DVAddressCorrectionDto Correction { get; set; }

        /// <summary>
        /// Getter/Setter Difference
        /// </summary>
        public int Difference { get; set; }

        /// <summary>
        /// Getter/Setter Similarity
        /// </summary>
        public int Similarity { get; set; }

        /// <summary>
        /// Getter/Setter Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Getter/Setter Key Value List
        /// </summary>
        public List<DVKeyValuePairDto> KeyValueList { get; set; }
    }
}
