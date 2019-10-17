using Cic.OpenOne.Common.DTO;
using System;

namespace Cic.One.DTO
{
    public class ZekContractDto : EntityDto
    {

        /// <summary>
        /// Number Monthly Rates
        /// </summary>
        public int anzahlMonatlicheRaten { get; set; }
        /// <summary>
        /// credit rating code IKO
        /// </summary>
        public int? bonitaetscodeIKO { get; set; }
        /// <summary>
        /// credit rating code
        /// </summary>
        public int? bonitaetscodeZEK { get; set; }
        /// <summary>
        /// Date of Credit Rating IKO
        /// </summary>
        public DateTime? datumBonitaetIKO { get; set; }
        /// <summary>
        /// Date of Credit Rating ZEK
        /// </summary>
        public DateTime? datumBonitaetZEK { get; set; }
        /// <summary>
        /// Date of First Rate
        /// </summary>
        public DateTime? datumErsteRate { get; set; }
        /// <summary>
        /// Date of last Rate
        /// </summary>
        public DateTime? datumLetzteRate { get; set; }
        /// <summary>
        /// Date of contract beginning
        /// </summary>
        public DateTime? datumVertragsBeginn { get; set; }
        /// <summary>
        /// Date of contract ending
        /// </summary>
        public DateTime? datumVertragsEnde { get; set; }
        /// <summary>
        /// Debtor Role
        /// </summary>
        public int? debtorRole { get; set; }
        /// <summary>
        /// subsidiary
        /// </summary>
        public int? filiale { get; set; }
        /// <summary>
        /// Origin
        /// </summary>
        public int? herkunft { get; set; }
        /// <summary>
        /// Credit ammount
        /// </summary>
        public float kreditbetrag { get; set; }
        /// <summary>
        /// Credit Contract ID
        /// </summary>
        public string kreditVertragID { get; set; }
        /// <summary>
        /// Montly Rate
        /// </summary>
        public float monatsrate { get; set; }
        /// <summary>
        /// Saveguarding Code
        /// </summary>
        public int? sicherstellungsCode { get; set; }
        /// <summary>
        /// Theoretischer RestSaldo
        /// </summary>
        public float theoRestSaldo { get; set; }
        /// <summary>
        /// Contract State
        /// </summary>
        public int? vertragsStatus { get; set; }

        /// <summary>
        /// Gets identification
        /// </summary>
        /// <returns>primary key</returns>
        public override long getEntityId()
        {
            try
            {
                return Convert.ToInt64(kreditVertragID);
            }
            catch
            {
                return 0;
            }
        }
    }
}
