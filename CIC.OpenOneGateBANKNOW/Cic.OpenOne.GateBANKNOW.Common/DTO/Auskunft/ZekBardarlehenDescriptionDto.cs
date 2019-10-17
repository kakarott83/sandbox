using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Cash Loan Description Data Transfer Object
    /// </summary>
    public class ZekBardarlehenDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Signature
        /// </summary>
        public int? kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter subsidiary
        /// </summary>
        public int? filiale { get; set; }

        /// <summary>
        /// Getter/Setter Origin
        /// </summary>
        public int? herkunft { get; set; }

        /// <summary>
        /// Getter/Setter Debtor Role
        /// </summary>
        public int? debtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Credit Contract ID
        /// </summary>
        public string kreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract State
        /// </summary>
        public int? vertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Number Monthly Rates
        /// </summary>
        public int anzahlMonatlicheRaten { get; set; }

        /// <summary>
        /// Getter/Setter Saveguarding Code
        /// </summary>
        public int? sicherstellungsCode { get; set; }

        /// <summary>
        /// Getter/Setter credit rating code
        /// </summary>
        public int? bonitaetscodeZEK { get; set; }
        /// <summary>
        /// Getter/Setter credit rating code IKO
        /// </summary>
        public int? bonitaetscodeIKO { get; set; }

        /// <summary>
        /// Getter/Setter Date of Credit Rating ZEK
        /// </summary>
        public string datumBonitaetZEK { get; set; }
        /// <summary>
        /// Getter/Setter Date of Credit Rating IKO
        /// </summary>
        public string datumBonitaetIKO { get; set; }

        /// <summary>
        /// Getter/Setter Credit ammount
        /// </summary>
        public float kreditbetrag { get; set; }

        /// <summary>
        /// Getter/Setter Montly Rate
        /// </summary>
        [AttributeRequired(true)]
        public float monatsrate { get; set; }

        /// <summary>
        /// Getter/Setter Date of First Rate
        /// </summary>
        [AttributeRequired(true)]
        public string datumErsteRate { get; set; }

        /// <summary>
        /// Getter/Setter Date of last Rate
        /// </summary>
        [AttributeRequired(true)]
        public string datumLetzteRate { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
    }
}