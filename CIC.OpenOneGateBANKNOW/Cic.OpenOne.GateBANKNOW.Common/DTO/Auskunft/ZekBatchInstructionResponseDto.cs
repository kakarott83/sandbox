
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZekBatchInstructionResponseDto
    /// </summary>
    public class ZekBatchInstructionResponseDto
    {
        /// <summary>
        /// Getter/Setter Transaction Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }

        /// <summary>
        /// Getter/Setter Return Code
        /// </summary>
        public ZekReturnCodeDto ReturnCode { get; set; }

        /// <summary>
        /// zekBatchMethode-Name
        /// </summary>
        public string zekBatchMethode { get; set; }
    }
}