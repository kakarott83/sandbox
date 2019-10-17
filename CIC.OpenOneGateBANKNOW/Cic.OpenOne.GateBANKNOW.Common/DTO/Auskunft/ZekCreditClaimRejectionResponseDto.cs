using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Credit Claim Reject Response Data Transfer Object
    /// </summary>
    public class ZekCreditClaimRejectionResponseDto
    {
        /// <summary>
        /// Getter/Setter ZEK Return Code
        /// </summary>
        public ZekReturnCodeDto ReturnCode { get; set; }

        /// <summary>
        /// Getter/Setter Transaction Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }

        /// <summary>
        /// Getter/Setter Synonyms
        /// </summary>
        public List<ZekAddressDescriptionDto> Synonyms { get; set; }
    }
}
