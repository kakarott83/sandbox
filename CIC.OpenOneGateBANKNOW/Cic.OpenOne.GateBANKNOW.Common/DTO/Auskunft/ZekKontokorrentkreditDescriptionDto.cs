
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Account Current Credit Description Data Transfer Object
    /// Die Feldtypen entsprechen den Typen aus ZekRef
    /// </summary>
    public class ZekKontokorrentkreditDescriptionDto
    {
        /// <summary>
        /// Getter/Setter signature
        /// </summary>
        public int? kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter Subsidiary
        /// </summary>
        public int? filiale { get; set; }

        /// <summary>
        /// Getter/Setter Origin
        /// </summary>
        public int? herkunft { get; set; }

        /// <summary>
        /// Getter/Setter Debtor role
        /// </summary>
        public int? debtorRole { get; set; }

        /// <summary>
        /// Getter/Setter Credit Contract ID
        /// </summary>
        public string kreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Contract state
        /// </summary>
        public int vertragsStatus { get; set; }

        /// <summary>
        /// Getter/Setter saveguarding code
        /// </summary>
        public int? sicherstellungscode { get; set; }

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
        /// Getter/Setter Credit limit
        /// </summary>
        public float kreditLimite { get; set; }

        /// <summary>
        /// Getter/Setter Contract begin
        /// </summary>
        public string datumVertragsBeginn { get; set; }

        /// <summary>
        /// Getter/Setter Contract End
        /// </summary>
        public string datumVertragsEnde { get; set; }

        /// <summary>
        /// Getter/Setter Used Credit in percent
        /// In der DB ist das ein Decimal, aber in der ZekRef ein Int32
        /// </summary>
        public int? beanspruchterKreditProzent { get; set; }

        /// <summary>
        /// Getter/Setter Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
    }
}