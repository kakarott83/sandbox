
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Leasing Mietvertrag Description Data Transfer Object
    /// </summary>
    public class ZekLeasingMietvertragDescriptionDto
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
        /// Getter/Setter Debitor Role
        /// </summary>
        public int? DebtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Credit contract ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract State
        /// </summary>
        public int? VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Number of monthly
        /// </summary>
        public int AnzahlMonatlicheRaten { get; set; }

        /// <summary>
        /// Getter/Setter Sicherstellungscode
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
        /// Erste Grosse Rate
        /// </summary>
        public int? ErsteGrosseRate { get; set; }

        /// <summary>
        /// Anzahlung
        /// </summary>
        public int? Anzahlung { get; set; }

        /// <summary>
        /// Grosse Schlussrate
        /// </summary>
        public int? GrosseSchlussrate { get; set; }

        /// <summary>
        /// Restwert
        /// </summary>
        public int? Restwert { get; set; }

        /// <summary>
        /// Kreditbetrag
        /// </summary>
        public float Kreditbetrag { get; set; }

        /// <summary>
        /// Monatsrate
        /// </summary>
        public float Monatsrate { get; set; }

        /// <summary>
        /// Datum der ersten Rate
        /// </summary>
        public string DatumErsteRate { get; set; }

        /// <summary>
        /// Datum der letzten Rate
        /// </summary>
        public string DatumLetzteRate { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
    }
}