using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Batch Out Data Transfer Object
    /// </summary>
    public class ZekBatchOutDto
    {
        /// <summary>
        /// Getter/Setter Kreditvertrag ID
        /// </summary>
        public string kreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Kreditgesuch ID
        /// </summary>
        public string kreditgesuchID { get; set; }

        /// <summary>
        /// Getter/Setter BatchStatusResponse
        /// Return-Werte aus batchStatus
        /// </summary>
        public ZekBatchStatusResponseDto batchStatusResponse { get; set; }

        /// <summary>
        /// Getter/Setter batchInstructionResponse
        /// Return-Werte aus closeBatch und updateBatch
        /// </summary>
        public ZekBatchInstructionResponseDto batchInstructionResponse { get; set; }
    }
}