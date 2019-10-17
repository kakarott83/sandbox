namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Fixed Credit Description Data Transfer Object
    /// </summary>
    public class ZekFestkreditDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Signature
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
        /// Getter/Setter Debtor Role
        /// </summary>
        public int? DebtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Credit Contract ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract State
        /// </summary>
        public int? VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Saveguarding Code
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
        /// Getter/Setter Credit Ammount
        /// </summary>
        public float Kreditbetrag { get; set; }

        /// <summary>
        /// Getter/Setter Date of contract beginning
        /// </summary>
        public string datumVertragsBeginn { get; set; }

        /// <summary>
        /// Getter/Setter Date of contract ending
        /// </summary>
        public string datumVertragsEnde { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
    }
}