using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Batch Close Contracts Data Transfer Object
    /// Massen-Vertragsabmeldung (EC5)
    /// Bei einer Vertragsauflösung muss eine Meldung an ZEK versendet werden.
    /// ZEKBatchServiceService.closeContractsBatch
    /// </summary>
    public class ZekBatchContractClosureInstructionDto
    {
        /// <summary>
        /// Getter/Setter batchRequestId
        /// Der Wert muss eindeutig pro Massenmeldung sein.
        /// </summary>
        public String batchRequestId { get; set; }

        /// <summary>
        /// Getter/Setter Customer Reference number
        /// Da wird die sysZek gespeichert
        /// </summary>
        public String customerReference { get; set; }

        /// <summary>
        /// Bonitätscode (Code)
        /// </summary>
        public System.Nullable<int> bonitaetsCodeZEK { get; set; }

        /// <summary>
        /// Bonitätscode (Code) IKO
        /// </summary>
        public System.Nullable<int> bonitaetsCodeIKO { get; set; }

        /// <summary>
        /// eCode178
        /// </summary>
        public string eCode178id { get; set; }

        /// <summary>
        /// Vertragsart/Kreditart (Code)
        /// </summary>
        public System.Nullable<int> contractType { get; set; }

        /// <summary>
        /// Datum der Bonitätscode-Festlegung
        /// </summary>
        public string datumBonitaetZEK { get; set; }

        /// <summary>
        /// Getter/Setter Date of Credit Rating IKO
        /// </summary>
        public string datumBonitaetIKO { get; set; }

        /// <summary>
        /// Dienst-Identitätswert Vertrag
        /// </summary>
        public string kreditVertragID { get; set; }

        /// <summary>
        /// Inaktiv-Flag
        /// </summary>
        public bool inaktiv { get; set; }
    }
}