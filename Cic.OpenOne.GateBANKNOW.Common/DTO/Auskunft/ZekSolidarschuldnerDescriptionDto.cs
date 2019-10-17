
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Solidarschuldner Description Data Transfer Object
    /// </summary>
    public class ZekSolidarschuldnerDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Kennzeichen
        /// </summary>
        public int Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter Filiale
        /// </summary>
        public int Filiale { get; set; }

        /// <summary>
        /// Getter/Setter herkunft
        /// </summary>
        public int Herkunft { get; set; }

        /// <summary>
        /// Getter/Setter Debitor Role
        /// </summary>
        public int? DebtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Kreditvertrag ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Vertragsstatus
        /// </summary>
        public int VertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter Anzahl monatliche Raten
        /// </summary>
        public int AnzahlMonatlicheRaten { get; set; }

        /// <summary>
        /// Getter/Setter Sicherstellungscode
        /// </summary>
        public int Sicherstellungscode { get; set; }

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
        /// Getter/Setter Kreditbetrag
        /// </summary>
        public float KreditBetrag { get; set; }

        /// <summary>
        /// Getter/Setter Monatsrate
        /// </summary>
        public float Monatsrate { get; set; }

        /// <summary>
        /// Getter/Setter Date First Rate
        /// </summary>
        public string DatumErsteRate { get; set; }

        /// <summary>
        /// Getter/Setter Date Last Rate
        /// </summary>
        public string DatumLetzteRate { get; set; }
    }
}
