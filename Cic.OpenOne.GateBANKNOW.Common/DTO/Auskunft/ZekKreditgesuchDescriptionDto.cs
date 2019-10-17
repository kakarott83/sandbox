
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Credit Application Description Data Transfer Object
    /// </summary>
    public class ZekKreditgesuchDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Signature
        /// </summary>
        public int Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter subsidiary
        /// </summary>
        public int Filiale { get; set; }

        /// <summary>
        /// Getter/Setter Origin
        /// </summary>
        public int Herkunft { get; set; }

        /// <summary>
        /// Getter/Setter Credit contract ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract State
        /// </summary>
        public int VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Reason of Rejection
        /// </summary>
        public int Ablehnungsgrund { get; set; }

        /// <summary>
        /// Getter/Setter Date of Credit Application
        /// </summary>
        public string DatumKreditgesuch { get; set; }

        /// <summary>
        /// Getter/Setter Date Valid till
        /// </summary>
        public string DatumGueltigBis { get; set; }

        /// <summary>
        /// Getter/Setter Date of rejection
        /// </summary>
        public string DatumAblehnung { get; set; }

        /// <summary>
        /// Getter/Setter DebtOrRole
        /// </summary>
        public int? DebtOrRole { get; set; }
    }
}