
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Card Management Description Data Transfer Object
    /// </summary>
    public class ZekKartenengagementDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Signature
        /// </summary>
        public int? Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter subsidiary
        /// </summary>
        public int? Filiale { get; set; }

        /// <summary>
        /// Getter/Setter Origin
        /// </summary>
        public int? Herkunft { get; set; }

        /// <summary>
        /// Getter/Setter Debtor role
        /// </summary>
        public int? DebtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Credit contract ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract State
        /// </summary>
        public int VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Balance account day
        /// </summary>
        public float SaldoAbrechnungsTag { get; set; }

        /// <summary>
        /// Getter/Setter Date of Balance deadline
        /// </summary>
        public string DatumSaldoStichTag { get; set; }

        /// <summary>
        /// Getter/Setter Date of Account Opening
        /// </summary>
        public string DatumKontoEroeffnung { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
    }
}