namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Request Entity Data Transfer Object
    /// </summary>
    public class ZekRequestEntityDto
    {
        /// <summary>
        /// Getter/Setter Reference number
        /// </summary>
        public int RefNo { get; set; }

        /// <summary>
        /// Getter/Setter Force new Address
        /// </summary>
        public int ForceNewAddress { get; set; }

        /// <summary>
        /// Getter/Setter Previous return code
        /// </summary>
        public int? PreviousReturnCode { get; set; }

        /// <summary>
        /// Getter/Setter Debtor Code
        /// </summary>
        public int? DebtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Address Description
        /// </summary>
        public ZekAddressDescriptionDto AddressDescription { get; set; } 
    }
}
