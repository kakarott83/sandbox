using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZekBatchStatusResponseDto
    /// </summary>
    public class ZekBatchStatusResponseDto
    {
        /// <summary>
        /// sysId vom ZekBR-Satz für BatchStatusResponse
        /// </summary>
        public long sysZekBR { get; set; }

        /// <summary>
        /// Getter/Setter countProcessedError
        /// </summary>
        public int countProcessedError { get; set; }

        /// <summary>
        /// Getter/Setter countProcessedSuccesfully
        /// </summary>
        public int countProcessedSuccesfully { get; set; }

        /// <summary>
        /// attempts
        /// Zähler für batchStatus-Versuche
        /// </summary>
        public long attempts { get; set; }

        /// <summary>
        /// Getter/Setter errorList
        /// </summary>
        public ZekBatchInstructionErrorDto[] errorList { get; set; }


        /// <summary>
        /// Getter/Setter Transaction Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }

        /// <summary>
        /// Getter/Setter Return Code
        /// </summary>
        public ZekReturnCodeDto ReturnCode { get; set; }
    }
}