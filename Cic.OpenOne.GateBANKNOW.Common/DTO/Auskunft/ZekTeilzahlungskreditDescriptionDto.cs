
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Teilzahlungskredit Description Data Transfer Object
    /// </summary>
    public class ZekTeilzahlungskreditDescriptionDto
    {
        /// <summary>
        /// Getter/Setter signature
        /// </summary>
        public int? Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter Subsidiary
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
        /// Getter/Setter Credit Contract ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract state
        /// </summary>
        public int? VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Number monthly rates
        /// </summary>
        public int AnzahlMonatlicheRaten { get; set; }

        /// <summary>
        /// Getter/Setter securing code
        /// </summary>
        public int? Sicherstellungscode { get; set; }

        /// <summary>
        /// Getter/Setter credit rating code
        /// </summary>
        public int? BonitaetscodeZEK { get; set; }
        /// <summary>
        /// Getter/Setter credit rating code IKO
        /// </summary>
        public int? BonitaetscodeIKO { get; set; }

        /// <summary>
        /// Getter/Setter Date of Credit Rating ZEK
        /// </summary>
        public string DatumBonitaetZEK { get; set; }
        /// <summary>
        /// Getter/Setter Date of Credit Rating IKO
        /// </summary>
        public string DatumBonitaetIKO { get; set; }

        /// <summary>
        /// Getter/Setter credit ammount
        /// </summary>
        public float Kreditbetrag { get; set; }

        /// <summary>
        /// Getter/Setter  rate
        /// </summary>
        public float Monatsrate { get; set; }

        /// <summary>
        /// Getter/Setter Date first rate
        /// </summary>
        public string DatumErsteRate { get; set; }

        /// <summary>
        /// Getter/Setter Date last rate
        /// </summary>
        public string DatumLetzteRate { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }

        /// <summary>
        /// Getter/Setter First rate
        /// </summary>
        public int? AnfangsRate { get; set; }

        /// <summary>
        /// Getter/Setter Restwert
        /// </summary>
        public int? Restwert { get; set; }
    }
}