
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Überziehungskredit Description Data Transfer Object
    /// </summary>
    public class ZekUeberziehungskreditDescriptionDto
    {
        /// <summary>
        /// Getter/Setter signature
        /// </summary>
        public int? Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter  Subsidiary
        /// </summary>
        public int? Filiale { get; set; }

        /// <summary>
        /// Getter/Setter Origin
        /// </summary>
        public int? Herkunft { get; set; }

        /// <summary>
        /// Getter/Setter Debitor Role
        /// </summary>
        public int? DebtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Credit Contract ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract State
        /// </summary>
        public int VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter net total bank statement 
        /// </summary>
        public float SaldoKontoAuszug { get; set; }

        /// <summary>
        /// Getter/Setter Date Reference
        /// </summary>
        public string DatumReferenz { get; set; }

        /// <summary>
        /// Getter/Setter Date net total call date
        /// </summary>
        public string DatumSaldoStichTag { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
    }
}