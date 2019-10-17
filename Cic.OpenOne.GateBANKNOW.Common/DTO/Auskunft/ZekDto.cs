using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZekBatchStatus (als Antwort aus ZEKBatchRef.batchStatus)
    /// </summary>
    public enum ZekBatchStatus
    {
        /// <summary>
        /// Batch Transaction Pending
        /// </summary>
        Pending = 31,

        /// <summary>
        /// Batch Transaction Processed
        /// </summary>
        Processed = 32
    }

    /// <summary>
    /// ZekActionFlag
    /// </summary>
    public enum ZekActionFlag
    {
        /// <summary>
        /// Inactive
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// ReadyToProcess
        /// </summary>
        ReadyToProcess = 1,

        /// <summary>
        /// InProcess
        /// </summary>
        InProcess = 2,

        /// <summary>
        /// Processed
        /// </summary>
        Processed = 3,

        /// <summary>
        /// Error
        /// </summary>
        Error = 4,

        /// <summary>
        /// Delayed
        /// </summary>
        Delayed = 5
    }

    /// <summary>
    /// ZEK Data Transfer Object
    /// </summary>
    public class ZekDto
    {
        /// <summary>
        /// ZEK.syszek
        /// </summary>
        public long syszek { get; set; }

        /// <summary>
        /// Getter/Setter actionFlag 
        /// </summary>
        public int actionFlag { get; set; }

        /// <summary>
        /// Getter/Setter Kreditgesuch ID
        /// </summary>
        public string rueckmeldeCode { get; set; }

        /// <summary>
        /// Rueckmeldung 
        /// </summary>
        public string rueckmeldung { get; set; }
    }
}