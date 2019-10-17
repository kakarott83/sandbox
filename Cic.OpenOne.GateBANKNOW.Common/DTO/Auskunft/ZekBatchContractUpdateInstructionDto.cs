using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Batch Contract Update Data Transfer Object
    /// Mutation Vertragsdaten (EC7) 
    /// Änderungen der Vertragsdaten (Änderung Bonitätscode, Vertragsverlängerung etc.) 
    /// ZEKBatchServiceService.updateContractsBatch
    /// </summary>
    public class ZekBatchContractUpdateInstructionDto
    {
        /// <summary>
        /// Getter/Setter batchRequestId
        /// Der Wert muss eindeutig pro Massenmeldung sein.
        /// </summary>
        public String batchRequestId { get; set; }

        /// <summary>
        /// Getter/Setter Customer Reference number
        /// Der Referenzwert muss eindeutig pro Massenmeldung sein.
        /// </summary>
        public String customerReference { get; set; }

        /// <summary>
        /// eCode178
        /// </summary>
        public string eCode178id { get; set; }

        /// <summary>
        /// Inaktiv-Flag
        /// </summary>
        public bool inaktiv { get; set; }

        /// <summary>
        /// ZEK Cash Loan Description
        /// </summary>
        public ZekBardarlehenDescriptionDto bardarlehen { get; set; }

        /// <summary>
        /// ZEK Fixed Credit Description
        /// </summary>
        public ZekFestkreditDescriptionDto festkredit { get; set; }

        /// <summary>
        /// ZEK Account Current Credit Description
        /// </summary>
        public ZekKontokorrentkreditDescriptionDto kontokorrentkredit { get; set; }

        /// <summary>
        /// ZEK Leasing Mietvertrag Description
        /// </summary>
        public ZekLeasingMietvertragDescriptionDto leasingMietvertrag { get; set; }

        /// <summary>
        /// ZEK Teilzahlungskredit Description
        /// </summary>
        public ZekTeilzahlungskreditDescriptionDto teilzahlungskredit { get; set; }

    }
}
